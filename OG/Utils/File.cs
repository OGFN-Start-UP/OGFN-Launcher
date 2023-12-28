using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public static class FileUtils
    {
        public static void CheckDirectory(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static string GetUnrealEngineVersion()
        {
            var info = FileVersionInfo.GetVersionInfo(Globals.FortniteShipping());
            return $"{info.FileMajorPart}.{info.FileMinorPart}.{info.FileBuildPart}";
        }

        public static void OpenLogError(Exception ex, string errorTitle)
        {
            _ = Globals.MainWindowStatic.Dispatcher.Invoke(async () =>
            {
                var errorMsg = $"Please send your Log File to a dev or in the help channel in the discord\nError - {ex.Message}";
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {
                    Title = $"Error - {errorTitle}",
                    Content = errorMsg
                };

                messageBox.ButtonLeftName = "Open Log";
                messageBox.ButtonRightName = "Close";
                messageBox.ButtonLeftClick += (s, e) => Process.Start("explorer.exe", Constants.LogFile);
                messageBox.ButtonRightClick += (s, e) => messageBox.Close();

                messageBox.Show();
            });

        }
    }
}
