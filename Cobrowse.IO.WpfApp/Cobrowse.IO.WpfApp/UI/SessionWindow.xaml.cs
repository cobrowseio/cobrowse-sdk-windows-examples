using System.Windows;

using Cobrowse.IO.WpfApp.ViewModel;

namespace Cobrowse.IO.WpfApp.UI
{
  /// <summary>
  /// Interaction logic for SessionWindow.xaml
  /// </summary>
  public partial class SessionWindow : Window
  {
    public SessionWindow()
    {
      InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      await ((SessionViewModel)DataContext).Initialize();
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {
      ((SessionViewModel)DataContext).Close();
    }
  }
}
