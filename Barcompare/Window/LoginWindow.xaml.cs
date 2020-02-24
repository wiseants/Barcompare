using HVision.Barcompare.ViewModel;
using System;
using System.Windows;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void ViewModel_OnResetEvent()
        {
            txbId.Text = String.Empty;
            pwdPassword.Password = String.Empty;
            txbId.Focus();
        }

        #endregion

        #region Override methods

        private void LoginWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is LoginWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);

                viewModel.OnResetEvent += ViewModel_OnResetEvent;
            }
        }

        #endregion
    }
}
