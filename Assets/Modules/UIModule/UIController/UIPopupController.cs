namespace JSGCode.UI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIPopupDescription
    {
        public string title;
        public string content;
        public Dictionary<string, System.Action> callbacks;
        public Dictionary<string, object> data;
    }

    public class UIPopupController : UIController
    {
        #region Members
        [SerializeField] protected Image blockCutton;
        #endregion

        #region Members
        private Dictionary<string, UIPopup> popupDic;
        private UIToast toastPopup;
        #endregion

        #region Method : Mono
        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            Release();
        }
        #endregion

        #region Method : UI Controller
        public override void Init()
        {
            popupDic = new Dictionary<string, UIPopup>();

            foreach (var popup in GetComponentsInChildren<UIPopup>(true))
            {
                popup.Init();
                popupDic.Add(popup.TypeID, popup);
            }

            toastPopup = GetComponentInChildren<UIToast>(true);
            toastPopup?.Init();

            IsInit = true;
        }

        public override void Release()
        {
            popupDic?.Clear();
            popupDic = null;

            IsInit = false;
        }
        #endregion

        #region Method : Controll Popup
        public UIPopup OpenPopup(string id, UIPopupDescription description)
        {
            var popup = popupDic[id];
            popup.SetTitle(description.title);
            popup.ExcuteData(description.data);
            popup.SetCallbacks(description.callbacks);
            popup?.Active(true);

            return popup;
        }

        public void ClosePopup(UIPopup popup)
        {
            popup.Active(false);
            popup.Dispose();
        }

        public void SetBlockCutton(bool isSet)
        {
            blockCutton.gameObject.SetActive(isSet);
        }
        #endregion

        #region Method : Controll Toast
        public void ShowToast(string msg, float duration = 0, UIToastPosition position = UIToastPosition.Upper, System.Action finishCallback = null)
        {
            toastPopup?.ShowToast(duration, msg, position, finishCallback);
        }
        #endregion
    }
}