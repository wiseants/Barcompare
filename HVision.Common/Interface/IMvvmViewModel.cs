namespace HVision.Common.Interface
{
    /// <summary>
    /// MVVM 윈도우 인터페이스.
    /// </summary>
    public interface IMvvmViewModel
    {
        /// <summary>
        /// 윈도우 메세지박스 열기 이벤트.
        /// </summary>
        event MessageEventHandler OpenMessageBoxEvent;

        /// <summary>
        /// 윈도우 닫기 이벤트.
        /// </summary>
        event CloseEventHandler OnCloseEvent;
    }
}
