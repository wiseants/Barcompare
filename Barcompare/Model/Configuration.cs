using HVision.Common.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 설정 정보 모델.
    /// </summary>
    [Serializable]
    [XmlRoot("Configuration")]
    public class Configuration : ObservableObject, ICloneable
    {
        #region Fields

        private int _maxLoopCount = 1;
        private string _unlockCode = String.Empty;
        private ObservableCollection<UserAccount> _userAccontList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자. 초기화 실행.
        /// </summary>
        public Configuration()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 알람 반복 횟수.
        /// </summary>
        [XmlElement("MaxLoopCount")]
        public int MaxLoopCount
        {
            get => _maxLoopCount;
            set
            {
                _maxLoopCount = value;
                OnPropertyChanged("MaxLoopCount");
            }
        }

        /// <summary>
        /// 알람 해제 코드.
        /// </summary>
        [XmlElement("UnlockCode")]
        public string UnlockCode
        {
            get => _unlockCode;
            set
            {
                _unlockCode = value;
                OnPropertyChanged("UnlockCode");
            }
        }

        /// <summary>
        /// 사용자 계정 리스트.
        /// </summary>
        [XmlArray("UserAccountList")]
        public ObservableCollection<UserAccount> UserAccountList
        {
            get => _userAccontList;
            set
            {
                _userAccontList = value;
                OnPropertyChanged("UserAccountList");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 초기화.
        /// </summary>
        public void Reset()
        {
        }

        #endregion

        #region IClonable members

        public object Clone()
        {
            Configuration newConfiguration = this.MemberwiseClone() as Configuration;
            {
                UserAccountList = new ObservableCollection<UserAccount>(UserAccountList.Select(x => x.Clone() as UserAccount));
            };

            return newConfiguration;
        }

        #endregion
    }
}
