using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinkShortener.Service.UrlShortener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkShortener.Domain.DTO;
using LinkShortener.Domain.DTO.Request;
using System.Net;

namespace LinkShortener.Service.UrlShortener.Tests
{
    [TestClass()]
    public class UrlShortenerServiceTests
    {
        private const string url = "https://github.com/vasylgrin";


        [TestMethod()]
        public async Task ShortUrl_CorrectLongUrl_ShortUrl()
        {
            // arrange

            var urlShortenerService = new UrlShortenerService();
            var urlRequest = new URLRequest { Url = url };
            var httpContextRequest = new HttpContextRequest { Scheme = "http", Host = "5090" };

            // act

            var urlResponse = await urlShortenerService.CreateShortUrlAsync(urlRequest, httpContextRequest);

            #region Check for working short url

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(shortUrl.Url);
            //request.Method = "HEAD";
            //request.Timeout = 5000;

            //try
            //{
            //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //    {
            //        if(response.StatusCode != HttpStatusCode.OK)
            //        {
            //            Assert.Fail("Short link returned status code inst OK.");
            //        }
            //    }
            //}
            //catch (WebException ex)
            //{
            //    Assert.Fail("Short link inst correct.  " + ex.Message);
            //}

            #endregion

            // assert

            if (string.IsNullOrWhiteSpace(urlResponse.ShortUrl))
            {
                Assert.Fail("ShortUrl wasn't created.");
            }
        }

        [TestMethod()]
        public async Task ShortUrl_InvalidUrl_InvalidDataException()
        {
            // arrange

            var urlShortenerService = new UrlShortenerService();
            var urlRequest = new URLRequest { Url = "string" };
            var httpContextRequest = new HttpContextRequest { Scheme = "invalid", Host = "invalid" };

            // act
            // assert

            await Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await urlShortenerService.CreateShortUrlAsync(urlRequest, httpContextRequest));
        }
    }
}