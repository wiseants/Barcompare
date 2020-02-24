using HVision.Common.Interface;
using HVision.Common.Tool;
using Microsoft.Win32;
using NLog;
using System;

namespace HVision.Common.License
{
    /// <summary>
    /// MAC 주소를 사용한 라이센스 클래스.
    /// </summary>
    public class MacLicense : ILicense
    {
        #region Fields

        /// <summary>
        /// 레지스트리 키이름.
        /// </summary>
        public static readonly string KEY_NAME = @"SOFTWARE\MacLicense";
        /// <summary>
        /// 레지스트리 값이름.
        /// </summary>
        public static readonly string VALUE_NAME = "LicenseKey";
        /// <summary>
        /// 암호/복호화 시드 문자열.
        /// </summary>
        public static readonly string SEED = "HVision";

        private static Logger _logger = LogManager.GetCurrentClassLogger();


        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public MacLicense()
        {

        }

        #endregion

        #region ILicense members

        public bool IsValidLicense
        {
            get
            {
                bool isValid = false;

                try
                {
                    string keyValue = String.Empty;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KEY_NAME))
                    {
                        if (key != null)
                        {
                            keyValue = key.GetValue(VALUE_NAME) as string;
                        }
                    }

                    if (String.IsNullOrEmpty(keyValue) == true)
                    {
                        return isValid;
                    }

                    string savedMacAddress = CipherTool.Decrypt(keyValue, SEED);
                    if (savedMacAddress != SystemTool.MacAddress.ToString())
                    {
                        return isValid;
                    }

                    isValid = true;
                }
                catch (Exception ex)
                {
                    _logger.Error("Exception occured. Message:{0}", ex.Message);
                }

                return isValid;
            }
        }

        #endregion
    }
}
