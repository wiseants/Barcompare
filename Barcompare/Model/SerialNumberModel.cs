using HVision.Common.Mvvm;
using System;
using System.Xml.Serialization;

namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 일련번호 모델.
    /// </summary>
    [XmlRoot("SerialNumberModel")]
    public class SerialNumberModel : ObservableObject, ICloneable
    {
        #region Fields

        private string _name;
        private string _serialNumber;
        private DateTime _updatedDate;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자. 갱신 시간 업데이트.
        /// </summary>
        public SerialNumberModel()
        {
            UpdatedDate = DateTime.Now;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 이름.
        /// </summary>
        [XmlElement("Name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 일련번호.
        /// </summary>
        [XmlElement("SerialNumber")]
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }

        /// <summary>
        /// 갱신 시간.
        /// </summary>
        [XmlElement("UpdatedDate")]
        public DateTime UpdatedDate
        {
            get => _updatedDate;
            set
            {
                _updatedDate = value;
                OnPropertyChanged("UpdatedDate");
            }
        }

        #endregion

        #region ICloneable members

        public object Clone()
        {
            return new SerialNumberModel()
            {
                Name = Name,
                SerialNumber = SerialNumber
            };
        }

        #endregion

        #region Override methods

        public override bool Equals(object obj)
        {
            SerialNumberModel model = obj as SerialNumberModel;
            if (model == null)
            {
                return false;
            }

            return Name == model.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion
    }
}
