using HVision.Barcompare.ViewModel;
using HVision.Common.Extension;
using HVision.Common.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for ModelWindow.xaml
    /// </summary>
    public partial class ModelWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public ModelWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private bool ViewModel_OpenAddWindowEvent(ObservableObject viewModel)
        {
            bool isDialogResult = false;
            if (viewModel is AddModelWindowViewModel addModelWindowViewModel)
            {
                AddModelWindow addModelWindow = new AddModelWindow
                {
                    Owner = this,
                    DataContext = addModelWindowViewModel
                };

                isDialogResult = (addModelWindow.ShowDialog() == true);
            }

            return isDialogResult;
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ModelWindow_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void GrdSerialNumberList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBoxEx.Show(this, "선택된 모델을 제거 하시겠습니까?", "경고", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Override methods

        private void ModelWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ModelWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);

                viewModel.OpenAddWindowEvent += ViewModel_OpenAddWindowEvent;
            }
        }

        #endregion
    }
}
