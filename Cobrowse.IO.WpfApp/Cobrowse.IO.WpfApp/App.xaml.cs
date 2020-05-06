using System.Windows;

using Cobrowse.IO.WpfApp.ViewModel;

namespace Cobrowse.IO.WpfApp
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      if (e.Args.Length == 0)
      {
        MessageBox.Show("Please provide license code as the first command line argument", "Can't Start", MessageBoxButton.OK, MessageBoxImage.Error);
        Shutdown();
        return;
      }

      MainViewModel vm = new MainViewModel(e.Args[0]);
      MainWindow = vm.Window;
      vm.Window.Show();
    }
  }
}
