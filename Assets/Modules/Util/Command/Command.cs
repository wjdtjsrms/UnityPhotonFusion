namespace JSGCode.Util
{
    public class Command : ICommand
    {
        #region Method : Command
        public virtual CommandType excutableType { get; }

        public virtual void Request()
        {
            CommandController.Instance.SendCommand(this);
        }

        public virtual void Finish()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
        }
        #endregion
    }
}