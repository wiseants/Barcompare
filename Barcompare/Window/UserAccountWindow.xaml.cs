using HVision.Barcompare.ViewModel;
using System.Windows;

namespace HVision.Barcompare.Window
{
    /// <summary>
    /// Interaction logic for UserAccountWindow.xaml
    /// </summary>
    public partial class UserAccountWindow : MetroBaseWindow
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public UserAccountWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Override methods

        private void AccountWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is UserAccountWindowViewModel viewModel)
            {
                DelegateBinding(viewModel);
            }
        }

        #endregion
    }
}
