using HVision.Barcompare.Model;
using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 모델 추가 윈도우 뷰모델. (AddModelWindow)
    /// </summary>
    public class AddModelWindowViewModel : ObservableObject, IMvvmViewModel
    {
        #region IMvvmViewModel members

        public event MessageEventHandler OpenMessageBoxEvent;
        public event CloseEventHandler OnCloseEvent;

        #endregion

        #region Fields

        public event Action OnResetEvent;

        private string _modelName = String.Empty;
        private string _serialNumber = String.Empty;
        private List<SerialNumberModel> _serialNumberModelList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public AddModelWindowViewModel()
        {
            AddCommand = new RelayCommand((object parameter) => OnAdd(parameter));
            CloseCommand = new RelayCommand((object parameter) => OnClose(parameter));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 추가 커맨드.
        /// </summary>
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// 닫기 커맨드.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// 모델 이름.
        /// </summary>
        public string ModelName
        {
            get => _modelName;
            set
            {
                _modelName = value;
                OnPropertyChanged("ModelName");
            }
        }

        /// <summary>
        /// 모델 일련번호.
        /// </summary>
        public string ModelSerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged("SerialNuber");
            }
        }

        /// <summary>
        /// 모델 리스트.
        /// </summary>
        public List<SerialNumberModel> SerialNumberModelList
        {
            get => _serialNumberModelList;
            set
            {
                _serialNumberModelList = value;
                OnPropertyChanged("SerialNumberModelList");
            }
        }

        #endregion

        #region Private methods

        private void OnAdd(object parameter)
        {
            if (String.IsNullOrEmpty(ModelName) == true)
            {
                OpenMessageBoxEvent?.Invoke("이름을 입력하십시요.", MessageBoxButton.OK);
                return;
            }

            SerialNumberModel newModel = new SerialNumberModel()
            {
                Name = ModelName,
                SerialNumber = ModelSerialNumber
            };

            if (SerialNumberModelList.Contains(newModel) == true)
            {
                OpenMessageBoxEvent?.Invoke("동일한 모델 이름이 이미 존재합니다.", MessageBoxButton.OK);
                return;
            }

            SerialNumberModelList.Add(newModel);
            OnCloseEvent?.Invoke(this, true);
        }

        private void OnClose(object parameter)
        {
            OnCloseEvent?.Invoke(this, false);
        }

        #endregion
    }
}
