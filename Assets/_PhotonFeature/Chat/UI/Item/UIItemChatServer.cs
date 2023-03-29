namespace JSGCode.Internship.UI
{
    using JSGCode.Internship.DataModel;
    using JSGCode.UI;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIItemChatServer : UIItemList<ChatServerModel>
    {
        #region Member
        [SerializeField] private TextMeshProUGUI userID;
        [SerializeField] private Button selectServerBtn;
        #endregion

        #region Method : UI
        public override void SetItem(ChatServerModel data)
        {
            base.SetItem(data);
            userID.text = data.userID;
        }
        #endregion
    }
}