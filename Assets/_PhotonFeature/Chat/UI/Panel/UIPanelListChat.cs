namespace JSGCode.Internship.UI
{
    using JSGCode.Internship.Chat;
    using JSGCode.Internship.DataModel;
    using JSGCode.UI;
    using System.Collections.Generic;

    public class UIPanelListChat : UIPanelList<UIItemChatMessage, ChatMessageModel>
    {
        #region Method : UI
        public override void Init()
        {
            base.Init();

            ChatProvider.Instance.onChangeChannelText += SetChanelText;
            ChatProvider.Instance.onDisconnect += RemoveAll;
        }
        public override void Release()
        {
            base.Release();

            if (ChatProvider.Instance != null)
            {
                ChatProvider.Instance.onChangeChannelText -= SetChanelText;
                ChatProvider.Instance.onDisconnect -= RemoveAll;
            }
        }
        #endregion

        #region Method : Member
        private void SetChanelText(List<string> messages)
        {
            RemoveAll();

            foreach (var message in messages)
            {
                var messageSplit = message.Split(":");
                AddItem(new ChatMessageModel(messageSplit[0], messageSplit[1]));
            }
        }
        #endregion
    }
}