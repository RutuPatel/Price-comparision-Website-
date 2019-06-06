using System.Web;
using System.Web.Optimization;

namespace Price_Grabber
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Content/Theme/plugins/jquery-3.1.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/globleFooterJs").Include(
          "~/Content/Theme/plugins/jquery-3.1.0.js",
           "~/Content/Theme/plugins/jquery-migrate.js",
           "~/Content/Theme/plugins/bootstrap/js/bootstrap.js",
           "~/Content/Theme/js/back-to-top.js",
           "~/Content/Theme/plugins/jquery-slimscroll/jquery.slimscroll.js",
           "~/Content/Theme/plugins/StarRating/js/star-rating.min.js"
           //"~/Content/Theme/js/layout.js"
           ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));


            //bundles.Add(new ScriptBundle("~/bundles/globleJs").Include(
            //      "~/Content/Theme/plugins/jquery.js",
            //      "~/Content/Theme/plugins/jquery-migrate.js",
            //      "~/Content/Theme/plugins/bootstrap/js/bootstrap.js",
            //      "~/Content/Theme/plugins/back-to-top.js",
            //      "~/Content/Theme/plugins/jquery-slimscroll/jquery.slimscroll.js"
            //           ));


            bundles.Add(new ScriptBundle("~/bundles/Fancybox").Include(
                  "~/Content/Theme/plugins/fancybox/source/jquery.fancybox.pack.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/OwlCarousel").Include(
              "~/Content/Theme/plugins/owl.carousel/owl.carousel.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Zoom").Include(
          "~/Content/Theme/plugins/zoom/jquery.zoom.js"
            ));
 

            bundles.Add(new ScriptBundle("~/bundles/BootstrapTouchspin").Include(
          "~/Content/Theme/plugins/bootstrap-touchspin/bootstrap.touchspin.js"
            ));
 
 
            bundles.Add(new ScriptBundle("~/bundles/Uniform").Include(
          "~/Content/Theme/plugins/uniform/jquery.uniform.js"
            ));

    

 
            bundles.Add(new StyleBundle("~/Content/globleCss").Include(
                      "~/Content/Theme/plugins/bootstrap/css/bootstrap.css",
                      "~/Content/Theme/plugins/font-awesome/css/font-awesome.css",
                      "~/Content/Theme/plugins/StarRating/css/star-rating.min.css"));
              

                bundles.Add(new StyleBundle("~/Content/ThemeCss").Include(
                    "~/Content/Theme/css/components.css",
                    "~/Content/Theme/css/style.css",
                    "~/Content/Theme/css/style-shop.css",
                    "~/Content/Theme/css/style-responsive.css",
                    "~/Content/Theme/css/themes/red.css",
                    "~/Content/Theme/css/custom.css"));

            bundles.Add(new StyleBundle("~/Content/pagelevelCss").Include(
                    "~/Content/Theme/css/components.css",
                    "~/Content/Theme/css/slider.css",
                    "~/Content/Theme/css/style.css"
                    ));
        }
    }
}
