namespace JSGCode.Internship.UI
{
    using JSGCode.UI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

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
        }
        #endregion
    }
}