using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fta.CfsSample.Api.Services
{
    public interface ICsvService<T>
    {
        Task<(Stream Stream, string ContentType, string FileName)> GetCsvAsync(List<T> items, int rowCountThreshold);
    }
}
