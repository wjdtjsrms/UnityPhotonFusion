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

            selectWorldChatBtn.onClick.AddListener(() => ChatProvider.Instance.ShowChannel("World"));
        }

        public override void Release()
        {
            base.Release();
            selectWorldChatBtn.onClick.RemoveAllListeners();
        }
        #endregion

        #region Method : Member
        public void AddChanel(string chanelName)
        {
            if (itemList.Find((item) => item.Data.serverName.Equals(chanelName)) == null)
                AddItem(new ChatServerModel(chanelName, chanelName.Split(":")[1]));
        }

        public void RemoveChanel(string chanelName)
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