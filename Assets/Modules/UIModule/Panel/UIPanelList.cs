namespace JSGCode.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class UIPanelList<T, U> : UIPanel, IPooling<GameObject> where T : UIItem<U>
    {
        #region Members : Editor
        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] protected Transform poolingRoot;
        [SerializeField] protected RectTransform listRoot;
        #endregion

        #region Members
        protected List<UIItem<U>> itemList = new List<UIItem<U>>();
        #endregion

        #region Methods
        public virtual UIItem<U> AddItem(U data)
        {
            var item = Get().GetComponent<UIItem<U>>();
            item.transform.SetParent(listRoot);
            item.SetItem(data);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            itemList.Add(item);

            return item;
        }

        public virtual void RemoveItem(UIItem<U> itemToRemove)
        {
            Release(itemToRemove.gameObject);
            itemList.Remove(itemToRemove);
        }

        public virtual void RemoveAll()
        {
            if (itemList.Count != 0)
            {
                while (itemList.Count != 0)
                    RemoveItem(itemList[0]);
            }
        }

        public virtual void ClickItem(UIItem<U> item) { }
        #endregion

        #region Method : Override 
        public override void Active(bool isActive)
        {
            base.Active(isActive);

            if (isActive == false)
                RemoveAll();
        }
        #endregion

        #region Implementation : IPooling
        public virtual GameObject Get()
        {
            if (poolingRoot.childCount != 0)
            {
                var obj = poolingRoot.GetChild(0).gameObject;
                obj.SetActive(true);

                return obj;
            }

            return Instantiate(itemPrefab);
        }

        public virtual void Release(GameObject item)
        {
            item.transform.SetParent(poolingRoot);
            item.gameObject.SetActive(false);
        }

        public virtual void Clear()
        {
            foreach (var item in poolingRoot.GetComponentsInChildren<Transform>(true))
            {
                if (item.Equals(poolingRoot))
                    continue;

                Destroy(item.gameObject);
            }
        }
        #endregion
    }
}