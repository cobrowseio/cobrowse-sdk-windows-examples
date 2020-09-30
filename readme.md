# Cobrowse.io SDK Windows Examples

This repo contains examples of Cobrowse.io usage.

## WPF Example

This is just an example of using Cobrowse.io Windows SDK with WPF.

To run the example app, you will need to feed it with your Cobrowse.io license key with first command line argument.

### Standalone

```Text
Cobrowse.IO.WpfApp.exe <put-license-here>
```

### Visual Studio

Put your license key (alone) to *Command line arguments* of *Debug* section of project settings.

## Standalone App

Standalone app is used to start Cobrowse.io screensharing sessions in separate process. 

### Building

You can build the standalone screensharing app with Visual Studio 2017 or later. Be sure that NuGet restore is performed on build.

Before using, you must provide a license obtained from [cobrowse.io](https://cobrowse.io/) site. See "Configuration" section.

### Configuration

The application settings are defined in `cobrowse.io.settings.json` file.

* `license` - put your license code here. This is required.
* `hideSessionControls` - this boolean variable allows to enable/disable the session activity controls for active display (red outline).
* `customData` - allows to set custom information about a device to identify it.

### Usage

How to run standalone screensharing app:

```csharp
public bool StartCobrowse()
{
  try
  {
    Process.Start(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Cobrowse.IO.Standalone.exe"));
    return true;
  }
  catch
  {
    return false;
  }
}
```
