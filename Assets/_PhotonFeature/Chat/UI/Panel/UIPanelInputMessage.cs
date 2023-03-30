namespace JSGCode.Internship.UI
{
    using JSGCode.Internship.Chat;
    using JSGCode.UI;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIPanelInputMessage : UIPanel
    {
        #region Member
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendMessageBtn;
        #endregion

        #region Method : UI
        public override void Init()
        {
            base.Init();

            sendMessageBtn.onClick.AddListener(() =>
            {
                ChatProvider.Instance.OnEnterSend(inputField.text);
                inputField.text = "";
            });
        }
        #endregion
    }
}