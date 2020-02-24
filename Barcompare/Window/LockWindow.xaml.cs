using HVision.Barcompare.ViewModel;
using System.Windows;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for LockWindow.xaml
    /// </summary>
    public partial class LockWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public LockWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Override methods

        private void LockWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is LockWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);

                viewModel.OnResetEvent += ViewModel_OnResetEvent;
            }
        }

        #endregion

        #region Event handlers

        private void ViewModel_OnResetEvent()
        {
            pwdPassword.Password = string.Empty;
        }

        private void LockWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult != true)
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}
