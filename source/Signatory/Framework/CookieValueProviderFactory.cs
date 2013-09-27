using System.Web.Mvc;

namespace Signatory.Framework
{
    public class CookieValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new CookieValueProvider(controllerContext.HttpContext.Request.Cookies);
        }
    }
}