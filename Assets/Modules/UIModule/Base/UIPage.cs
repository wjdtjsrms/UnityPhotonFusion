namespace JSGCode.UI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UIPage : UIBase
    {
        #region Members
        protected List<UIPanel> panelList;
        protected UIPageController parentController;
        #endregion

        #region Property
        public PageOpenMode OpenMode { get; private set; }
        public float currentWidth => GetComponent<RectTransform>().rect.width;
        #endregion

        #region Method : Mono
        private void OnDestroy() { Release(); }
        #endregion

        #region Method : UIBase
        public override void Init()
        {
            panelList = new List<UIPanel>();

            foreach (var panel in GetComponentsInChildren<UIPanel>(true))
            {
                if (panel.IsSubPanel)
                    continue;

                panel.Init(this);
                panelList.Add(panel);
            }

            IsInit = true;
        }

        public virtual void Init(UIPageController parent)
        {
            parentController = parent;
            Init();
        }

        public override void Active(bool isActive)
        {
            baseImage?.gameObject.SetActive(isActive);

            if (panelList != null)
            {
                foreach (var panel in panelList)
                    panel.Active(isActive);
            }
        }

        public override void Release()
        {
            panelList?.Clear();
            panelList = null;
            parentController = null;
        }
        #endregion

        #region Method : Open & Close
        public virtual void Open(PageOpenMode openMode)
        {
            OpenMode = openMode;
            Active(true);
        }
        public virtual void Close()
        {
            Active(false);
        }
        #endregion

        #region Method
        public virtual void ExcuteData(Dictionary<string, object> data) { }
        public virtual void SetCallbacks(Dictionary<string, Action> callbacks) { }
        public virtual void SetResultCallback(Action<object> callback) { }
        #endregion

        #region Method : Helper
        protected virtual TPanel GetPanel<TPanel>() where TPanel : UIPanel
        {
            return panelList.Find((x) => x.GetType() == typeof(TPanel)) as TPanel;
        }
        #endregion
    }
}