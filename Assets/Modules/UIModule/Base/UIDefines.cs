namespace JSGCode.UI
{
    public enum PageOpenMode
    {
        Convert, Additive, Popup, Only
    }

    public enum PopupType
    {
        None, CustomOneButton, CustomTwoButton, Toast
    }

    public enum UIToastPosition 
    {
        Upper, Center, Lower 
    }

    public class UIStringValues
    {
        #region Keys
        public static readonly string Data = "data";
        public static readonly string Content = "content";
        public static readonly string ContentType = "contentType";
        public static readonly string Title = "title";
        #endregion
    }

    public class CallbackValues
    {
        public static readonly string OneButtonCallbackKey = "firstCallback";
        public static readonly string TwoButtonCallbackKey = "secondCallback";
        public static readonly string BackBUttonCallbackKey = "backButtonCallback";
    }
}