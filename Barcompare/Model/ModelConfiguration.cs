using HVision.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 일련번호 모델 저장용 설정 모델.
    /// </summary>
    [Serializable]
    [XmlRoot("ModelConfiguration")]
    public class ModelConfiguration : ObservableObject, ICloneable
    {
        #region Fields

        private SerialNumberModel _selectedModel = null;
        private List<SerialNumberModel> _serialNumberModelList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자. 초기화 실행.
        /// </summary>
        public ModelConfiguration()
        {
            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 일련번호 모델 리스트.
        /// </summary>
        [XmlArray("SerialNumberModelList")]
        public List<SerialNumberModel> SerialNumberModelList
        {
            get => _serialNumberModelList;
            set
            {
                _serialNumberModelList = value;
                OnPropertyChanged("SerialNumberModelList");
            }
        }

        /// <summary>
        /// 선택된 모델.
        /// </summary>
        [XmlElement("SelectedModel")]
        public SerialNumberModel SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                OnPropertyChanged("SelectedModel");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 초기화.
        /// </summary>
        public void Reset()
        {
            SelectedModel = null;
            SerialNumberModelList = new List<SerialNumberModel>();
        }

        #endregion

        #region IClonable members

        public object Clone()
        {
            ModelConfiguration newConfiguration = new ModelConfiguration()
            {
                SelectedModel = SelectedModel.Clone() as SerialNumberModel
            };

            foreach (SerialNumberModel model in SerialNumberModelList)
            {
                newConfiguration.SerialNumberModelList.Add(model.Clone() as SerialNumberModel);
            }

            return newConfiguration;
        }

        #endregion
    }
}
