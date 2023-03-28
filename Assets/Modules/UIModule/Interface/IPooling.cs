namespace JSGCode.UI
{
    public interface IPooling<T>
    {
        void Release(T obj);
        T Get();
        void Clear();
    }
}