using HVision.Barcompare.Model;
using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.Mvvm;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 로그인 윈도우 뷰모델. (LoginWindow)
    /// </summary>
    public class LoginWindowViewModel : ObservableObject, IMvvmViewModel
    {
        #region Events

        public event Action OnResetEvent;

        #endregion

        #region Fields

        private string _id = String.Empty;
        private Dictionary<string, UserAccount> _userAccountMap = new Dictionary<string, UserAccount>();
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public LoginWindowViewModel()
        {
            LoginCommand = new RelayCommand((object parameter) => OnLogin(parameter));
            CloseCommand = new RelayCommand((object parameter) => OnClose(parameter));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 로그인 커맨드.
        /// </summary>
        public ICommand LoginCommand { get; private set; }

        /// <summary>
        /// 닫기 커맨드.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// 입력 계정.
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// 사용자 계정 맵
        /// </summary>
        public Dictionary<string,UserAccount> UserAccountMap
        {
            get => _userAccountMap;
            set
            {
                _userAccountMap = value;
                OnPropertyChanged("UserAccountMap");
            }
        }

        #endregion

        #region Private methods

        private void OnLogin(object parameter)
        {
            bool isSuccess = false;

            try
            {
                if (parameter is PasswordBox passwordBox)
                {
                    if (UserAccountMap.TryGetValue(Id, out UserAccount value) == true)
                    {
                        isSuccess = value.Password == passwordBox.Password;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Error("예외가 발생했습니다. 메세지:{0}", ex.ToString());
            }

            if (isSuccess == true)
            {
                OnCloseEvent?.Invoke(this, true);
            }
            else
            {
                OpenMessageBoxEvent?.Invoke("잘못된 정보를 입력하셨습니다.", MessageBoxButton.OK);
                OnResetEvent?.Invoke();
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
