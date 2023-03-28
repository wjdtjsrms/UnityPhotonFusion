namespace JSGCode.UI
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIPopupTwoButton : UIPopupOneButton
    {
        #region Members : UI Elements
        [SerializeField] protected Button secondButton;
        #endregion

        #region Method : Overriding
        public override void Init()
        {
            base.Init();
            typeID = PopupType.CustomTwoButton.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            secondButton?.onClick.RemoveAllListeners();
        }

        public override void ExcuteData(Dictionary<string, object> dataDic)
        {
            base.ExcuteData(dataDic);

            if (dataDic.ContainsKey(CallbackValues.TwoButtonCallbackKey))
                secondButton.GetComponentInChildren<TextMeshProUGUI>().text = dataDic[CallbackValues.TwoButtonCallbackKey] as string;
        }
        public override void SetCallbacks(Dictionary<string, System.Action> callbacks)
        {
            base.SetCallbacks(callbacks);

            if (callbacks.ContainsKey(CallbackValues.TwoButtonCallbackKey))
                secondButton.onClick.AddListener(() =>
                {
                    callbacks[CallbackValues.TwoButtonCallbackKey]?.Invoke();
                    Active(false);
                });
        }
        #endregion
    }
}