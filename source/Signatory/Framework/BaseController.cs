using System.Collections.Generic;
using System.Web.Mvc;

namespace Signatory.Framework
{
    public class BaseController : Controller
    {
        public ActionResult Csv<T>(IEnumerable<T> list, string filename)
        {
            return new CsvActionResult<T>(list, filename);
        }
    }
}