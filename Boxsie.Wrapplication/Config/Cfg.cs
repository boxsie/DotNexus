using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Config.Contracts;
using Boxsie.Wrapplication.Logging;
using Boxsie.Wrapplication.Storage.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boxsie.Wrapplication.Config
{
    public static class Cfg
    {
        private const string DebugFileExt = ".debug.json";
        private const string LiveFileExt = ".live.json";
        private static readonly string ConfigJsonPath = Path.Combine("Config", "Json");

        private static string _appDataPath;
        private static IBxLogger _logger;
        private static Dictionary<Type, IConfig> _configs;
        private static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
        
        public static void InitialiseConfig(IServiceProvider serviceProvider)
        {
            _appDataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), ConfigJsonPath);

            _configs = serviceProvider.GetServices<IConfig>().ToDictionary(x => x.GetType());

            if (!_configs.ContainsKey(typeof(GeneralConfig)))
                throw new FileNotFoundException($"Could not find the Genral Config");

            foreach (var cfgs in _configs.Values)
                cfgs.LoadUserConfigAsync(_appDataPath).GetAwaiter().GetResult();
            
            var userDataPath = GetConfig<GeneralConfig>().UserConfig.UserDataPath;

            _logger = serviceProvider.GetService<IBxLogger>();
            _logger.WriteLine($"Application data path set to {_appDataPath}", LogLevel.Debug);
            _logger.WriteLine($"General user data load complete", LogLevel.Information);
            _logger.WriteLine($"User data path set to {userDataPath}");

            if (Directory.Exists(userDataPath))
                return;

            _logger.WriteLine($"Application data directory not found, creating...", LogLevel.Debug);

            Directory.CreateDirectory(userDataPath);
        }

        public static T GetConfig<T>() where T : IConfig
        {
            var tType = typeof(T);

            if (_configs.ContainsKey(tType))
                return (T)_configs[tType];

            _logger.WriteLine($"Unable to get {tType}", LogLevel.Warning);

            return default(T);
        }

        public static async Task<string> GetConfigJsonAsync(string configName)
        {
            using (var store = new TextStore())
            {
                var cfgName = configName.ToLower();
                
                var liveFile = Path.Combine(_appDataPath, $"{cfgName}{LiveFileExt}");

                if (!File.Exists(liveFile))
                    throw new FileNotFoundException();

                var liveJson = await store.ReadAsync(liveFile);

                if (!IsDebug)
                    return liveJson;

                var debugFile = Path.Combine(_appDataPath, $"{cfgName}{DebugFileExt}");

                if (!File.Exists(debugFile))
                    return liveJson;

                var debugJson = await store.ReadAsync(debugFile);

                var jObjLive = JObject.Parse(liveJson);
                var jObjDebug = JObject.Parse(debugJson);

                foreach (var prop in jObjDebug.Properties())
                {
                    var targetProperty = jObjLive.Property(prop.Name);

                    if (targetProperty == null)
                        jObjDebug.Add(prop.Name, prop.Value);
                    else
                        targetProperty.Value = prop.Value;
                }

                return jObjLive.ToString(Formatting.None);
            }
        }

        public static T ConfigFactory<T>(string name = null) where T : IConfig
        {
            return (T)ConfigFactory(typeof(T), name);
        }

        public static IConfig ConfigFactory(Type configType, string name = null)
        {
            var configName = name ?? configType.Name.Replace("Config", "");

            return (IConfig)JsonConvert.DeserializeObject(GetConfigJsonAsync(configName).GetAwaiter().GetResult(), configType);
        }
    }
}