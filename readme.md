# Cobrowse SDK Windows Examples

## Startup

To run the example app, you will need to feed it with your Cobrowse.io license key with first command line argument.

### Standalone

```Text
Cobrowse.IO.WpfApp.exe <put-license-here>
```

### Visual Studio

Put your license key (alone) to *Command line arguments* of *Debug* section of project settings.

## Known Problems

### HiDPI

Due to some strange behavior of MS Win10 in HiDPI mode (when display scale is not 100%) you may see black screen in screensharing session. This occurs at first start
of the client app from certain location and will be autofixed after restart from same directory.

To avoid this your application should be "DPI aware" and should tell the system about it. To enable this add `app.manifest` ("New item" -> "Application Manifest File" in Visual Studio) and add/uncomment the following XML tag:

```XML
  <application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    </windowsSettings>
  </application>
```