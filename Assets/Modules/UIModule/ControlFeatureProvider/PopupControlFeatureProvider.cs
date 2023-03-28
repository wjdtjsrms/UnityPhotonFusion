namespace JSGCode.UI
{
    using System.Collections.Generic;

    public class PopupControlFeatureProvider
    {
        #region Member : Popup
        private UIPopupController popupController;
        private Stack<UIPopup> popupStack = new Stack<UIPopup>();
        #endregion

        #region Method : Init
        public void Init()
        {

        }

        public void Release()
        {
            popupStack.Clear();
            popupStack = null;
        }
        #endregion

        #region Methods : Register
        public void RegisterPopupController(UIPopupController controller)
        {
            popupController = controller;
        }

        public void UnRegisterPopupController()
        {
            popupController = null;
        }
        #endregion

        #region Method : Control Popup 
        public void OpenPopup(UICommandPopup popupCommand)
        {
            if (popupCommand.popupType == PopupType.None)
            {
                ClosePopup();
                popupCommand.Finish();
                return;
            }

            var currentPopup = popupController.OpenPopup(popupCommand.popupType.ToString(),
                                                            new UIPopupDescription()
                                                            {
                                                                title = popupCommand.title,
                                                                callbacks = popupCommand.callbackDic,
                                                                data = popupCommand.dataDic
                                                            });

            currentPopup.SetFunctionOnClose(() => popupCommand.Finish());
            popupStack.Push(currentPopup);
        }

        public void ClosePopup()
        {
            if (popupStack.Count < 1)
                return;

            var recentPopup = popupStack.Pop();
            popupController.ClosePopup(recentPopup);
        }

        public void ShowToast(string msg, float duration = 0, UIToastPosition position = UIToastPosition.Upper, System.Action finishCallback = null)
        {
            popupController?.ShowToast(msg, duration, position, finishCallback);
        }
        #endregion
    }
}