namespace JSGCode.UI
{
    using UnityEngine;

    public class UIPanel : UIBase
    {
        #region Memberes
        [SerializeField] protected bool setAsSubPanel;
        protected UIPage parentPage;
        #endregion

        #region Properties
        public bool IsSubPanel => setAsSubPanel;
        #endregion

        #region Method : Mono
        private void OnDestroy() { Release(); }
        #endregion

        #region UIBase
        public virtual void Init(UIPage parent)
        {
            parentPage = parent;
            Init();
        }

        public override void Init()
        {
            typeID = gameObject.name.Split('_')[1];
            IsInit = true;
        }

        public override void Release()
        {
            typeID = null;
            IsInit = false;
        }

        public override void Active(bool isActive)
        {
            baseImage?.gameObject.SetActive(isActive);
        }
        #endregion

        #region Method
        public virtual void SetPanel(System.Action finishCallback)
        {
            finishCallback?.Invoke();
        }
        #endregion
    }
}
