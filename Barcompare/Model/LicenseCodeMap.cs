namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 라이센스 갱신을 위한 코드 맵핑 클래스.
    /// </summary>
    public class LicenseCodeMap
    {
        #region Fileds

        private readonly static int[] _codeArray = new int[]
        {
        //  X0, X1, X2, X3, X4, X5, X6, X7, X8, X9
            17, 83, 43, 75, 67, 52, 67, 98, 08, 41, // 0X
            77, 94, 38, 18, 23, 23, 05, 68, 50, 27, // 1X
            22, 48, 81, 07, 14, 68, 37, 21, 45, 35, // 2X
            57, 92, 47, 12, 28, 51, 47, 96, 38, 47, // 3X
            22, 47, 48, 27, 59, 17, 91, 45, 06, 67, // 4X
            64, 39, 36, 63, 03, 41, 65, 12, 69, 90, // 5X
            28, 81, 38, 23, 78, 94, 24, 46, 42, 37, // 6X
            54, 37, 84, 38, 51, 65, 94, 34, 16, 63, // 7X
            73, 47, 65, 53, 66, 42, 88, 11, 38, 64, // 8X
            39, 37, 93, 83, 35, 07, 22, 07, 22, 98  // 9X
        };

        #endregion

        #region Public methods

        /// <summary>
        /// 요청 코드와 응답 코드를 비교하여 같은 경우 True, 아닌 경우 False
        /// </summary>
        /// <param name="request">요청 코드.</param>
        /// <param name="response">응답 코드.</param>
        /// <returns></returns>
        public static bool CompareCode(int request, int response)
        {
            if (request >= _codeArray.Length || response >= _codeArray.Length || request < 0 || response < 0)
            {
                return false;
            }

            return _codeArray[request] == response;
        }

        #endregion
    }
}
