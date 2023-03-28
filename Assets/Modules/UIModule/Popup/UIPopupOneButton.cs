namespace JSGCode.UI
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIPopupOneButton : UIPopup
    {
        #region Members : UI Elements
        [SerializeField] protected Button firstButton;
        [SerializeField] protected TextMeshProUGUI content;
        [SerializeField] protected Image blockCutton;
        #endregion

        #region Methods : Member
        protected virtual void SetContent(string content)
        {
            this.content.text = content;
        }
        #endregion

        #region Method : Overriding
        public override void Init()
        {
            base.Init();
            typeID = PopupType.CustomOneButton.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            firstButton?.onClick.RemoveAllListeners();
        }

        public override void Active(bool isActive)
        {
            base.Active(isActive);
            blockCutton?.gameObject.SetActive(isActive);

            if (isActive == false)
                Dispose();

            //UIManager.Instance.PopupController.SetBlockCutton(isActive);
        }

        public override void ExcuteData(Dictionary<string, object> dataDic)
        {
            if (dataDic.ContainsKey(UIStringValues.Content))
                SetContent(dataDic[UIStringValues.Content] as string);

            if (dataDic.ContainsKey(UIStringValues.Title))
                SetTitle(dataDic[UIStringValues.Title] as string);

            if (dataDic.ContainsKey(CallbackValues.OneButtonCallbackKey))
                firstButton.GetComponentInChildren<TextMeshProUGUI>().text = dataDic[CallbackValues.OneButtonCallbackKey] as string;
        }

        public override void SetCallbacks(Dictionary<string, System.Action> callbacks)
        {
            if (callbacks.ContainsKey(CallbackValues.OneButtonCallbackKey))
                firstButton?.onClick.AddListener(() =>
                {
                    callbacks[CallbackValues.OneButtonCallbackKey]?.Invoke();
                    Active(false);
                });
        }
        #endregion
    }
}