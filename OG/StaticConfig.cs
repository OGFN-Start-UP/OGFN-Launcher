using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using og.Utils;

namespace og
{
    public static class Globals
    {
        public static MainWindow MainWindowStatic { get; set; }

        public static AccountStorage AccountStorage { get; set; }

        public static ItemShopStorage itemShopStorage { get; set; }

        public static Athena Athena { get; set; }

        public static exchange exchange { get; set; }

        public static string FortniteLaucher() => Path.Combine(Constants.BasePath, $"FortniteLauncher.exe");
        //currentbuild.path to Config.Configuration.CurrentBuild
        public static string FortniteShipping() => Path.Combine(Config.Configuration.CurrentBuild, $"FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");
        public static string FortniteShippingEAC() => Path.Combine(Config.Configuration.CurrentBuild, $"FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_Eac.exe");

        //public static string FortniteArgs() => $"-epicapp=Fortnite -epicenv=Prod -EpicPortal -noeac -nobe -fltoken=fdd9g715h4i20110dd40d7d3 -AUTH_TYPE=epic " +
        //    $"-AUTH_LOGIN={Config.Configuration.Email} -AUTH_PASSWORD={Config.Configuration.Password} {Config.Configuration.CustomLaunchArguments}";
        public static string FortniteArgs() => $"-epicapp=Fortnite -epicenv=Prod -EpicPortal -noeac -nobe -fltoken=fdd9g715h4i20110dd40d7d3 -AUTH_TYPE=exchangecode -AUTH_LOGIN=unused " +
            $"-AUTH_PASSWORD={exchange.Code} {Config.Configuration.CustomLaunchArguments}";
    }

    public static class Constants
    {
        public static readonly string BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OG-Launcher-Config\\");

        public static readonly string BaseLogs = Path.Combine(BasePath, "Advanced");

        public static readonly string LogPath = Path.Combine(BaseLogs, "Logs");

        public static readonly string LogFile = Path.Combine(LogPath, "OG-Launcher.log");
        public static readonly string ConfigFile = Path.Combine(BasePath, "config.OGLauncher");

        public static readonly string RunTime = Path.Combine(BasePath, "OG-Runtime.dll");

        public static readonly string ApiAuth = "Bearer bdy32rw3dbui3odnoi3ndidfbewifbvowebi3h34uigweo8u";

        public static readonly string Version = "1.0.0";
    }
}
