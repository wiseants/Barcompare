using HVision.Barcompare.Model;
using HVision.Common;
using HVision.Common.Interface;
using HVision.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HVision.Barcompare.ViewModel
{
    /// <summary>
    /// 모델 윈도우 뷰모델. (ModelWindow)
    /// </summary>
    public class ModelWindowViewModel : ObservableObject, IMvvmViewModel
    {
        #region IMvvmViewModel members

        public event MessageEventHandler OpenMessageBoxEvent;
        public event CloseEventHandler OnCloseEvent;

        #endregion

        #region Fields

        public event DialogEventHandler OpenAddWindowEvent;
        public event DialogEventHandler OpenEditWindowEvent;

        private string _filterString = String.Empty;
        private SerialNumberModel _selectedModel = null;
        private List<SerialNumberModel> _serialNumberModelList = null;

        private SerialNumberModel _selectedModel_backup = null;
        private List<SerialNumberModel> _serialNumberModelList_backup = new List<SerialNumberModel>();

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public ModelWindowViewModel()
        {
            LoadedCommand = new RelayCommand((object parameter) => OnLoaded(parameter));
            ResetCommand = new RelayCommand((object parameter) => OnReset(parameter));
            AddCommand = new RelayCommand((object parameter) => OnAdd(parameter));
            DeleteCommand = new RelayCommand((object parameter) => OnDelete(parameter), (object parameter) => SelectedModel != null);
            CloseCommand = new RelayCommand((object parameter) => OnClose(parameter));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 로디드 커맨드.
        /// </summary>
        public ICommand LoadedCommand { get; private set; }

        /// <summary>
        /// 초기화 버튼 클릭 커맨드.
        /// </summary>
        public ICommand ResetCommand { get; private set; }

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

        /// <summary>
        /// 닫기 버튼 클릭 커맨드.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// 검색 문자열.
        /// </summary>
        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;
                OnPropertyChanged("FilterString");
                OnPropertyChanged("FilteredModelList");
            }
        }

        /// <summary>
        /// 선택된 모델.
        /// </summary>
        public SerialNumberModel SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                OnPropertyChanged("SelectedModel");
            }
        }

        /// <summary>
        /// 실제로 보여지는 필터된 리스트.
        /// </summary>
        public ObservableCollection<SerialNumberModel> FilteredModelList
        {
            get
            {
                StringComparison conparison = StringComparison.CurrentCultureIgnoreCase;
                var filteredModelList = 
                    new ObservableCollection<SerialNumberModel>(
                        _serialNumberModelList.Where(x => x.Name.IndexOf(FilterString, conparison) >= 0 || 
                        x.SerialNumber.IndexOf(FilterString, conparison) >= 0));

                if (filteredModelList.Count > 0)
                {
                    SelectedModel = filteredModelList.First();
                }

                return filteredModelList;
            }
        }

        /// <summary>
        /// 소스 모델 리스트.
        /// </summary>
        public List<SerialNumberModel> SerialNumberModelList
        {
            get => _serialNumberModelList;
            set
            {
                _serialNumberModelList = value;
                OnPropertyChanged("FilteredModelList");
            }
        }

        #endregion

        #region Event handlers

        private void OnLoaded(object parameter)
        {
            if (SelectedModel != null)
            {
                _selectedModel_backup = SelectedModel.Clone() as SerialNumberModel;
            }

            if (SerialNumberModelList != null)
            {
                _serialNumberModelList_backup = new List<SerialNumberModel>(SerialNumberModelList.Select(x => x.Clone() as SerialNumberModel));
            }
        }

        private void OnReset(object parameter)
        {
            if (_selectedModel_backup != null)
            {
                SelectedModel = _selectedModel_backup.Clone() as SerialNumberModel;
            }

            SerialNumberModelList.Clear();
            foreach (SerialNumberModel model in _serialNumberModelList_backup)
            {
                SerialNumberModelList.Add(model.Clone() as SerialNumberModel);
            }

            OnPropertyChanged("FilteredModelList");
        }

        private void OnAdd(object parameter)
        {
            AddModelWindowViewModel viewModel = new AddModelWindowViewModel()
            {
                SerialNumberModelList = SerialNumberModelList
            };

            OpenAddWindowEvent?.Invoke(viewModel);

            OnPropertyChanged("FilteredModelList");
        }

        private void OnDelete(object parameter)
        {
            if (parameter is IList<object> objectList)
            {
                if (OpenMessageBoxEvent?.Invoke("선택된 모델을 제거 하시겠습니까?", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }

                SerialNumberModelList.RemoveAll(x => objectList.Contains(x) == true);
            }

            OnPropertyChanged("FilteredModelList");
        }

        private void OnClose(object parameter)
        {
            OnCloseEvent?.Invoke(this, parameter as bool?);
        }

        #endregion
    }
}
