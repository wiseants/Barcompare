using System.Net.NetworkInformation;

namespace HVision.Common.Tool
{
    /// <summary>
    /// 시스템 관련 툴.
    /// </summary>
    public class SystemTool
    {
        /// <summary>
        /// MAC 주소 반환.
        /// https://www.codeproject.com/Questions/371096/get-maq-address-in-message-box-using-csharp
        /// </summary>
        /// <returns></returns>
        public static PhysicalAddress MacAddress
        {
            get
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        return nic.GetPhysicalAddress();
                    }
                }

                return null;
            }
        }
    }
}
