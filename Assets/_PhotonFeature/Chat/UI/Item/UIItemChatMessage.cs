namespace JSGCode.Internship.UI
{
    using JSGCode.Internship.Chat;
    using JSGCode.Internship.DataModel;
    using JSGCode.UI;
    using TMPro;
    using UnityEngine;

    public class UIItemChatMessage : UIItemList<ChatMessageModel>
    {
        #region Member
        [SerializeField] private TextMeshProUGUI message;
        #endregion

        #region Method : UI
        public override void SetItem(ChatMessageModel data)
        {
            base.SetItem(data);
            message.text = $"{data.sender}: {data.meesage}";
            message.color = data.sender == ChatProvider.Instance.userName ? Color.black : Color.white;
        }
        #endregion
    }
}