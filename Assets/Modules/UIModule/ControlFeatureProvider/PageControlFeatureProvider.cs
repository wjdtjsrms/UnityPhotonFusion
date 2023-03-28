namespace JSGCode.UI
{
    using JSGCode.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PageControlFeatureProvider<T> where T : Enum
    {
        #region Member : Page
        private List<UIPageController> pageControllers = new List<UIPageController>();
        private Stack<UIPage> additivePageStack = new Stack<UIPage>();

        private UIPage currentPage;
        private UIPage prevPage;

        private bool isInit;
        #endregion

        #region Property
        public UIPage CurrentPage => currentPage;
        public UIPage PrevPage => prevPage;
        public bool IsInit => isInit;
        #endregion

        #region Method : Init
        public void Init()
        {
            isInit = true;
        }

        public void Release()
        {
            pageControllers.Clear();
            pageControllers = null;

            additivePageStack.Clear();
            additivePageStack = null;

            isInit = false;
        }
        #endregion

        #region Methods : Register
        public void RegisterPageController(UIPageController controller)
        {
            if (!pageControllers.Contains(controller))
                pageControllers.Add(controller);
        }

        public void UnRegisterPageController(UIPageController controller)
        {
            pageControllers.Remove(controller);
        }
        #endregion

        #region Method : Page
        public void OpenPage(T page, PageOpenMode openMode)
        {
            if (TryGetPageController(page, out var pageController))
            {
                LogWrapper.Log(CustomLogType.UI, "Open page : " + page + ", " + openMode);

                if (currentPage != null && currentPage.OpenMode != PageOpenMode.Additive)
                    prevPage = currentPage;

                if (prevPage != null && !prevPage.Equals(currentPage))
                    DisposePageByOpenMode();

                currentPage = pageController.OpenPage(page.ToString(), openMode);
            }
            else
                LogWrapper.LogWarning(CustomLogType.Page, "Can't find page controller which has page type " + page);
        }

        public void OpenPage(UICommandPage<T> pageCommand)
        {
            LogWrapper.Log(CustomLogType.UI, "Open page : " + pageCommand.pageType + ", " + pageCommand.mode);

            if (TryGetPageController(pageCommand.pageType, out var pageController))
            {
                if (pageCommand.mode != PageOpenMode.Popup)
                {
                    if (currentPage != null && currentPage.OpenMode != PageOpenMode.Additive)
                        prevPage = currentPage;

                    currentPage = pageController.OpenPage(pageCommand.pageType.ToString(),
                                                              new UIPageDescription()
                                                              {
                                                                  data = pageCommand.dataDic,
                                                                  openMode = pageCommand.mode,
                                                                  callbacks = pageCommand.callbackDic,
                                                                  reusltCallback = pageCommand.resultCallback
                                                              });

                    if (!currentPage.Equals(prevPage))
                        DisposePageByOpenMode();
                }
                else
                    pageController.OpenPage(pageCommand.pageType.ToString(),
                                                              new UIPageDescription()
                                                              {
                                                                  data = pageCommand.dataDic,
                                                                  openMode = pageCommand.mode,
                                                                  callbacks = pageCommand.callbackDic,
                                                                  reusltCallback = pageCommand.resultCallback
                                                              });

            }
            else
                LogWrapper.LogWarning(CustomLogType.Page, "Can't find page controller which has page type " + pageCommand.pageType);

            pageCommand.Finish();
        }

        public bool TryGetPageController(T targetPage, out UIPageController pageController)
        {
            pageController = pageControllers.Find((controller) => controller.IsPageVaild(targetPage.ToString()));
            return pageController != null;
        }

        private void DisposePageByOpenMode()
        {
            switch (currentPage.OpenMode)
            {
                case PageOpenMode.Convert:
                    prevPage.Close();
                    break;
                case PageOpenMode.Only:
                    CloseAllPages();
                    break;
                case PageOpenMode.Additive:
                    additivePageStack.Push(currentPage);
                    LogWrapper.Log(CustomLogType.Page, "page : stack pushed : " + currentPage.gameObject.name + ", count : " + additivePageStack.Count);
                    break;
            }
        }

        public void ReturnToPrevPage(bool bRefresh = false)
        {
            switch (currentPage.OpenMode)
            {
                case PageOpenMode.Additive:
                    if (additivePageStack.Count != 0)
                    {
                        CloseCurrentPage();

                        if (additivePageStack.Count != 0)
                        {
                            currentPage = additivePageStack.Peek();

                            if (bRefresh)
                                currentPage.Open(currentPage.OpenMode);
                        }
                        else
                        {
                            if (prevPage != null)
                            {
                                currentPage = prevPage;
                                currentPage.Open(currentPage.OpenMode);
                                prevPage = null;
                            }
                        }
                    }
                    else
                        Debug.LogWarning("No return pages on stack");
                    break;
                case PageOpenMode.Convert:
                    if (prevPage != null)
                    {
                        currentPage = prevPage;
                        currentPage.Open(currentPage.OpenMode);
                        prevPage = null;
                    }
                    break;
                case PageOpenMode.Popup:
                    CloseCurrentPage();
                    break;
            }
        }

        public void CloseCurrentPage()
        {
            if (currentPage != null)
            {
                LogWrapper.Log(CustomLogType.Page, "open page : close current page : " + currentPage.gameObject.name);
                if (currentPage.OpenMode == PageOpenMode.Additive && additivePageStack.Count != 0 && currentPage.Equals(additivePageStack.Peek()))
                    additivePageStack.Pop();

                currentPage.Close();
                currentPage = null;
            }
        }

        public void CloseAllPages()
        {
            LogWrapper.Log(CustomLogType.Page, "Open page : Close all pages");

            CloseCurrnetPage();
            ClosePrevPage();
            CloseStackedPages();
        }

        private void CloseCurrnetPage()
        {
            LogWrapper.Log(CustomLogType.Page, "Open page : Close current page");

            if (currentPage != null)
            {
                currentPage.Close();
                currentPage = null;
            }
        }

        public void ClosePrevPage()
        {
            LogWrapper.Log(CustomLogType.Page, "Open page : Close prev page");
            if (prevPage != null)
            {
                prevPage.Close();
                prevPage = null;
            }
        }

        public void CloseStackedPages()
        {
            LogWrapper.Log(CustomLogType.Page, "Open page : Close stacked pages " + additivePageStack.Count);
            while (additivePageStack.Count > 0)
            {
                CloseStackedPage();
            }
        }

        public void CloseStackedPage()
        {
            if (additivePageStack.Count > 0)
            {
                var page = additivePageStack.Pop();
                LogWrapper.Log(CustomLogType.Page, "Open page : Close stacked page " + page.name);
                page?.Close();
            }
        }
        #endregion

        #region Methods : Util
        public void SetUIVisibility(bool visible)
        {
            foreach (var controller in pageControllers)
                controller.SetVisibility(visible);
        }

        public UIPage GetPage(string pageName)
        {
            foreach (var controller in pageControllers)
            {
                var page = controller.GetPage(pageName);

                if (page != null)
                    return page;
            }

            return null;
        }
        #endregion
    }
}