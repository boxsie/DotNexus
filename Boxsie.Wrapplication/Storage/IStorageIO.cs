using System.Threading.Tasks;

namespace Boxsie.Wrapplication.Storage
{
    public interface IStorageIO<T>
    {
        Task WriteAsync(string filePath, T content);
        Task<T> ReadAsync(string filePath);
    }
}