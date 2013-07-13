using System.Web.Optimization;

// ReSharper disable CheckNamespace
namespace Signatory
// ReSharper restore CheckNamespace
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleBundles(bundles);
            RegisterJavascriptBundles(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
                            .Include("~/Content/bootstrap.css")
                            .Include("~/Content/bootstrap-responsive.css")
                            .Include("~/Content/font-awesome.css")
                            .Include("~/Content/site.css"));
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js")
                            .Include("~/Scripts/jquery-{version}.js")
                            .Include("~/Scripts/jquery-ui-{version}.js")
                            .Include("~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/jSignature")
                            .Include("~/Scripts/jSignature.js"));
        }
    }
}