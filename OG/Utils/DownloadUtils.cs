using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public class DownloadUtils
    {
        private static int counter;
        private static string totalfix;

        internal class Endpoints
        {
            public static readonly Uri Base = new Uri("http://localhost:3001/");

            public static readonly Uri Native = new Uri(Base, "v1/download/redirect");

            public static readonly Uri LauncherFake = new Uri(Base, "v1/download/FortniteLauncher");
            public static readonly Uri Shipping = new Uri(Base, "v1/download/FortniteClient-Win64-Shipping");
        }


        public static async Task DownloadNative()
            => await File.WriteAllBytesAsync(Constants.RunTime, await new HttpClient().GetByteArrayAsync(Endpoints.Native));

        public static async Task DownloadFakeLauncher()
            => await File.WriteAllBytesAsync(Globals.FortniteLaucher(), await new HttpClient().GetByteArrayAsync(Endpoints.LauncherFake));

        public static async Task DownloadPaks()
        {
            try
            {
                Logger.Log("Getting info for Custom Pak");

                var webclient = new WebClient();

                webclient.Proxy = null;
                webclient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webclient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                while (webclient.IsBusy)
                    await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), LogLevel.Error);
                FileUtils.OpenLogError(ex, "Download error");
            }
        }

        private static void ProgressChanged(object obj, DownloadProgressChangedEventArgs e)
        {
            var Logger = $"Downloading Custom Pak\n{e.ProgressPercentage}% of 100%, {((e.BytesReceived / 1024f) / 1024f).ToString("#0.##")}MB of {((e.TotalBytesToReceive / 1024f) / 1024f).ToString("#0.##")}MB.";

            totalfix = $"{((e.TotalBytesToReceive / 1024f) / 1024f).ToString("#0.##")}MB.";

            counter++;

            Utils.Logger.Log(Logger, LogLevel.Debug);

            if (counter % 65 == 0)
                Globals.MainWindowStatic.Dispatcher.Invoke(async () =>
                {
                    //Globals.MainWindowStatic.loadingLabel.Text = Logger;
                });
        }

        private static void Completed(object obj, AsyncCompletedEventArgs e)
        {
            Globals.MainWindowStatic.Dispatcher.Invoke(async () =>
            {
                //Globals.MainWindowStatic.loadingLabel.Text = $"Downloading Custom Pak\n100% of 100%, {totalfix}MB of {totalfix}MB.";
            });
        }
    }
}
