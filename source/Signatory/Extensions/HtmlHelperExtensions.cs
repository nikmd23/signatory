using MarkdownSharp;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Signatory.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString RenderMarkdown(this HtmlHelper htmlHelper, string markdown)
        {
            return htmlHelper.Raw(new Markdown().Transform(markdown));
        }
    }
}