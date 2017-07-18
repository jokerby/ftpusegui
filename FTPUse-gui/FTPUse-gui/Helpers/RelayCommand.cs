using System;
using System.Windows.Input;

namespace FTPUse_gui.Helpers
{
    internal sealed class ButtonCommandBinding : ICommand
    {
        private readonly Action _handler;
        public ButtonCommandBinding(Action handler)
        {
            _handler = handler;
        }

        public bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _handler();
        }
    }
}