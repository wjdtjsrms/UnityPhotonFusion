namespace JSGCode.UI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UIPopup : UIBase, IDisposable
    {
        #region Members
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected Button closeBtn;
        #endregion

        #region Methods
        public virtual void SetFunctionOnClose(System.Action func)
        {
            closeBtn?.onClick.AddListener(() =>
            {
                func?.Invoke();
                Active(false);
            });
        }

        public virtual void ExcuteData(Dictionary<string, object> dataDic) { }
        public virtual void SetCallbacks(Dictionary<string, System.Action> callbacks) { }

        public virtual void SetTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
                this.title.text = title;
        }

        public virtual void Dispose()
        {
            closeBtn?.onClick.RemoveAllListeners();
        }
        #endregion

        #region Overriding : UIBase
        public override void Init()
        {
            closeBtn?.onClick.AddListener(() => Active(false));
        }

        public override void Release()
        {
            closeBtn?.onClick.RemoveAllListeners();
        }

        public override void Active(bool isActive)
        {
            title?.gameObject.SetActive(isActive);
            baseImage?.gameObject.SetActive(isActive);

            if (!isActive)
                Dispose();
        }
        #endregion
    }
}