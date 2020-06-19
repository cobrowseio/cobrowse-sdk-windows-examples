using System.Collections.Generic;

namespace Cobrowse.IO.Screenshare.ViewModel
{
  partial class MainViewModel
  {
    // Set license key here
    public const string License =
       "TODO";

    // Set the device metadata here
    public static readonly Dictionary<string, object> CustomData = new Dictionary<string, object>()
    {
      { CobrowseIO.DeviceNameKey, "TODO" },
      { CobrowseIO.DeviceIdKey, "TODO" },
    };
  }
}
