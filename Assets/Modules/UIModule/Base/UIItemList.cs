namespace JSGCode.UI
{
    using JSGCode.Util;
    using UnityEngine.EventSystems;

    public class UIItemList<T> : UIItem<T>, IDataObserver<T>, IPointerClickHandler where T : new()
    {
        #region Delegate
        public System.Action<UIItemList<T>> OnClickItem;
        #endregion

        #region Implementation : IDataObserver
        public virtual void Notify(T data) { SetItem(data); }

        public virtual void OnPointerClick(PointerEventData eventData) { OnSelect(); }

        public override void Release()
        {
            base.Release();
            OnClickItem = null;
        }
        #endregion
    }
}