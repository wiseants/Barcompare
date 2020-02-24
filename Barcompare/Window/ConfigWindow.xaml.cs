using HVision.Barcompare.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public ConfigWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Override methods

        private void ConfigWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ConfigWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);

                viewModel.OpenUserAccountWindowEvent += ViewModel_OpenUserAccountWindowEvent;
            }
        }

        private bool ViewModel_OpenUserAccountWindowEvent(Common.Mvvm.ObservableObject viewModel)
        {
            bool isDialogResult = false;
            if (viewModel is UserAccountWindowViewModel userAccountWindowViewModel)
            {
                UserAccountWindow userAccountWindow = new UserAccountWindow
                {
                    Owner = this,
                    DataContext = userAccountWindowViewModel
                };

                isDialogResult = (userAccountWindow.ShowDialog() == true);
            }

            return isDialogResult;
        }

        #endregion

        #region Private methods

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        #endregion

        #region Event handlers

        private void TxbResponseCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsTextAllowed(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        #endregion
    }
}
