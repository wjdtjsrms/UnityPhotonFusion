namespace JSGCode.Internship.Chat
{
    using JSGCode.Util;
    using Photon.Chat;
    using Photon.Chat.Demo;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChatProvider : SingletonMonoBehaviour<ChatProvider>, IChatClientListener
    {
        #region Member
        public string[] ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.

        public string[] FriendsList;

        private int historyLengthToFetch = -1; // Get All Histroy

        public string userName { get; private set; }

        private string selectedChannelName;

        private ChatClient chatClient;

        private ChatAppSettings chatAppSettings;

        public List<string> currentChannelText = new();
        public List<string> CurrentChannelText
        {
            get
            {
                return currentChannelText;
            }
            set
            {
                currentChannelText = value;
                onChangeChannelText?.Invoke(currentChannelText);
            }
        }
        public Toggle ChannelToggleToInstantiate; // set in inspector

        public Action<List<string>> onChangeChannelText;
        public Action<string> onUserSubscribed;
        public Action<string> onUserUnsubscribed;

        public GameObject FriendListUiItemtoInstantiate;

        private readonly Dictionary<string, Toggle> channelToggles = new();

        private readonly Dictionary<string, FriendItem> friendListItemLUT = new();

        public bool ShowState = true;
        [SerializeField] private ChatState chatState;

        public int TestLength = 2048;
        private byte[] testBytes = new byte[2048];
        #endregion

        #region Method : Mono
        public void Awake()
        {
            chatState = ChatState.Uninitialized;

            chatAppSettings ??= new ChatAppSettings();
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

            this.userName = userName;

            chatClient ??= new ChatClient(this);
#if !UNITY_WEBGL
            chatClient.UseBackgroundWorkerForSending = true;
#endif
            chatClient.AuthValues = new AuthenticationValues(this.userName);
            chatClient.ConnectUsingSettings(chatAppSettings);
        }

        public void OnEnterSend(string message)
        {
            SendChatMessage(message);
        }

        private void SendChatMessage(string inputLine)
        {
            if (string.IsNullOrEmpty(inputLine))
            {
                return;
            }
            if ("test".Equals(inputLine))
            {
                if (this.TestLength != this.testBytes.Length)
                {
                    this.testBytes = new byte[this.TestLength];
                }

                this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
            }


            bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelName);
            string privateChatTarget = string.Empty;
            if (doingPrivateChat)
            {
                // the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
                // so the remote ID is simple to figure out

                string[] splitNames = this.selectedChannelName.Split(new char[] { ':' });
                privateChatTarget = splitNames[1];
            }
            //UnityEngine.Debug.Log("selectedChannelName: " + selectedChannelName + " doingPrivateChat: " + doingPrivateChat + " privateChatTarget: " + privateChatTarget);

            if (doingPrivateChat)
            {
                chatClient.SendPrivateMessage(privateChatTarget, inputLine);
            }
            else
            {
                chatClient.PublishMessage(selectedChannelName, inputLine);
            }
        }

        private void InstantiateChannelButton(string channelName)
        {
            if (this.channelToggles.ContainsKey(channelName))
            {
                Debug.Log("Skipping creation for an existing channel toggle.");
                return;
            }
            //Toggle cbtn = Instantiate(this.ChannelToggleToInstantiate);
            //cbtn.gameObject.SetActive(true);
            //cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
            //cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);

            //this.channelToggles.Add(channelName, cbtn);
        }

        private void InstantiateFriendButton(string friendId)
        {
            GameObject fbtn = Instantiate(this.FriendListUiItemtoInstantiate);
            fbtn.gameObject.SetActive(true);
            FriendItem _friendItem = fbtn.GetComponent<FriendItem>();

            _friendItem.FriendId = friendId;

            fbtn.transform.SetParent(this.FriendListUiItemtoInstantiate.transform.parent, false);

            this.friendListItemLUT[friendId] = _friendItem;
        }

        public void ShowChannel(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                return;
            }

            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            if (!found)
            {
                Debug.Log("ShowChannel failed to find channel: " + channelName);
                return;
            }

            this.selectedChannelName = channelName;
            CurrentChannelText = channel.ToStringListMessage();
            Debug.Log("ShowChannel: " + this.selectedChannelName);

            foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
            {
                pair.Value.isOn = pair.Key == channelName ? true : false;
            }
        }
        #endregion

        #region IChatClientListener implementation

        public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
        {
            if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
            {
                Debug.LogError(message);
            }
            else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }

        public void OnDisconnected()
        {
            Debug.Log("OnDisconnected()");

        }

        public void OnConnected()
        {
            if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
            {
                this.chatClient.Subscribe("World", 0, this.historyLengthToFetch, creationOptions: new ChannelCreationOptions { PublishSubscribers = true });
            }

            if (this.FriendsList != null && this.FriendsList.Length > 0)
            {
                this.chatClient.AddFriends(this.FriendsList); // Add some users to the server-list to get their status updates

                // add to the UI as well
                foreach (string _friend in this.FriendsList)
                {
                    if (this.FriendListUiItemtoInstantiate != null && _friend != this.userName)
                    {
                        this.InstantiateFriendButton(_friend);
                    }

                }

            }

            if (this.FriendListUiItemtoInstantiate != null)
            {
                this.FriendListUiItemtoInstantiate.SetActive(false);
            }


            this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
        }

        public void OnChatStateChange(ChatState state)
        {
            // use OnConnected() and OnDisconnected()
            // this method might become more useful in the future, when more complex states are being used.
            chatState = state;
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            if (channelName.Equals(selectedChannelName))
                ShowChannel(selectedChannelName);
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            // as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
            // you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg
            if (userName.Equals(sender) == false)
            {
                onUserSubscribed?.Invoke(chatClient.GetPrivateChannelNameByUser(sender));
            }

            byte[] msgBytes = message as byte[];
            if (msgBytes != null)
            {
                Debug.Log("Message with byte[].Length: " + msgBytes.Length);
            }

            Debug.LogWarning(channelName);
            if (this.selectedChannelName.Equals(channelName))
            {
                this.ShowChannel(channelName);
            }
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            // in this demo, we simply send a message into each channel. This is NOT a must have!
            foreach (string channel in channels)
            {
                this.chatClient.PublishMessage(channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.

                if (this.ChannelToggleToInstantiate != null)
                {
                    this.InstantiateChannelButton(channel);

                }
            }

            Debug.Log("OnSubscribed: " + string.Join(", ", channels));

            // Switch to the first newly created channel
            this.ShowChannel(channels[0]);
        }

        public void OnUnsubscribed(string[] channels)
        {
            foreach (string channelName in channels)
            {
                if (this.channelToggles.ContainsKey(channelName))
                {
                    Toggle t = this.channelToggles[channelName];
                    Destroy(t.gameObject);

                    this.channelToggles.Remove(channelName);

                    Debug.Log("Unsubscribed from channel '" + channelName + "'.");

                    // Showing another channel if the active channel is the one we unsubscribed from before
                    if (channelName == this.selectedChannelName && this.channelToggles.Count > 0)
                    {
                        IEnumerator<KeyValuePair<string, Toggle>> firstEntry = this.channelToggles.GetEnumerator();
                        firstEntry.MoveNext();

                        this.ShowChannel(firstEntry.Current.Key);

                        firstEntry.Current.Value.isOn = true;
                    }
                }
                else
                {
                    Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
                }
            }
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));

            if (this.friendListItemLUT.ContainsKey(user))
            {
                FriendItem _friendItem = this.friendListItemLUT[user];
                if (_friendItem != null) _friendItem.OnFriendStatusUpdate(status, gotMessage, message);
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

            if (selectedChannelName.Contains(user))
                ShowChannel("World");
        }
        #endregion
    }
}