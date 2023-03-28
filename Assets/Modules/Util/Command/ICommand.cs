namespace JSGCode.Util
{
    using System;

    public interface ICommand : IDisposable
    {
        public CommandType excutableType { get; }

        public void Request();
    }

    public interface ICommandExcutor
    {
        public CommandType availableType { get; }

        public void Excute(ICommand command);
    }
}