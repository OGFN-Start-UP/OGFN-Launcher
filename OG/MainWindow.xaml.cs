using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using Wpf.Ui;
using System.Runtime.CompilerServices;
using og.Utils;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
using System.IO;
using RestSharp;
using System.Net;
using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace og
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Home.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Visible;
            Navigation.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Hidden;

            Accent.ApplySystemAccent();
            Wpf.Ui.Appearance.Background.Apply(this, Wpf.Ui.Appearance.BackgroundType.Mica);

            Loaded += (_, _) => InvokeSplashScreen();
        }

        private bool _initialized = false;
        private BackgroundWorker worker;

        private async void InvokeSplashScreen()
        {
            if (_initialized)
                return;

            _initialized = true;

            await Config.Load();

            if (Config.Configuration.Password != "")
            {
                Config.Configuration.Password = "";
                await Config.Save();
            }

            if (Config.Configuration.Email != "unused")
            {
                EmailTxt.Text = Config.Configuration.Email;
                PasswordTxt.Text = Config.Configuration.Password;
            }

            /*if (Config.Configuration.CustomLaunchArguments != "null")
            {
                CustomArgs.Text = Config.Configuration.CustomLaunchArguments;
            }*/

            Globals.MainWindowStatic = this;
        }

        private void Go_Home_Click(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Visible;
            Settings.Visibility= Visibility.Hidden;
        }
        private void GoSettingsClick(object sender, RoutedEventArgs e)
        {
            Settings.Visibility = Visibility.Visible;
            Home.Visibility = Visibility.Hidden;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            loadingLabel.Text = "Checking for updates...";
            Loading.Visibility = Visibility.Visible;
            Login.Visibility = Visibility.Hidden;

            // New rest client to check for updates
            var restclientLatestVersion = new RestClient();

            // request to API
            var GetLatestVersion = new RestRequest("http://localhost:3001/v1/version", Method.Get).AddHeader("authorization", Constants.ApiAuth);

            // get the latest version
            var latestVersion = await restclientLatestVersion.ExecuteAsync(GetLatestVersion);

            // Check versions
            if (latestVersion.Content != Constants.Version)
            {
                snakeBarNot.Show("Update Required!", "Please update to the latest version. Please check the discord!", Wpf.Ui.Common.SymbolRegular.ShieldError24, Wpf.Ui.Common.ControlAppearance.Danger);
                loadingLabel.Text = "Update Required. Please check the Discord!";
                return;
            }

            loadingLabel.Text = "Logging in...";

            // Login
            Globals.AccountStorage = await FnBackend.Login(EmailTxt.Text, PasswordTxt.Password);

            if (Globals.AccountStorage.ErrorMessage != null)
            {
                Login.Visibility= Visibility.Visible;
                Loading.Visibility= Visibility.Hidden;
                snakeBarNot.Show("Login Error", Globals.AccountStorage.ErrorMessage, Wpf.Ui.Common.SymbolRegular.ShieldError24, Wpf.Ui.Common.ControlAppearance.Danger);
                return; //stop
            }

            loadingLabel.Text = "Contacting Services...";

            // Url to get users current skin
            var getcid = $"http://localhost:3001/v1/GetSkin?accountId={Globals.AccountStorage.AccountId}";
            
            // New rest client
            var restclient = new RestClient();

            // request to API
            var getcidreq = new RestRequest(getcid, Method.Get) .AddHeader("authorization", Constants.ApiAuth);

            // get response
            var cid = await restclient.ExecuteAsync(getcidreq);
            Logger.Log(cid.Content, LogLevel.Debug);

            // Get the current skin image
            string CidIcon = $"https://fortnite-api.com/images/cosmetics/br/{cid.Content}/icon.png";
            Logger.Log(CidIcon, LogLevel.Debug);
            // Apply the current skin image
            CidIco.ImageSource = new BitmapImage(new Uri(CidIcon));

            /*

            // Item Shop

            Globals.itemShopStorage = await FnBackend.GetShop();

            // set labels

            FeaturedLabel.Content = "<--- Featured";
            DailyLabel.Content = "Daily --->";

            // Set images

            ImageFeatured1.Source = new BitmapImage(new Uri(Globals.itemShopStorage.featured1.ImageUrl));
            ImageFeatured2.Source = new BitmapImage(new Uri(Globals.itemShopStorage.featured2.ImageUrl));
            ImageDaily1.Source = new BitmapImage(new Uri(Globals.itemShopStorage.daily1.ImageUrl));
            ImageDaily2.Source = new BitmapImage(new Uri(Globals.itemShopStorage.daily2.ImageUrl));
            ImageDaily3.Source = new BitmapImage(new Uri(Globals.itemShopStorage.daily3.ImageUrl));
            ImageDaily4.Source = new BitmapImage(new Uri(Globals.itemShopStorage.daily4.ImageUrl));
            ImageDaily5.Source = new BitmapImage(new Uri(Globals.itemShopStorage.daily5.ImageUrl));
            ImageDaily6.Source = new BitmapImage(new Uri(Globals.itemShopStorage.daily6.ImageUrl));

            // add vbucks to price

            string Featured1Price = Globals.itemShopStorage.featured1.Price + " Vbucks";
            string Featured2Price = Globals.itemShopStorage.featured2.Price + " Vbucks";
            string daily1price = Globals.itemShopStorage.daily1.Price + " Vbucks";
            string daily2price = Globals.itemShopStorage.daily2.Price + " Vbucks";
            string daily3price = Globals.itemShopStorage.daily3.Price + " Vbucks";
            string daily4price = Globals.itemShopStorage.daily4.Price + " Vbucks";
            string daily5price = Globals.itemShopStorage.daily5.Price + " Vbucks";
            string daily6price = Globals.itemShopStorage.daily6.Price + " Vbucks";

            // Set prices

            PriceFeatured1.Content = Featured1Price;
            PriceFeatured2.Content = Featured2Price;
            PriceDaily1.Content = daily1price;
            PriceDaily2.Content = daily2price;
            PriceDaily3.Content = daily3price;
            PriceDaily4.Content = daily4price;
            PriceDaily5.Content = daily5price;
            PriceDaily6.Content = daily6price;

            */

            // Load config
            if (Config.Configuration.CustomLaunchArguments != null)
            {
                CustomArgs.Text = Config.Configuration.CustomLaunchArguments;
            }

            if (Config.Configuration.CurrentBuild != null)
            {
                FortniteGame_CH1_Path.Text = Config.Configuration.CurrentBuild;
            }

            // Set the welcome message
            WelcomeMessage.Content = $"Welcome Back to og, {Globals.AccountStorage.DisplayName}!";

            // ogRPC.StartLogin();

            // Save config. Do not save password for security.
            Config.Configuration.Email = EmailTxt.Text;
            //Config.Configuration.Password = PasswordTxt.Password;
            await Config.Save();

            // hide loading screen and show the user interface
            Loading.Visibility = Visibility.Hidden;
            Navigation.Visibility = Visibility.Visible;
            Home.Visibility = Visibility.Visible;
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            PasswordTxt.Password = "";
            Settings.Visibility = Visibility.Hidden;
            Navigation.Visibility = Visibility.Hidden;
            Home.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Visible;
            // ogRPC.Logout();
        }

        private async void CustomArgsSave(object sender, RoutedEventArgs e)
        {
            if (CustomArgs.Text != Config.Configuration.CustomLaunchArguments)
            {
                Config.Configuration.CustomLaunchArguments = CustomArgs.Text;
                await Config.Save();
                snakeBarNot.Show("Saved!", "You're custom launch arguments have been saved!", Wpf.Ui.Common.SymbolRegular.Save24, Wpf.Ui.Common.ControlAppearance.Success);
            }
            else
            {
                snakeBarNot.Show("Saved!", "You're custom launch arguments have been saved!", Wpf.Ui.Common.SymbolRegular.Save24, Wpf.Ui.Common.ControlAppearance.Success);
                return;
            }
            
        }

        private async void FortniteGame_CH1_Path_Save(object sender, RoutedEventArgs e)
        {
            if (Config.Configuration.CurrentBuild != FortniteGame_CH1_Path.Text)
            {
                Config.Configuration.CurrentBuild = FortniteGame_CH1_Path.Text;
                await Config.Save();
                snakeBarNot.Show("Saved!", "You're build path has been saved!", Wpf.Ui.Common.SymbolRegular.Save24, Wpf.Ui.Common.ControlAppearance.Success);

            }
            else
            {
                snakeBarNot.Show("Saved!", "You're build path has been saved!", Wpf.Ui.Common.SymbolRegular.Save24, Wpf.Ui.Common.ControlAppearance.Success);
            }
        }

        private void FortniteGameFileExplorer(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                var selectedPath = dialog.SelectedPath;
                if (!Directory.Exists(selectedPath + "\\FortniteGame"))
                {
                    snakeBarNot.Show("Path Error", "Please select a folder witch contains", Wpf.Ui.Common.SymbolRegular.ShieldError24, Wpf.Ui.Common.ControlAppearance.Danger);
                    return;
                }

                FortniteGame_CH1_Path.Text = selectedPath;
            }
        }

        private async void LaunchFortniteGame(object sender, RoutedEventArgs e)
        {
            if (Config.Configuration.CurrentBuild == null)
            {
                snakeBarNot.Show("Error!", "Please select a gamepath in settings.", Wpf.Ui.Common.SymbolRegular.ShieldError24, Wpf.Ui.Common.ControlAppearance.Danger);
                return;
            }

            Globals.exchange = await FnBackend.GetExchange();
            worker = new BackgroundWorker();
            worker.DoWork += LaunchFortnite;
            worker.RunWorkerAsync();
        }

        private async void LaunchFortnite(object sender, DoWorkEventArgs e)
        {
            try
            {
                /*_ = Dispatcher.Invoke(async () =>
                {
                    RootMainGrid.Visibility = Visibility.Collapsed;
                    RootLoadingGrid.Visibility = Visibility.Visible;
                });*/

                if (!System.IO.File.Exists(Globals.FortniteLaucher()))
                {
                    /*_ = Dispatcher.Invoke(async () =>
                    {
                        loadingLabel.Text = "Downloading an update...";
                    });*/

                    Logger.Log($"Downloading an update...");
                    await DownloadUtils.DownloadFakeLauncher();
                }


                /*if (FileUtils.GetUnrealEngineVersion() is "4.23.0")
                {
                    _ = Dispatcher.Invoke(async () =>
                    {
                        loadingLabel.Text = "Downloading Custom Paks...";
                    });

                    await DownloadUtils.DownloadPaks();
                }*/

                /*_ = Dispatcher.Invoke(async () =>
                {
                    loadingLabel.Text = "Launching Fortnite...";
                });*/

                await Launcher.LaunchFortniteGame();

                _ = Dispatcher.Invoke(async () =>
                {
                    var fTokenData = await FnBackend.Login(EmailTxt.Text, PasswordTxt.Password);

                    /*// start of get skin icon
                    if (fTokenData.ErrorMessage is null)
                    {
                        var cid = await FortniteUtils.GetCharacter(fTokenData.AccountId, fTokenData.AccessToken);

                        var characterData = await FortniteUtils.GetIcon(cid);

                        SkinIcon.ImageSource = new BitmapImage(new Uri(characterData.Image));

                        var icon = characterData.Series is "" ?
                        $"api/files/{characterData.Rarity}.png" ;
                        $"http://ogfn.duckdns.org:3555/api/files/{characterData.Series}.png";

                        RarityIcon.ImageSource = new BitmapImage(new Uri(icon));
                    }
                    // end of get skin icon*/
                });

                /*_ = Dispatcher.Invoke(async () =>
                {
                    RootMainGrid.Visibility = Visibility.Visible;
                    RootLoadingGrid.Visibility = Visibility.Collapsed;
                });*/
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), LogLevel.Error);
                FileUtils.OpenLogError(ex, "Fortnite Laucher");
                return;
            }
        }
    }
}