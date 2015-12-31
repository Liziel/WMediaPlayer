using System;
using System.Windows.Input;

namespace WPFUiLibrary.Utils
{
    public class UiCommand : ICommand
    {
        #region Fields

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Constructor

        /// <exception cref="ArgumentNullException"><paramref name=""/> is <see langword="null" />.</exception>
        public UiCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        public event EventHandler canExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null ? true : this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (!this.CanExecute(parameter))
                throw new InvalidOperationException("The command is not valid for execution, check the CanExecute method before attempting to execute.");
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}