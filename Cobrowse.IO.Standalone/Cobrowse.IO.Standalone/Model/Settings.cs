using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;

namespace Cobrowse.IO.Standalone.Model
{
  class Settings
  {
    #region Singleton

    private Settings() { }

    private static Settings instance;

    public static Settings Instance
    {
      get
      {
        if (instance == null)
        {
          string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "cobrowse.io.settings.json");
          instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
        }
        return instance;
      }
    }

    #endregion

    public string License { get; set; }

    public IReadOnlyDictionary<string, object> CustomData { get; } = new Dictionary<string, object>();

    public string ApiUrl { get; set; }

    public bool HideSessionControls { get; set; }
  }
}
