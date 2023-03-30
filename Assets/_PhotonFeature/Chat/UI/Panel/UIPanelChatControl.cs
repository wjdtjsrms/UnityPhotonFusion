namespace JSGCode.Internship.UI
{
    using JSGCode.Internship.Chat;
    using JSGCode.Internship.DataModel;
    using JSGCode.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIPanelChatControl : UIPanelList<UIItemChatServer, ChatServerModel>
    {
        #region Member
        [SerializeField] private Button selectWorldChatBtn;
        [SerializeField] private Button exitBtn;
        #endregion

        #region Method : UI
        public override void Init()
        {
            base.Init();
            ChatProvider.Instance.onUserSubscribed += AddChanel;
            ChatProvider.Instance.onUserUnsubscribed += RemoveChanel;
            ChatProvider.Instance.onDisconnect += RemoveAll;

            selectWorldChatBtn.onClick.AddListener(ChatProvider.Instance.ShowRoomChanel);
        }

        public override void Release()
        {
            base.Release();

            if (ChatProvider.Instance != null)
            {
                ChatProvider.Instance.onUserSubscribed -= AddChanel;
                ChatProvider.Instance.onUserUnsubscribed -= RemoveChanel;
                ChatProvider.Instance.onDisconnect -= RemoveAll;
            }

            selectWorldChatBtn.onClick.RemoveAllListeners();
        }
        #endregion

        #region Method : Member
        private void AddChanel(string chanelName)
        {
            if (itemList.Find((item) => item.Data.serverName.Equals(chanelName)) == null)
                AddItem(new ChatServerModel(chanelName, chanelName.Split(":")[1]));
        }

        private void RemoveChanel(string chanelName)
        {
            foreach (var item in itemList.ToArray())
            {
                if (item.Data.serverName.Equals(chanelName))
                    RemoveItem(item);
            }
        }
        #endregion
    }
}