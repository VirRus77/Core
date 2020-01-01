using System;
using System.Windows.Input;

namespace Core.Wpf.Tools
{
    /// <summary>
    /// Класс описания и выполнения команды
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _handler;
        readonly Predicate<object> _canExecute;
        private event EventHandler _canExecuteChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор с без параметризованными методами выполнения команды
        /// </summary>
        /// <param name="handler">Метод вызываемый при выполнении команды</param>
        /// <param name="canExecute">Метод возвращающий флаг возможности вызвать команду</param>
        public RelayCommand(Action handler, Func<bool> canExecute = null)
            : this((obj) => handler(), canExecute == null ? (Predicate<object>)null : (obj) => canExecute())
        {
        }

        /// <summary>
        /// Конструктор с параметризованными методами выполнения команды
        /// </summary>
        /// <param name="handler">Метод вызываемый при выполнении команды</param>
        /// <param name="canExecute">Метод возвращающий флаг возможности вызвать команду</param>
        public RelayCommand(Action<object> handler, Predicate<object> canExecute = null)
        {
            _handler = handler;
            _canExecute = canExecute;
        }

        #endregion

        #region MembersOfInterfaces

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _handler(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (_canExecute != null)
                OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
