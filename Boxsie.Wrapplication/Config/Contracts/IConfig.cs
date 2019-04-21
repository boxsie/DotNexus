using System.Threading.Tasks;

namespace Boxsie.Wrapplication.Config.Contracts
{
    public interface IConfig
    {
        IUserConfig GetUserConfig();
        Task LoadUserConfigAsync(string appDataPath);
        Task LoadEncryptedUserConfigAsync(string appDataPath, string key);
    }
}