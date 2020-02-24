// https://www.codeproject.com/Tips/813345/Basic-MVVM-and-ICommand-Usage-Example

using System;
using System.Windows.Input;

namespace HVision.Common.Mvvm
{
    /// <summary>
    /// 커맨드 인터페이스 객체.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        private event EventHandler CanExecuteChangedInternal;
        private Action<object> execute;
        private Predicate<object> canExecute;

        #endregion

        #region Constuctors

        /// <summary>
        /// 커맨드 액션을 전달하는 생성자.
        /// </summary>
        /// <param name="execute">커맨드 액션.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// 커맨드 액션과 유효성 액션을 전달하는 생성자.
        /// </summary>
        /// <param name="execute">커맨드 액션.</param>
        /// <param name="canExecute">유효성 액선.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("Not exist execute-action.");
            this.canExecute = canExecute ?? throw new ArgumentNullException("Not exist canExecute-action.");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 유효성 변경 이벤트 핸들러 실행.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 액션 해제.
        /// </summary>
        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        #endregion

        #region Private methods

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region ICommnad members

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion
    }
}
