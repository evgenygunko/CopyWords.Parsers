using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CopyWords.Parsers.Services
{
    public interface IFileDownloader
    {
        Task<string> DownloadPageAsync(string url, Encoding encoding);
    }

    public class FileDownloader : IFileDownloader
    {
        private readonly HttpClient _httpClient;

        public FileDownloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadPageAsync(string url, Encoding encoding)
        {
            string content = null;

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                content = encoding.GetString(bytes, 0, bytes.Length - 1);
            }
            else
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw new ServerErrorException("Server returned " + response.StatusCode);
                }
            }

            return content;
        }
    }
}
