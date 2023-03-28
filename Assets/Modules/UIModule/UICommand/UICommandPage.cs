namespace JSGCode.UI
{
    using System;

    public class UICommandPage<T> : UICommand where T : Enum
    {
        #region Members
        public T pageType { get; private set; }
        public PageOpenMode mode { get; private set; }

        public Action<UICommandPage<T>> onFinishCommand;
        #endregion

        #region Constructors
        public UICommandPage(T pageType, PageOpenMode mode = PageOpenMode.Convert) : base()
        {
            this.pageType = pageType;
            this.mode = mode;
        }

        public UICommandPage(T pageType, Action<UICommandPage<T>> finishCallback, PageOpenMode mode = PageOpenMode.Convert) : this(pageType, mode)
        {
            onFinishCommand = finishCallback;
        }
        #endregion

        #region Overrdings
        public override void Finish()
        {
            onFinishCommand?.Invoke(this);
            Dispose();
        }

        public override void Dispose()
        {
            onFinishCommand = null;
        }
        #endregion
    }
}