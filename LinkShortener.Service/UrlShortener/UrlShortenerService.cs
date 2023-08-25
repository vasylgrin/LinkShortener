using LinkShortener.Domain.DTO.Request;
using LinkShortener.Domain.DTO.Response;
using LinkShortener.Domain.Mapper;
using LinkShortener.Entity.Exceptions;
using LinkShortener.Entity.Models;
using LinkShortener.Repository.Repositories;
using System.Text;

namespace LinkShortener.Service.UrlShortener
{
    public sealed class UrlShortenerService
    {
        private readonly IRepositoryBase<UrlModel> _repositoryBase;
        private const string SYMBOLS_FOR_CREATE_SHORT_URL = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz";
        private const int LENGHT_OF_SHORT_URL = 10;

        public UrlShortenerService()
        {
            _repositoryBase = new MSSQLRepository<UrlModel>();
        }

        public async Task<URLResponse> CreateShortUrlAsync(URLRequest urlRequest, HttpContextRequest httpContextRequest)
        {
            CheckInputUrlData(urlRequest);

            var urlModel = await _repositoryBase.FindAsync(url => url.LongUrl.Trim() == urlRequest.Url.Trim());
            if (urlModel != null)
            {
                return urlModel.ToUrlResponse();
            }

            string urlShortString = await CreateUniqShortUrl(httpContextRequest);

            urlModel = new UrlModel
            {
                ShortUrl = urlShortString,
                LongUrl = urlRequest.Url
            };
            await _repositoryBase.AddAsync(urlModel);

            return urlModel.ToUrlResponse();
        }


        public async Task<URLResponse> FindShortUrlAsync(URLRequest urlRequest)
        {
            CheckInputUrlData(urlRequest);

            var urlMatch = await _repositoryBase.FindAsync(url => url.ShortUrl.Trim() == urlRequest.Url.Trim());
            if (urlMatch == null)
            {
                throw new ShortLinkNotFound("Short link not found.", urlRequest.Url);
            }

            return urlMatch.ToUrlResponse();
        }

        public async Task<IEnumerable<UrlModel>> GetAllShortUrl()
        {
            return await _repositoryBase.LoadAsync();
        }

        public async Task<bool> RemoveShortUrl(int id)
        {
            var urlForRemoving = await _repositoryBase.FindAsync(id);
            await _repositoryBase.RemoveAsync(urlForRemoving);
            return true;
        }


        private async Task<string> CreateUniqShortUrl(HttpContextRequest httpContextRequest)
        {
            string shortUrl;
            UrlModel urlModel;

            do
            {
                shortUrl = CreateShortUrl(httpContextRequest);
                urlModel = await _repositoryBase.FindAsync(url => url.ShortUrl == shortUrl);

            } while (urlModel != null);

            return shortUrl;

        }

        private string CreateShortUrl(HttpContextRequest httpContextRequest)
        {
            var random = new Random();
            var shortUrl = new StringBuilder($"{httpContextRequest.Scheme}://{httpContextRequest.Host}/", LENGHT_OF_SHORT_URL);

            for (int i = 0; i < LENGHT_OF_SHORT_URL; i++)
            {
                int indexOfSymbols = random.Next(0, SYMBOLS_FOR_CREATE_SHORT_URL.Length);
                shortUrl.Append(SYMBOLS_FOR_CREATE_SHORT_URL[indexOfSymbols]);
            }

            return shortUrl.ToString();
        }

        private void CheckInputUrlData(URLRequest urlRequest)
        {
            if (string.IsNullOrWhiteSpace(urlRequest.Url))
            {
                throw new ArgumentNullException(nameof(urlRequest), "Url cannot be null or whitespace.");
            }

            bool isValidUrl = Uri.TryCreate(urlRequest.Url, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            if (!isValidUrl)
            {
                throw new InvalidDataException("Invalid url.");
            }
        }
    }
}
