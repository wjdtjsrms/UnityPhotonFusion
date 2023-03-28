namespace JSGCode.UI
{
    using UnityEngine;

    public abstract class UIController : MonoBehaviour
    {
        public bool IsInit { get; protected set; } = false;

        public abstract void Init();

        public abstract void Release();
    }
}
