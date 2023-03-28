namespace JSGCode.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class UIPageDescription
    {
        public string pageType;
        public string category;
        public PageOpenMode openMode;
        public Dictionary<string, object> data;
        public Dictionary<string, System.Action> callbacks;
        public System.Action<object> reusltCallback;
    }

    public class UIPageController : UIController
    {
        #region Members
        [SerializeField] private bool exceptVisibilityOption;
        protected Dictionary<string, UIPage> pageDic = new Dictionary<string, UIPage>();
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

        #region Method : Override
        public override void Init()
        {
            foreach (var page in GetComponentsInChildren<UIPage>())
            {
                page.Init(this);
                pageDic.Add(page.TypeID, page);
            }

            IsInit = true;
        }

        public override void Release()
        {
            foreach (var page in pageDic.Values)
                page.Release();

            pageDic.Clear();
        }
        #endregion

        #region Method : Helper
        public bool IsPageVaild(string pageID)
        {
            return pageDic.ContainsKey(pageID);
        }

        public void SetVisibility(bool visible)
        {
            if (exceptVisibilityOption)
                return;

            if (TryGetComponent<Canvas>(out var canvas))
            {
                canvas.enabled = visible;
            }
        }

        public UIPage GetPage(string page)
        {
            if (IsPageVaild(page))
                return pageDic[page];

            return null;
        }
        #endregion

        #region Method : Page Open & Close
        public virtual UIPage OpenPage(string page, PageOpenMode openMode)
        {
            var pageToOpen = GetPage(page);
            pageToOpen.Open(openMode);

            return pageToOpen;
        }

        public virtual UIPage OpenPage(string page, UIPageDescription description = null)
        {
            var pageToOpen = GetPage(page);
            pageToOpen.Open(description.openMode);

            if (description != null)
            {
                pageToOpen.ExcuteData(description.data);
                pageToOpen.SetCallbacks(description.callbacks);
                pageToOpen.SetResultCallback(description.reusltCallback);
            }

            return pageToOpen;
        }

        public virtual void ClosePage(string page)
        {
            var pageToClose = GetPage(page);
            pageToClose.Close();
        }
        #endregion
    }
}