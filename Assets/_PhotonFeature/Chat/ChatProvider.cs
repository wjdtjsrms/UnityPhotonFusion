namespace JSGCode.Internship.Chat
{
    using JSGCode.Util;
    using Photon.Chat;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ChatProvider : SingletonMonoBehaviour<ChatProvider>, IChatClientListener
    {
        #region Member
        private readonly int historyLengthToFetch = -1;
        private string currentChannelName;
        private ChatClient chatClient;
        private ChatAppSettings chatAppSettings;
        private List<string> currentChannelText;

        private readonly string currentRoomName = "World";
        #endregion

        #region Property
        public string UserName { get; private set; }

        public List<string> CurrentChannelText
        {
            get => currentChannelText;
            set
            {
                currentChannelText = value;
                onChangeChannelText?.Invoke(currentChannelText);
            }
        }
        #endregion

        #region Event
        public Action<List<string>> onChangeChannelText;
        public Action<string> onUserSubscribed;
        public Action<string> onUserUnsubscribed;
        #endregion

        #region Method : Mono
        public void Awake()
        {
            chatAppSettings ??= new ChatAppSettings();
            currentChannelText ??= new();
            chatAppSettings.AppIdChat = "870598ae-744a-4d8d-8728-5b8471e1f9f8";

            Connect();
        }

        public void OnDestroy()
        {
            chatClient?.Disconnect();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            chatClient?.Disconnect();
        }

        public void Update()
        {
            chatClient?.Service();
        }
        #endregion

        #region Method
        public void Connect(string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
                userName = "user" + Environment.TickCount % 99; //made-up username

            this.UserName = userName;

            chatClient ??= new ChatClient(this);
#if !UNITY_WEBGL
            chatClient.UseBackgroundWorkerForSending =  true;
#endif
            chatClient.AuthValues = new AuthenticationValues(this.UserName);
            chatClient.ConnectUsingSettings(chatAppSettings);
        }

        public void SendChatMessage(string inputLine)
        {
            if (string.IsNullOrEmpty(inputLine))
                return;

            bool doingPrivateChat = chatClient.PrivateChannels.ContainsKey(currentChannelName);
            string privateChatTarget = string.Empty;

            if (doingPrivateChat)
            {
                string[] splitNames = currentChannelName.Split(new char[] { ':' });
                privateChatTarget = splitNames[1];
            }

            if (doingPrivateChat)
                chatClient.SendPrivateMessage(privateChatTarget, inputLine);
            else
                chatClient.PublishMessage(currentChannelName, inputLine);
        }

        public void ShowChannel(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
                return;

            bool found = chatClient.TryGetChannel(channelName, out var channel);
            if (!found)
            {
                Debug.Log("ShowChannel failed to find channel: " + channelName);
                return;
            }

            currentChannelName = channelName;
            CurrentChannelText = channel.ToStringListMessage();
        }
        #endregion

        #region IChatClientListener implementation

        public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
        {
            if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
            {
                LogWrapper.LogError(message);
            }
            else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
            {
                LogWrapper.LogWarning(message);
            }
            else
            {
                LogWrapper.Log(message);
            }
        }

        public void OnConnected()
        {
            chatClient.Subscribe(currentRoomName, 0, historyLengthToFetch, creationOptions: new ChannelCreationOptions { PublishSubscribers = true });
            chatClient.SetOnlineStatus(ChatUserStatus.Online);
        }

        public void OnDisconnected()
        {
            Debug.Log("OnDisconnected()");
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            if (channelName.Equals(currentChannelName))
                ShowChannel(currentChannelName);
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            if (UserName.Equals(sender) == false)
                onUserSubscribed?.Invoke(chatClient.GetPrivateChannelNameByUser(sender));

            if (currentChannelName.Equals(channelName))
            {
                ShowChannel(channelName);
            }
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            foreach (string channel in channels)
            {
                chatClient.PublishMessage(channel, "says 'hi'.");
            }

            ShowChannel(channels[0]);
        }

        public void OnUnsubscribed(string[] channels)
        {
            foreach (string channelName in channels)
            {
                if (currentChannelName.Contains(channelName))
                    ShowChannel(currentRoomName);
            }
        }

        public void OnUserSubscribed(string channel, string user)
        {
            onUserSubscribed?.Invoke(chatClient.GetPrivateChannelNameByUser(user));
            chatClient.SendPrivateMessage(user, "Subscribed User", true);
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            onUserUnsubscribed?.Invoke(chatClient.GetPrivateChannelNameByUser(user));

            if (currentChannelName.Contains(user))
                ShowChannel(currentRoomName);
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
        public void OnChatStateChange(ChatState state) { }
        #endregion
    }
}