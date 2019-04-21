namespace Boxsie.Wrapplication.Net
{
    public class HttpClientFactory
    {
        public HttpClientFactory()
        {
            
        }

        public HttpFileDownload GetHttpFileDownload()
        {
            return new HttpFileDownload();
        }
    }
}
