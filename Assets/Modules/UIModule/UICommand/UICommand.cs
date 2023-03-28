namespace JSGCode.UI
{
    using JSGCode.Util;
    using System;
    using System.Collections.Generic;

    public class UICommand : Command
    {
        #region Constructor & Dispose
        public UICommand()
        {
            dataDic = new Dictionary<string, object>();
            callbackDic = new Dictionary<string, Action>();
        }

        ~UICommand() => Dispose();
        #endregion

        #region Property
        public string title { get; protected set; }
        public Dictionary<string, object> dataDic { get; protected set; }
        public Dictionary<string, Action> callbackDic { get; protected set; }
        public Action<object> resultCallback { get; protected set; }
        #endregion

        #region Method : Command
        public override CommandType excutableType => CommandType.UI;
        #endregion

        #region Methods
        public void SetTitle(string title) { this.title = title; }
        public void SetData(string key, object value) { dataDic[key] = value; }
        public void SetCallback(string key, Action callback) { callbackDic[key] = callback; }
        public void SetResultCallback(Action<object> resultCallback) { this.resultCallback = resultCallback; }
        public bool ContainsData() => dataDic == null ? false : true;
        #endregion
    }
}