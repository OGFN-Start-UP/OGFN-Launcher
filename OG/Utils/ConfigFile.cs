using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public class Config
    {
        public static Root Configuration { get; set; }

        public async static Task Load()
        {
            try
            {
                if (!File.Exists(Constants.ConfigFile))
                {
                    Logger.Log($"Couldnt find Config File, creating a new one", LogLevel.Warning);

                    await Task.Delay(1000);

                    Configuration = new Root();

                    await Save();
                }
                else
                {
                    Configuration = JsonConvert.DeserializeObject<Root>(await File.ReadAllTextAsync(Constants.ConfigFile));
                    Logger.Log($"Found Config File now reading it");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), LogLevel.Error);
                FileUtils.OpenLogError(ex, "Config");
                return;
            }
        }

        public async static Task Save()
        {
            Logger.Log($"Saved Config File!");
            await File.WriteAllTextAsync(Constants.ConfigFile, JsonConvert.SerializeObject(Configuration));
        }

        public static async Task<string> AddBuild(Builds build)
        {
            var check = Configuration.Builds.FirstOrDefault(x => x.Name == build.Name);

            if (check != null)
                return $"A build with that name is already taken";

            Configuration.Builds.Add(build);
            await Save();

            Logger.Log($"Added the build \"{build.Name}\" to list");

            return $"The build {build.Name} was added!";
        }

        public static async Task DeleteBuildByName(string name)
        {
            var build = Configuration.Builds.FirstOrDefault(x => x.Name == name);

            if (build == null)
                return;

            Configuration.Builds.Remove(build);

            Logger.Log($"Removed the build \"{build.Name}\" from list");
            await Save();
        }
    }

    public class Root
    {
        public string Email { get; set; } = "unused";
        public string Password { get; set; }
        public string CurrentBuild { get; set; }
        public List<Builds> Builds { get; set; } = new List<Builds>();
        public string CustomLaunchArguments { get; set; }
    }

    public class Builds
    {
        public string Name { get; set; }
        public string Path { set; get; }
    }
}
