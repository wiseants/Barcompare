using HVision.Common.Mvvm;
using HVision.Common.Tool;
using System;
using System.Xml.Serialization;

namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 사용자 계정 모델.
    /// </summary>
    [Serializable]
    [XmlRoot("UserAccount")]
    public class UserAccount : ObservableObject, ICloneable
    {
        #region Fields

        public static readonly string SEED = "hcan";

        private string _id = String.Empty;
        private string _name = String.Empty;
        private string _description = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public UserAccount()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 계정.
        /// </summary>
        [XmlElement("Id")]
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
        /// 비밀번호.
        /// </summary>
        [XmlIgnore]
        public string Password
        {
            get
            {
                if (String.IsNullOrEmpty(CryptoPassword) == true)
                {
                    return String.Empty;
                }

                return CipherTool.Decrypt(CryptoPassword, SEED);
            }
            set
            {

                CryptoPassword = CipherTool.Encrypt(value, SEED);
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// 암호화 비밀번호.
        /// </summary>
        [XmlElement("CryptoPassword")]
        public string CryptoPassword
        {
            get;
            set;
        }

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
        /// 설명.
        /// </summary>
        [XmlElement("Description")]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        #endregion

        #region Public methods

        public bool IsValid()
        {
            return String.IsNullOrEmpty(Id) == false && String.IsNullOrEmpty(Name) == false;
        }

        #endregion

        #region IConable members

        public object Clone()
        {
            return new UserAccount()
            {
                Id = Id,
                CryptoPassword = CryptoPassword,
                Name = Name,
                Description = Description
            };
        }

        #endregion

        #region Override methods

        public override bool Equals(object obj)
        {
            if (obj is UserAccount userAccount)
            {
                return Id == userAccount.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
