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
        }
        public override void Release()
        {
            base.Release();
            ChatProvider.Instance.onChangeChannelText -= SetChanelText;
        }
        #endregion

        #region Method : Member
        private void SetChanelText(List<string> messages)
        {
            RemoveAll();

            foreach (var message in messages)
            {
                var sender = message.Split(":")[0];
                var content = message.Split(":")[1];

                AddItem(new ChatMessageModel(sender, content));
            }
        }
        #endregion
    }
}