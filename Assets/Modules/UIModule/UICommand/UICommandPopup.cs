namespace JSGCode.UI
{
    public class UICommandPopup : UICommand
    {
        #region Members
        public PopupType popupType { get; private set; }
        #endregion

        #region Constructor
        public UICommandPopup(PopupType popupType) : base()
        {
            this.popupType = popupType;
        }
        #endregion
    }
}