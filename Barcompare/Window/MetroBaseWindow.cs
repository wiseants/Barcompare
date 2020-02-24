using HVision.Common.Extension;
using HVision.Common.Interface;
using MahApps.Metro.Controls;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// 메트로 윈도우 기본 클래스.
    /// </summary>
    public class MetroBaseWindow : MetroWindow, IMvvmView
    {
        #region IMvvmView

        public void DelegateBinding(IMvvmViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            // 윈도우 메세지박스 출력 이벤트 핸들러 바인딩.
            viewModel.OpenMessageBoxEvent += (message, type) => 
            {
                return MessageBoxEx.Show(this, message, "알림", type);
            };

            // 윈도우 닫기 이벤트 핸들러 바인딩.
            viewModel.OnCloseEvent += (sender, result) =>
            {
                DialogResult = result;
                Close();
            };
        }

        #endregion
    }
}
