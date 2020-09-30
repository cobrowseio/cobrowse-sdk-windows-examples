using System.Windows;

using Cobrowse.IO.Standalone.ViewModel;

namespace Cobrowse.IO.Standalone
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      MainViewModel vm = new MainViewModel();
      MainWindow = vm.Window;
      vm.Window.Show();
    }
  }
}
