using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using CSharp___DllImport;
using System.Windows.Threading;
namespace accelDisable
{
    public class Tile
    {
        public static void updateTile(bool status)
        {
            var tile = Microsoft.Phone.Shell.ShellTile.ActiveTiles.Where(x => x.NavigationUri.ToString().Contains("Toggle")).FirstOrDefault();
            if (tile != null)
            {
                Microsoft.Phone.Shell.StandardTileData data = new Microsoft.Phone.Shell.StandardTileData
                {
                    Title = "Orientation Lock",
                };
                if (status)
                {
                    data.BackgroundImage = new Uri("locked.png", UriKind.Relative);
                }
                else
                {
                    data.BackgroundImage = new Uri("unlocked.png", UriKind.Relative);
                }
                System.Diagnostics.Debug.WriteLine(data.BackgroundImage.ToString());
                tile.Update(data);
            }
        }
        public static void setLastSetting(bool last)
        {
            try
            {
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("last");
            }
            catch
            {
            }
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("last", last);
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
        }
        
        
    }
}
