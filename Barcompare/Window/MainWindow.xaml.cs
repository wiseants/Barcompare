using HVision.Barcompare.ViewModel;
using HVision.Common.Mvvm;
using System;
using System.Windows;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private bool ViewModel_OpenModelWindowEvent(ObservableObject viewModel)
        {
            bool isDialogResult = false;
            if (viewModel is ModelWindowViewModel modelWindowViewModel)
            {
                ModelWindow modelWindow = new ModelWindow()
                {
                    Owner = this,
                    DataContext = modelWindowViewModel
                };

                isDialogResult = (modelWindow.ShowDialog() == true);
            }

            return isDialogResult;
        }

        private bool ViewModel_OpenConfigWindowEvent(ObservableObject viewModel)
        {
            bool isDialogResult = false;
            if (viewModel is ConfigWindowViewModel configWindowViewModel)
            {
                ConfigWindow configWindow = new ConfigWindow()
                {
                    Owner = this,
                    DataContext = configWindowViewModel
                };

                isDialogResult = (configWindow.ShowDialog() == true);
            }

            return isDialogResult;
        }

        private bool ViewModel_OpenLockWindowEvent(ObservableObject viewModel)
        {
            bool isDialogResult = false;
            if (viewModel is LockWindowViewModel lockWindowViewModel)
            {
                LockWindow configWindow = new LockWindow()
                {
                    Owner = this,
                    DataContext = lockWindowViewModel
                };

                isDialogResult = (configWindow.ShowDialog() == true);
            }

            return isDialogResult;
        }

        private void ModelWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            txbSerialNumberFromBarcode.Focus();
        }

        private void ModelWindow_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Override methods

        private void ModelWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MainWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);

                viewModel.OpenModelWindowEvent += ViewModel_OpenModelWindowEvent;
                viewModel.OpenConfigWindowEvent += ViewModel_OpenConfigWindowEvent;
                viewModel.OpenLockWindowEvent += ViewModel_OpenLockWindowEvent;
            }
        }

        #endregion
    }
}
