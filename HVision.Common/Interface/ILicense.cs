namespace HVision.Common.Interface
{
    /// <summary>
    /// 라이센스 사용 인터페이스.
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// 라이센스 사용 가능 여부.
        /// </summary>
        bool IsValidLicense { get; }
    }
}
