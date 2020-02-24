using HVision.Barcompare.ViewModel;
using HVision.Barcompare.Window;
using NLog;
using System;
using System.Linq;
using System.Windows;

namespace HVision.Barcompare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Event handler

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
                LoginWindowViewModel loginWindowViewModel = new LoginWindowViewModel
                {
                    UserAccountMap = mainWindowViewModel.Configuration.UserAccountList.ToDictionary(x => x.Id, x => x)
                };

                LoginWindow loginWindow = new LoginWindow()
                {
                    DataContext = loginWindowViewModel
                };

                MainWindow mainWindow = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };

                if (loginWindow.ShowDialog() == true)
                {
                    _logger.Info("{0} 계정으로 로그인 했습니다.", loginWindowViewModel.Id);
                    mainWindow.Show();
                }
                else
                {
                    mainWindow.Close();
                }
            }
            catch(Exception ex)
            {
                _logger.Error("예외가 발생했습니다. 메세지:{0}", ex.ToString());
            }
        }

        #endregion
    }
}
