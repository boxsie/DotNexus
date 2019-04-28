using System.IO;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Storage.Stores;
using Newtonsoft.Json;

namespace Boxsie.Wrapplication.Config.Contracts
{
    public abstract class BaseConfig<T> : ICfg, IConfig where T : IUserConfig, new()
    {
        public string UserConfigFilename { get; set; }

        [JsonIgnore]
        public T UserConfig { get; private set; }

        public IUserConfig GetUserConfig()
        {
            return UserConfig;
        }

        public async Task LoadUserConfigAsync(string appDataPath)
        {
            if (string.IsNullOrWhiteSpace(UserConfigFilename))
                return;

            var filePath = Path.Combine(appDataPath, UserConfigFilename);

            using (var js = new JsonStore<T>())
            {
                if (File.Exists(filePath))
                    UserConfig = await js.ReadAsync(filePath);
                else
                {
                    UserConfig = new T();
                    UserConfig.SetDefault();

                    await js.WriteAsync(filePath, UserConfig);
                }
            }
        }

        public async Task LoadEncryptedUserConfigAsync(string appDataPath, string key)
        {
            if (string.IsNullOrWhiteSpace(UserConfigFilename))
                return;

            var filePath = Path.Combine(appDataPath, UserConfigFilename);

            using (var js = new EncryptedJsonStore<T>(key))
            {
                if (File.Exists(filePath))
                    UserConfig = await js.ReadAsync(filePath);
                else
                {
                    UserConfig = new T();
                    UserConfig.SetDefault();

                    await js.WriteAsync(filePath, UserConfig);
                }
            }
        }
    }
}