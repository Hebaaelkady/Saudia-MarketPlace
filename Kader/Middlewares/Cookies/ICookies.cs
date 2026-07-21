using System.Threading.Tasks;

namespace Kader.Middlewares.Cookies
{
    public interface ICookies
    {
        Task<bool> Add(string Name, string Value, double? expireTime = 30);
        Task<string> Read(string Name);
        Task<T> Read<T>(string Name);
        Task<bool> Remove(string Name);
    }
}
