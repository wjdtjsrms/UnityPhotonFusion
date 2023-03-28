namespace JSGCode.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class UIBase : MonoBehaviour
    {
        #region Members : UI Elements
        [SerializeField] protected Image baseImage;
        [SerializeField] protected string typeID;
        #endregion

        #region Property
        public virtual string TypeID => typeID;
        #endregion

        #region Members
        public bool IsInit { get; protected set; }
        #endregion

        #region Methods
        public abstract void Init();
        public abstract void Release();
        public abstract void Active(bool isActive);
        #endregion
    }
}