using System;

namespace WTLib.Mvvm
{
    public class RelayCommand : CommandBase
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public bool CanExecute() { return CanExecute(null); }

        public override void OnExecute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }
        public void Execute() { OnExecute(null); }
    }

    public class RelayCommand<T> : CommandBase

    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public bool CanExecute(T parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public override void OnExecute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            _execute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        public void Execute() { OnExecute(null); }

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter)) return;
            _execute(parameter);
        }
    }
}
