using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsulExample;

public interface IConsulService
{
    Task<ICollection<string>> GetKeysAsync();
    Task<T> GetValueAsync<T>(string key);
    Task<object> GetValueAsync(string key);
    Task DeleteKeyAsync(string key);
    Task UpdateAsync(string key, object value);
    Task CreateAsync(string key, object value);
}