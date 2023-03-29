namespace JSGCode.Internship.UI
{
    using JSGCode.UI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UIPageChatMain : UIPage
    {
        #region Method : Test
        private void Awake()
        {
            Init();
        }
        #endregion

        #region Method : UI Base
        public override void Init()
        {
            //typeID = PageType.SelectAvatar.ToString();
            base.Init();
        }
        #endregion
    }
}