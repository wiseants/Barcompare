using HVision.Barcompare.Core;
using HVision.Barcompare.Model;
using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.License;
using HVision.Common.Mvvm;
using HVision.Common.Tool;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 메인 윈도우 뷰모델. (MainWindow)
    /// </summary>
    public class MainWindowViewModel : ObservableObject, IMvvmViewModel, IDisposable
    {
        #region Fields

        /// <summary>
        /// 설정 파일 상대 경로.
        /// </summary>
        public static readonly string CONFIG_FILE_FULLPATH = FolderTool.WorkingFolderFullPath + @"\..\configuration\config.xml";
        /// <summary>
        /// 모델 설정 파일 상대 경로.
        /// </summary>
        public static readonly string MODEL_CONFIG_FILE_FULLPATH = FolderTool.WorkingFolderFullPath + @"\..\configuration\model_config.xml";
        public static readonly string RESULT_FOLDER_PATH = FolderTool.WorkingFolderFullPath + @"\..\result";

        public event DialogEventHandler OpenModelWindowEvent;
        public event DialogEventHandler OpenConfigWindowEvent;
        public event DialogEventHandler OpenLockWindowEvent;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private string _serialNumberFromBarcode = String.Empty;
        private bool _isChecked = false;
        private bool _isEnable = true;
        private ILicense _license = new MacLicense();
        private Configuration _configuration = new Configuration();
        private ModelConfiguration _modelConfiguration = new ModelConfiguration();
        private ComparisonResult _currentResult = ComparisonResult.None;
        private ComparisonSoundPlayer _player = new ComparisonSoundPlayer()
        {
            WavFiles = new List<ComparisonResult>()
            {
                ComparisonResult.None,
                ComparisonResult.OK,
                ComparisonResult.NG
            }
        };

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public MainWindowViewModel()
        {
            ClosedCommand = new RelayCommand((object parameter) => OnClosed(parameter));
            OnModelWindowCommand = new RelayCommand((object parameter) => OnModelWindow(parameter));
            OnConfigWindowCommand = new RelayCommand((object parameter) => OnConfigWindow(parameter));
            OnKeyDownCommand = new RelayCommand((object parameter) => OnKeyDown(parameter));
            OnCheckCommand = new RelayCommand(
                (object parameter) => OnCheck(parameter), 
                (parameter) => { return ModelConfiguration.SelectedModel != null; });

            LoadConfiguration();

            //using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey(MacLicense.KEY_NAME))
            //{
            //    if (subKey.GetValue(MacLicense.VALUE_NAME) != null)
            //    {
            //        subKey.DeleteValue(MacLicense.VALUE_NAME);
            //    }
            //}

            Enable = _license.IsValidLicense;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 클로즈드 커맨드.
        /// </summary>
        public ICommand ClosedCommand { get; private set; }

        /// <summary>
        /// 모델 윈도우 열기 커맨드.
        /// </summary>
        public ICommand OnModelWindowCommand { get; private set; }

        /// <summary>
        /// 설정 윈도우 열기 커맨드.
        /// </summary>
        public ICommand OnConfigWindowCommand { get; private set; }

        /// <summary>
        /// 키보드 버튼 누루기 커맨드.
        /// </summary>
        public ICommand OnKeyDownCommand { get; private set; }

        /// <summary>
        /// 체크 버튼 클릭 커맨드.
        /// </summary>
        public ICommand OnCheckCommand { get; private set; }

        /// <summary>
        /// 사용 가능 여부.
        /// </summary>
        public bool Enable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                OnPropertyChanged("Enable");
            }
        }

        /// <summary>
        /// 설정.
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
        /// 모델 설정.
        /// </summary>
        public ModelConfiguration ModelConfiguration
        {
            get => _modelConfiguration;
            set
            {
                _modelConfiguration = value;
                OnPropertyChanged("ModelConfiguration");
            }
        }

        /// <summary>
        /// 바코드 리더기로 읽은 일련번호.
        /// </summary>
        public string SerialNumberFromBarcode
        {
            get => _serialNumberFromBarcode;
            set
            {
                _serialNumberFromBarcode = value;
                OnPropertyChanged("SerialNumberFromBarcode");
            }
        }

        /// <summary>
        /// 현재의 결과.
        /// </summary>
        public ComparisonResult CurrentResult
        {
            get => _currentResult;
            set
            {
                _currentResult = value;
                _player.Configuration = Configuration;
                _player.Play(_currentResult);
                OnPropertyChanged("CurrentResult");
            }
        }

        /// <summary>
        /// 체크 발생 여부.
        /// True : 체크 했다. False : 일련번호 입력중이다.
        /// </summary>
        private bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                {
                    return;
                }

                _isChecked = value;
                if (value == false)
                {
                    SerialNumberFromBarcode = String.Empty;
                    CurrentResult = ComparisonResult.None;
                }
            }
        }

        #endregion

        #region Private methods

        private void LoadConfiguration()
        {
            if (File.Exists(CONFIG_FILE_FULLPATH) == false)
            {
                Configuration newConfiguration = new Configuration()
                {
                    UserAccountList = new ObservableCollection<UserAccount>()
                };

                XmlTool<Configuration>.Export(CONFIG_FILE_FULLPATH, newConfiguration);

                _logger.Info("설정 파일이 존재하지 않아서 설정 파일을 생성합니다.");
            }

            Configuration = XmlTool<Configuration>.Import(CONFIG_FILE_FULLPATH);
            if (Configuration.UserAccountList.Count <= 0)
            {
                Configuration.UserAccountList.Add(new UserAccount() { Id = "admin", Password = "0000", Name = "관리자", Description = "기본 관리자 계정." });
                _logger.Info("계정이 존재하지 않아서 기본 계정을 추가합니다.");
            }

            if (File.Exists(MODEL_CONFIG_FILE_FULLPATH) == false)
            {
                XmlTool<ModelConfiguration>.Export(MODEL_CONFIG_FILE_FULLPATH, new ModelConfiguration());
            }

            ModelConfiguration = XmlTool<ModelConfiguration>.Import(MODEL_CONFIG_FILE_FULLPATH);
        }

        private void Check()
        {
            if (Enable == false)
            {
                return;
            }

            if (ModelConfiguration.SelectedModel == null)
            {
                OnModelWindow(null);
                return;
            }

            // 로드된 모델과 읽은 일련번호를 내보내기.
            ExportResult(new string[] { ModelConfiguration.SelectedModel.Name, SerialNumberFromBarcode });

            if (TryCheck(out string errorMessage) == false)
            {
                string unlockCode = Configuration.UnlockCode;
                //if (String.IsNullOrEmpty(unlockCode) == false)
                //{
                //    LockWindowViewModel viewModel = new LockWindowViewModel()
                //    {
                //        UnlockCode = unlockCode,
                //        ErrorMessage = errorMessage
                //    };
                //    OpenLockWindowEvent?.Invoke(viewModel);
                //    _player.Stop();
                //}
            }

            IsChecked = true;
        }

        private bool TryCheck(out string errorMessage)
        {
            errorMessage = "알 수 없는 알람.";

            int plusMarkIndex = SerialNumberFromBarcode.IndexOf('+');
            if (plusMarkIndex == -1 || SerialNumberFromBarcode.Length != 25)
            {
                errorMessage = "형식이 잘못되었습니다.";
                return false;
            }

            string modelNameFromBarcode = SerialNumberFromBarcode.Substring(0, plusMarkIndex);
            if (String.IsNullOrEmpty(modelNameFromBarcode) == true)
            {
                errorMessage = "모델 이름이 없습니다.";
                return false;
            }

            string serialFromBarcode = SerialNumberFromBarcode.Substring(plusMarkIndex + 1, SerialNumberFromBarcode.Length - plusMarkIndex - 1);
            if (String.IsNullOrEmpty(serialFromBarcode) == true)
            {
                errorMessage = "일련번호가 없습니다.";
                return false;
            }

            CurrentResult = modelNameFromBarcode == ModelConfiguration.SelectedModel.Name ? ComparisonResult.OK : ComparisonResult.NG;
            if (CurrentResult == ComparisonResult.NG)
            {
                errorMessage = "모델이 일치하지 않습니다.";
                return false;
            }

            return true;
        }

        private void ExportResult(string[] columnArray)
        {
            if (Directory.Exists(RESULT_FOLDER_PATH) == false)
            {
                Directory.CreateDirectory(RESULT_FOLDER_PATH);
            }

            string filePath = String.Format(@"{0}\{1}.csv", RESULT_FOLDER_PATH, DateTime.Now.ToString("yyyy_MM_dd"));
			try
			{
				using (StreamWriter writer = new StreamWriter(filePath, true))
				{
					writer.WriteLine(String.Join(",", columnArray));
				}
			}
			catch (Exception ex)
			{
				OpenMessageBoxEvent?.Invoke("결과 쓰기 실패.", MessageBoxButton.OK);
			}
		}

        #endregion

        #region Event handlers

        private void OnCheck(object parameter)
        {
            Check();
        }

        private void OnClosed(object parameter)
        {
            XmlTool<Configuration>.Export(CONFIG_FILE_FULLPATH, Configuration);
            XmlTool<ModelConfiguration>.Export(MODEL_CONFIG_FILE_FULLPATH, ModelConfiguration);
        }

        private void OnKeyDown(object parameter)
        {
            IsChecked = false;

            if (parameter is KeyEventArgs keyArgs)
            {
                if (keyArgs.Key == Key.Enter)
                {
                    Check();
                }
            }
        }

        private void OnModelWindow(object parameter)
        {
            ModelWindowViewModel viewModel = new ModelWindowViewModel()
            {
                SelectedModel = ModelConfiguration.SelectedModel,
                SerialNumberModelList = ModelConfiguration.SerialNumberModelList
            };

            if (OpenModelWindowEvent?.Invoke(viewModel) == true)
            {
                ModelConfiguration.SelectedModel = viewModel.SelectedModel;
            }

            if (ModelConfiguration.SerialNumberModelList.IndexOf(ModelConfiguration.SelectedModel) == -1)
            {
                ModelConfiguration.SelectedModel = null;
            }

            XmlTool<ModelConfiguration>.Export(MODEL_CONFIG_FILE_FULLPATH, ModelConfiguration);
        }

        private void OnConfigWindow(object parameter)
        {
            ConfigWindowViewModel viewModel = new ConfigWindowViewModel()
            {
                Configuration = Configuration,
                IsValidLicense = Enable
            };

            OpenConfigWindowEvent?.Invoke(viewModel);
            Enable = viewModel.IsValidLicense;

            XmlTool<Configuration>.Export(CONFIG_FILE_FULLPATH, Configuration);
        }

        #endregion

        #region IMvvmViewModel members

        public event MessageEventHandler OpenMessageBoxEvent;
        public event CloseEventHandler OnCloseEvent;

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            _player.Dispose();
        }

        #endregion
    }
}
