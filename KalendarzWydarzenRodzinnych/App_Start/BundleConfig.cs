﻿using System.Web;
using System.Web.Optimization;

namespace KalendarzWydarzenRodzinnych
{
    public class BundleConfig
    {
        // Aby uzyskać więcej informacji o grupowaniu, odwiedź stronę https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap.min.js",
                        "~/Scripts/MVCDataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/img").Include(
                         "~/Scripts/imageForm.js"));

            bundles.Add(new ScriptBundle("~/bundles/ekko").Include(
                         "~/Scripts/ekko-lightbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/bootstrap-datetimepicker.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                        "~/Scripts/chosen/chosen.jquery.min.js",
                        "~/Scripts/docsupport/prism.js",
                        "~/Scripts/docsupport/init.js"
                       ));



            // Użyj wersji deweloperskiej biblioteki Modernizr do nauki i opracowywania rozwiązań. Następnie, kiedy wszystko będzie
            // gotowe do produkcji, użyj narzędzia do kompilowania ze strony https://modernizr.com, aby wybrać wyłącznie potrzebne testy.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                      "~/Content/themes/base/all.css"));

            bundles.Add(new StyleBundle("~/Content/ekko").Include(
                      "~/Content/ekko-lightbox.css"));

            bundles.Add(new StyleBundle("~/Content/chosen").Include(
                      "~/Content/docsupport/style.css",
                     "~/Content/docsupport/prism.css",
                    "~/Content/chosen/chosen.css"));
        }
    }
}
