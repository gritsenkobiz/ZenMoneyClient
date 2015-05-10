using System.Threading.Tasks;

namespace Gritsenko.Universal.Abstract
{
    public interface ISettingsService
    {
        TSetting Load<TSetting>(string key);

        Task Save(string key, object value);
    }
}