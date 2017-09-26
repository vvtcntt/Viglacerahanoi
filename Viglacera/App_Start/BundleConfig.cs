using System.Web;
using System.Web.Optimization;

namespace Viglacera
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Display/css/style").Include(
                     "~/Content/Display/css/font-awesome.css",
                     "~/Content/Display/css/news.css ",
                       "~/Content/Display/css/newsRs.css ",
                         "~/Content/Display/css/nivo-slider.css",
                           "~/Content/Display/css/owl.carousel.min.css ",
                             "~/Content/Display/css/owl.theme.default.min.css ",
                               "~/Content/Display/css/product.css ",
                                 "~/Content/Display/css/productRs.css ",
                                   "~/Content/Display/css/default.css ",
                                     "~/Content/Display/css/defaultRs.css ",
                                     "~/Content/Display/css/contact.css",
                                     "~/Content/Display/css/contact_Rs.css",
                                     "~/Content/PagedList.css",
                                     "~/Content/Display/css/baogia.css",
                     "~/Content/Display/css/baogia_Rs.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
