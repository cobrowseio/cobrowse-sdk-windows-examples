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

      if (MainViewModel.License == "TODO")
      {
        MessageBox.Show(
            "No license key provided.\n\nPlease set up a license key to License const in \\ViewModel\\MainViewModel.Consts.cs",
            "Screensharing",
            MessageBoxButton.OK,
            MessageBoxImage.Stop
          );

        Shutdown();
        return;
      }

      MainViewModel vm = new MainViewModel();
      MainWindow = vm.Window;
      vm.Window.Show();
    }
  }
}
