using System.Web.Optimization;

namespace CollaborativeBath
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-united.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/font").Include(
                "~/Content/open-iconic/font/css/open-iconic-bootstrap.css", new CssRewriteUrlTransform()));

            //Landing Page
            bundles.Add(new StyleBundle("~/Content/landingCss").Include(
                "~/Content/Landing/vendor/bootstrap/css/bootstrap.min.css",
                "~/Content/Landing/vendor/magnific-popup/magnific-popup.css",
                "~/Content/Landing/css/creative.css"));

            bundles.Add(new StyleBundle("~/Content/landingFont").Include(
                "~/Content/Landing/vendor/fontawesome-free/css/all.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/landingscripts").Include(
                "~/Scripts/Landing/bootstrap.bundle.min.js",
                "~/Scripts/Landing/jquery.easing.min.js",
                "~/Scripts/Landing/scrollreveal.min.js",
                "~/Scripts/Landing/jquery.magnific-popup.min.js",
                "~/Scripts/Landing/creative.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/pdf.js").Include(
                "~/Scripts/PDF.js/pdf.js",
                "~/Scripts/PDF.js/pdf.worker.js"));
        }
    }
}