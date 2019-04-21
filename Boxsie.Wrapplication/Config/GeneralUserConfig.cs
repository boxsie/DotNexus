using System.IO;
using Boxsie.Wrapplication.Config.Contracts;

namespace Boxsie.Wrapplication.Config
{
    public partial class GeneralUserConfig : IUserConfig
    {
        public string UserDataPath { get; set; }
        public string LogOutputPath { get; set; }

        public void SetDefault()
        {
            UserDataPath = GetDefaultUserDataPath();
            LogOutputPath = Path.Combine(UserDataPath, "Log");
        }

        private static string GetDefaultUserDataPath()
        {
            return BxUtils.GetDefaultUserDataPath(Cfg.GetConfig<GeneralConfig>().AppName);
        }
    }
}