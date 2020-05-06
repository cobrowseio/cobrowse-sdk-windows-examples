using System.ComponentModel;
using System.Runtime.CompilerServices;

using Cobrowse.IO.WpfApp.UI;

namespace Cobrowse.IO.WpfApp.ViewModel
{
  class MainViewModel: INotifyPropertyChanged
  {
    private string deviceId = "n/a";

    public MainViewModel(string licenseKey)
    {
      CommandStart = new RelayCommand(CommandStart_Execute);

      Window = new MainWindow();
      Window.DataContext = this;

      CobrowseIO.Instance.License = CobrowseIO.Instance.License = licenseKey;

      DeviceId = CobrowseIO.Instance.Device.DeviceId;

      CobrowseIO.Instance.Device.CustomData.Add("user_name", "Test User");
      CobrowseIO.Instance.Device.CustomData.Add("user_email", "example@testmail.com");
      CobrowseIO.Instance.Device.CustomData.Add("device_name", "WPF Device");
    }

    public MainWindow Window { get; }

    public string DeviceId
    {
      get { return deviceId; }
      private set
      {
        deviceId = value;
        OnPropertyChanged();
      }
    }

    #region Commands

    public RelayCommand CommandStart { get; }

    private void CommandStart_Execute()
    {
      SessionViewModel vm = new SessionViewModel(Window);
      vm.Start();
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
