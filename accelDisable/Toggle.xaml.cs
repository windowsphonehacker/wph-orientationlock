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
using CSharp___DllImport;
namespace accelDisable
{
    public partial class Toggle : PhoneApplicationPage
    {
        public Toggle()
        {
            var last = getLastSetting();
            if (last)
            {
                DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", "ACC1:", 1, (int)Phone.Network.WiFi.PowerState.D4);
            }
            else
            {
                DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", "ACC1:", 1, (int)Phone.Network.WiFi.PowerState.D0);
            }

            Tile.setLastSetting(!last);
            
            Tile.updateTile(last);

            throw new Exception();

        }

        bool getLastSetting()
        {
            bool last = false;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("last", out last);
            return last;
        }

    }
}