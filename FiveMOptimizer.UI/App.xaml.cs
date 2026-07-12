using System.Windows;
using FiveMOptimizer.Helpers;

namespace FiveMOptimizer.UI;

public partial class App : Application
{
    public App()
    {
        if (!AdminHelper.IsRunningAsAdmin())
        {
            MessageBox.Show("Este aplicativo requer privilégios de administrador!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            AdminHelper.RequestAdminPrivileges();
            Shutdown();
        }
    }
}
