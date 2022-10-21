// .versionrc.js
const tracker = [
  {
    filename: 'Cobrowse.IO.Standalone/Cobrowse.IO.Standalone/Properties/AssemblyInfo.cs',
    updater: {
      readVersion: function(contents) {
        return contents.match(/\[assembly: AssemblyVersion\("([0-9]+\.[0-9]+\.[0-9]+)/)[1]
      },
      writeVersion: function (contents, version) {
        return contents.replace(/(\[assembly: AssemblyVersion\(")([0-9\.]+)(\"\)\])/, `$1${version}.0$3`)
      }
    }
  },
  {
    filename: 'Cobrowse.IO.Standalone/Cobrowse.IO.Standalone/Properties/AssemblyInfo.cs',
    updater: {
      readVersion: function(contents) {
        return contents.match(/\[assembly: AssemblyFileVersion\("([0-9]+\.[0-9]+\.[0-9]+)/)[1]
      },
      writeVersion: function (contents, version) {
        return contents.replace(/(\[assembly: AssemblyFileVersion\(")([0-9\.]+)(\"\)\])/, `$1${version}.0$3`)
      }
    }
  },
  {
    filename: 'Cobrowse.IO.WpfApp/Cobrowse.IO.WpfApp/Properties/AssemblyInfo.cs',
    updater: {
      readVersion: function(contents) {
        return contents.match(/\[assembly: AssemblyVersion\("([0-9]+\.[0-9]+\.[0-9]+)/)[1]
      },
      writeVersion: function (contents, version) {
        return contents.replace(/(\[assembly: AssemblyVersion\(")([0-9\.]+)(\"\)\])/, `$1${version}.0$3`)
      }
    }
  },
  {
    filename: 'Cobrowse.IO.WpfApp/Cobrowse.IO.WpfApp/Properties/AssemblyInfo.cs',
    updater: {
      readVersion: function(contents) {
        return contents.match(/\[assembly: AssemblyFileVersion\("([0-9]+\.[0-9]+\.[0-9]+)/)[1]
      },
      writeVersion: function (contents, version) {
        return contents.replace(/(\[assembly: AssemblyFileVersion\(")([0-9\.]+)(\"\)\])/, `$1${version}.0$3`)
      }
    }
  },
]

module.exports = {
  packageFiles: tracker,
  bumpFiles: tracker
}