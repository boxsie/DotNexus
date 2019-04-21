using Boxsie.Wrapplication.Config.Contracts;

namespace Boxsie.Wrapplication.Config
{
    public partial class GeneralConfig : BaseConfig<GeneralUserConfig>
    {
        public string AppName { get; set; }
        public string DbFilename { get; set; }
        public string UserDataDirName { get; set; }
        public string EncryptKeyBase { get; set; }
    }
}