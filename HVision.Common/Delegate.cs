using HVision.Common.Mvvm;
using System.Windows;

namespace HVision.Common
{
    /// <summary>
    /// 윈도우 닫기 이벤트 핸들러.
    /// </summary>
    /// <param name="sender">발송자.</param>
    /// <param name="result">윈도우 닫기 결과.</param>
    public delegate void CloseEventHandler(object sender, bool? result);

    /// <summary>
    /// 메세지박스 출력 이벤트 핸들러.
    /// </summary>
    /// <param name="message">메세지.</param>
    /// <param name="type">메세지박스 타입.</param>
    /// <returns>메세지박스 결과.</returns>
    public delegate MessageBoxResult MessageEventHandler(string message, MessageBoxButton type);

    /// <summary>
    /// 다이얼로그 출력 이벤트 핸들러.
    /// </summary>
    /// <param name="viewModel">뷰모델.</param>
    /// <returns>다이얼로그 결과.</returns>
    public delegate bool DialogEventHandler(ObservableObject viewModel);
}
