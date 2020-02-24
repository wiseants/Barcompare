using HVision.Barcompare.ViewModel;
using System;
using System.Windows;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for AddModelWindow.xaml
    /// </summary>
    public partial class AddModelWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public AddModelWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void ViewModel_OnReset()
        {
            txbName.Text = String.Empty;
            txbName.Focus();
        }

        private void AddModelWindow_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Override methods

        private void MetroWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AddModelWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);

                viewModel.OnResetEvent += ViewModel_OnReset;
            }
        }

        #endregion
    }
}
