namespace JSGCode.UI
{
    public class UIPanelData<T> : UIPanel
    {
        public T Data { get; protected set; }
        public virtual void SetData(T data) => Data = data;

        public override void Release()
        {
            base.Release();
            Data = default;
        }
    }
}

