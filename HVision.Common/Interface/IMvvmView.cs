namespace HVision.Common.Interface
{
    /// <summary>
    /// MVVM 뷰모델 인터페이스.
    /// </summary>
    public interface IMvvmView
    {
        /// <summary>
        /// 뷰모델 이벤트 바인딩.
        /// </summary>
        /// <param name="viewModel"></param>
        void DelegateBinding(IMvvmViewModel viewModel);
    }
}
