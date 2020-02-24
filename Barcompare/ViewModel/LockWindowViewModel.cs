using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.Mvvm;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 잠김 윈도우 뷰모델. (LockWindow)
    /// </summary>
    public class LockWindowViewModel : ObservableObject, IMvvmViewModel
    {
        #region Events

        public event Action OnResetEvent;

        #endregion

        #region Fields

        private string _errorMessage = "빈 메세지.";

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public LockWindowViewModel()
        {
            UnlockCommand = new RelayCommand((object parameter) => OnUnlock(parameter));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 잠김풀기 커맨드.
        /// </summary>
        public ICommand UnlockCommand { get; private set; }

        /// <summary>
        /// 잠김풀기 코드.
        /// </summary>
        public string UnlockCode
        {
            get;set;
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;

                OnPropertyChanged("ErrorMessage");
            }
        }

        #endregion

        #region Private methods

        private void OnUnlock(object parameter)
        {
            bool isSuccess = false;
            if (parameter is PasswordBox passwordBox)
            {
                isSuccess = UnlockCode == passwordBox.Password;
            }

            if (isSuccess == true)
            {
                OnCloseEvent?.Invoke(this, true);
            }
            else
            {
                OnResetEvent?.Invoke();
                OpenMessageBoxEvent?.Invoke("해제 코드가 일치하지 않습니다.", MessageBoxButton.OK);
            }
        }

        private void OnClose(object parameter)
        {
            OnCloseEvent?.Invoke(this, false);
        }

        #endregion

        #region IMvvmViewModel members

        public event MessageEventHandler OpenMessageBoxEvent;
        public event CloseEventHandler OnCloseEvent;

        #endregion
    }
}
