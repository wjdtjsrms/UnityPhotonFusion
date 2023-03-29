namespace JSGCode.Internship.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using JSGCode.UI;
    using JSGCode.Internship.DataModel;
    using UnityEngine.UI;

    public class UIPanelChatControl : UIPanelList<UIItemChatServer, ChatServerModel>
    {
        #region Member
        [SerializeField] private Button selectWorldChatBtn;
        [SerializeField] private Button exitBtn;
        #endregion
    }
}