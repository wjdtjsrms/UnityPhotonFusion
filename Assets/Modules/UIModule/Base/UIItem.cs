namespace JSGCode.UI
{
    public class UIItem<T> : UIBase, ISelectable
    {
        #region Members : Data
        protected T data;
        #endregion

        #region Property
        public T Data => data;
        #endregion

        #region Methods
        public virtual void SetItem(T data)
        {
            this.data = data;
        }
        #endregion

        #region Overriding
        public override void Init() { }

        public override void Release()
        {
            data = default;
        }

        public override void Active(bool isActive) { }
        #endregion

        #region Methods : Selectable
        public virtual void OnSelect() { }

        public virtual void OnDeselect() { }
        #endregion
    }
}