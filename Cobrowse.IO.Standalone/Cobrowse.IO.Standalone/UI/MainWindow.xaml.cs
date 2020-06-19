using System.ComponentModel;
using System.Windows;

using Cobrowse.IO.Standalone.ViewModel;

namespace Cobrowse.IO.Standalone.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      await ((MainViewModel)DataContext).Initialize();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      MainViewModel mvm = (MainViewModel)DataContext;

      e.Cancel = !mvm.CanClose;

      mvm.Close();
    }
  }
}
