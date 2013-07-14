using MarkdownSharp;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Signatory.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static readonly IDictionary<string, IHtmlString> cache = new Dictionary<string, IHtmlString>();

        public static IHtmlString RenderMarkdownFile(this HtmlHelper htmlHelper, string path, string repo)
        {
            var src = htmlHelper.ViewContext.HttpContext.Server.MapPath(path);

            var mdFile = new FileInfo(src);

            var cacheKey = mdFile.FullName.ToLower();

#if !DEBUG
            if (cache.ContainsKey(cacheKey))
                return cache[cacheKey];
#endif

            var markdown = string.Empty;
            using (var reader = mdFile.OpenText())
            {
                markdown = reader.ReadToEnd();
            }

            var result = htmlHelper.RenderMarkdown(markdown.Replace("{repo}", repo));

            cache[cacheKey] = result;

            return result;
        }

        public static IHtmlString RenderMarkdown(this HtmlHelper htmlHelper, string markdown)
        {
            return htmlHelper.Raw(new Markdown().Transform(markdown));
        }
    }
}