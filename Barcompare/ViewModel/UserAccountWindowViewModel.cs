using HVision.Barcompare.Model;
using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 사용자 계정 윈도우 뷰모델. (UserAccountWindow)
    /// </summary>
    public class UserAccountWindowViewModel : ObservableObject, IMvvmViewModel
    {
        #region IMvvmViewModel members

        public event MessageEventHandler OpenMessageBoxEvent;
        public event CloseEventHandler OnCloseEvent;

        #endregion

        #region Fields

        private UserAccount _userAccount = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public UserAccountWindowViewModel()
        {
            OkCommand = new RelayCommand((object parameter) => OnOk(parameter), (object parameter) => IsValid(parameter));
            CloseCommand = new RelayCommand((object parameter) => OnClose(parameter));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 확인 커맨드.
        /// </summary>
        public ICommand OkCommand { get; private set; }

        /// <summary>
        /// 닫기 커맨드.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// 새로 생성하는 사용자계정인지 여부.
        /// True : 새 사용자 계정. False : 편집 사용자 계정.
        /// </summary>
        public bool IsNewAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 현재 사용자 계정.
        /// </summary>
        public UserAccount UserAccount
        {
            get => _userAccount;
            set
            {
                _userAccount = value;
                OnPropertyChanged("UserAccount");
            }
        }

        /// <summary>
        /// 사용자 계정 중복 체크를 위한 키리스트.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get;
            set;
        }

        #endregion

        private void OnOk(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;
                if (String.IsNullOrEmpty(password) == true)
                {
                    OpenMessageBoxEvent?.Invoke("비밀번호를 입력하십시요.", MessageBoxButton.OK);
                    return;
                }

                if (IsNewAccount == true && Keys.Contains(UserAccount.Id) == true)
                {
                    OpenMessageBoxEvent?.Invoke("중복된 계정입니다.", MessageBoxButton.OK);
                    return;
                }

                UserAccount.Password = password;
                OnCloseEvent?.Invoke(this, true);
            }
        }

        private void OnClose(object parameter)
        {
            OnCloseEvent?.Invoke(this, parameter as bool?);
        }

        private bool IsValid(object paramter)
        {
            if (UserAccount == null)
            {
                return false;
            }

            return UserAccount.IsValid();
        }
    }
}
