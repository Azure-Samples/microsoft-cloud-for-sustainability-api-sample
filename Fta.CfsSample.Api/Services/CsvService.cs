using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fta.CfsSample.Api.Services
{
    public class CsvService<T> : ICsvService<T>
    {
        public async Task<(Stream Stream, string ContentType, string FileName)> GetCsvAsync(List<T> items, int rowCountThreshold)
        {
            var useStreaming = items.Count > rowCountThreshold;

            var contentType = "text/csv";
            var fileName = "items.csv";

            var headerRow = string.Join(",", GetHeaderNames(typeof(T)));

            if (useStreaming)
            {
                var memoryStream = new MemoryStream();

                var streamWriter = new StreamWriter(memoryStream);
                
                await streamWriter.WriteLineAsync(headerRow);

                foreach (var item in items)
                {
                    var line = string.Join(",", GetItemValues(item));
                    await streamWriter.WriteLineAsync(line);
                }

                memoryStream.Position = 0;
                
                return (memoryStream, contentType, fileName);
            }
            else
            {
                var csvLines = new List<string> { headerRow };
                csvLines.AddRange(items.Select(item => string.Join(",", GetItemValues(item))));

                var csvContent = string.Join(Environment.NewLine, csvLines);
                var byteArray = Encoding.UTF8.GetBytes(csvContent);
                var stream = new MemoryStream(byteArray);
                
                return (stream, contentType, fileName);
            }
        }

        private static IEnumerable<string> GetHeaderNames(Type type) => type.GetProperties().Select(property => property.Name);

        private static IEnumerable<string> GetItemValues(T item) => item!.GetType().GetProperties().Select(property => property.GetValue(item)?.ToString()!);
    }
}
