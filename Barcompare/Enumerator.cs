using System.ComponentModel;

namespace HVision.Barcompare
{
    /// <summary>
    /// 바코드 비교 결과.
    /// </summary>
    public enum Comparison
    {
        /// <summary>
        /// 비교 결과 없음.
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// 바코드 매치.
        /// </summary>
        [Description("OK")]
        OK,

        /// <summary>
        /// 바코드 매치 되지 않음.
        /// </summary>
        [Description("NG")]
        NG
    }
}
