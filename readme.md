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

You can build the standalone screensharing app with Visual Studio 2017 or later.

Before using you must provide a Cobrowse.io license key to the app. To do this replace `License` constant in [MainViewModel.Consts.cs](https://github.com/cobrowseio/cobrowse-sdk-windows-examples/blob/master/Cobrowse.IO.Standalone/Cobrowse.IO.Standalone/ViewModel/MainViewModel.Consts.cs).

You may also provide some metadata to identify the device to `CustomData`.

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
