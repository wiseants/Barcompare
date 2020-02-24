using HVision.Barcompare.Model;
using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.License;
using HVision.Common.Mvvm;
using HVision.Common.Tool;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 설정 윈도우 뷰모델. (ConfigWindow)
    /// </summary>
    public class ConfigWindowViewModel : ObservableObject, IMvvmViewModel
    {
        #region IMvvmViewModel members

        public event MessageEventHandler OpenMessageBoxEvent;
        public event CloseEventHandler OnCloseEvent;

        #endregion

        #region Fields

        public event DialogEventHandler OpenUserAccountWindowEvent;

        private int _requestCode = 0;
        private string _responseCode = String.Empty;
        private bool _isValidLicense = false;
        private Random _random = new Random(DateTime.Now.Millisecond);
        private Configuration _configuration = new Configuration();
        private UserAccount _selectedUserAccount = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public ConfigWindowViewModel()
        {
            OkCommand = new RelayCommand((object parameter) => OnOk(parameter));
            AddCommand = new RelayCommand((object parameter) => OnAdd(parameter));
            EditCommand = new RelayCommand(
                (object parameter) => OnEdit(parameter), 
                (object parameter) => SelectedUserAccount != null);
            DeleteCommand = new RelayCommand(
                (object parameter) => OnDelete(parameter),
                (object parameter) => SelectedUserAccount != null);
            LicenseUpdateCommand = new RelayCommand(
                (object parameter) => OnUpdateLicense(parameter),
                (object parameter) => String.IsNullOrEmpty(ResponseCode) == false && ResponseCode.Length == 2);

            UpdateRequestCode();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 체크버튼 클릭 커맨드.
        /// </summary>
        public ICommand OkCommand { get; private set; }

        /// <summary>
        /// 추가 버튼 클릭 커맨드.
        /// </summary>
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// 편집 버튼 클릭 커맨드.
        /// </summary>
        public ICommand EditCommand { get; private set; }

        /// <summary>
        /// 삭제 버튼 클릭 커맨드.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        public ICommand LicenseUpdateCommand { get; private set; }

        /// <summary>
        /// 설정 모델.
        /// </summary>
        public Configuration Configuration
        {
            get => _configuration;
            set
            {
                _configuration = value;
                OnPropertyChanged("Configuration");
            }
        }

        /// <summary>
        /// 선택된 사용자계정.
        /// </summary>
        public UserAccount SelectedUserAccount
        {
            get => _selectedUserAccount;
            set
            {
                _selectedUserAccount = value;
                OnPropertyChanged("SelectedUserAccount");
            }
        }

        /// <summary>
        /// 라이센스 갱신 요청 번호.
        /// </summary>
        public int RequestCode
        {
            get => _requestCode;
            set
            {
                _requestCode = value;
                OnPropertyChanged("RequestCode");
            }
        }

        /// <summary>
        /// 라이센스 갱신 응답 번호.
        /// </summary>
        public string ResponseCode
        {
            get => _responseCode;
            set
            {
                _responseCode = value;
                OnPropertyChanged("ResponseCode");
            }
        }

        /// <summary>
        /// 라이센스 유효성.
        /// </summary>
        public bool IsValidLicense
        {
            get => _isValidLicense;
            set
            {
                _isValidLicense = value;
                OnPropertyChanged("IsValidLicense");
            }
        }

        #endregion

        #region private methods

        private void OnOk(object parameter)
        {
            OnCloseEvent?.Invoke(this, parameter as bool?);
        }

        private void OnAdd(object parameter)
        {
            UserAccount newAccount = new UserAccount();
            UserAccountWindowViewModel viewModel = new UserAccountWindowViewModel()
            {
                IsNewAccount = true,
                UserAccount = newAccount,
                Keys = Configuration.UserAccountList.Select(x => x.Id)
            };

            if (OpenUserAccountWindowEvent?.Invoke(viewModel) == true)
            {
                Configuration.UserAccountList.Add(newAccount);
            }
        }

        private void OnEdit(object parameter)
        {
            UserAccount newAccount = SelectedUserAccount.Clone() as UserAccount;
            UserAccountWindowViewModel viewModel = new UserAccountWindowViewModel()
            {
                IsNewAccount = false,
                UserAccount = newAccount,
                Keys = Configuration.UserAccountList.Select(x => x.Id)
            };

            if (OpenUserAccountWindowEvent?.Invoke(viewModel) == true)
            {
                Configuration.UserAccountList.Remove(newAccount);
                Configuration.UserAccountList.Add(newAccount);
            }
        }

        private void OnDelete(object parameter)
        {
            if (parameter is IList<object> objectList)
            {
                if (OpenMessageBoxEvent?.Invoke("선택된 계정을 제거 하시겠습니까?", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }

                while (objectList.Count > 0)
                {
                    if (objectList[0] is UserAccount userAccount)
                    {
                        Configuration.UserAccountList.Remove(userAccount);
                    }
                }
            }
        }

        private void OnUpdateLicense(object parameter)
        {
            if (Int32.TryParse(ResponseCode, out int convertedResponseCode) == true)
            {
                if (LicenseCodeMap.CompareCode(RequestCode, convertedResponseCode) == true)
                {
                    IsValidLicense = UpdateLicense();
                }
            }

            if (IsValidLicense == false)
            {
                OpenMessageBoxEvent?.Invoke("라이센스 갱신을 실패했습니다.", MessageBoxButton.OK);
            }

            ResponseCode = String.Empty;
            UpdateRequestCode();
        }

        private void UpdateRequestCode()
        {
            RequestCode = _random.Next(99);
        }

        private bool UpdateLicense()
        {
            bool isUpdated = false;

            PhysicalAddress macAddress = SystemTool.MacAddress;
            if (macAddress == null)
            {
                return isUpdated;
            }

            string keyValue = CipherTool.Encrypt(macAddress.ToString(), MacLicense.SEED);
            if (String.IsNullOrEmpty(keyValue) == true)
            {
                return isUpdated;
            }

            using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey(MacLicense.KEY_NAME))
            {
                if (subKey.GetValue(MacLicense.VALUE_NAME) != null)
                {
                    subKey.DeleteValue(MacLicense.VALUE_NAME);
                }

                subKey.SetValue(MacLicense.VALUE_NAME, keyValue, RegistryValueKind.String);
                isUpdated = true;
            }

            return isUpdated;
        }

        #endregion
    }
}
