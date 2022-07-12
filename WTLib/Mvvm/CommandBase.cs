using System;
using System.Windows.Input;

namespace WTLib.Mvvm
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter);

        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter) == false)
                return;
            OnExecute(parameter);
        }

        public abstract void OnExecute(object parameter);

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
