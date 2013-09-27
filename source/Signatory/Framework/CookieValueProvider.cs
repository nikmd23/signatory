using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Signatory.Framework
{
    // http://www.dotnetguy.co.uk/post/2010/03/07/aspnet-mvc-working-smart-with-cookies-custom-valueprovider/
    public class CookieValueProvider : IValueProvider
    {
        public CookieValueProvider(HttpCookieCollection cookies)
        {
            Cookies = cookies;
        }

        public HttpCookieCollection Cookies { get; set; }

        public bool ContainsPrefix(string prefix)
        {
            return Cookies[prefix] != null;
        }

        public ValueProviderResult GetValue(string key)
        {
            HttpCookie cookie = Cookies[key];
            return cookie != null ? new ValueProviderResult(cookie.Value, cookie.Value, CultureInfo.CurrentUICulture) : null;
        }
    }
}