using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Viglacera.Models;

namespace Viglacera
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Menufactures", "{tag}-mm", new { controller = "Menufactures", action = "ManufacturesDetail", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^ManufacturesDetail$" });

            routes.MapRoute("productList", "{tag}.htm", new { controller = "product", action = "productDetail", tag = UrlParameter.Optional }, new { controller = "^p.*", action = "^productDetail$" });
            routes.MapRoute("productDetail", "{tag}.html", new { controller = "product", action = "productDetail", tag = UrlParameter.Optional }, new { controller = "^p.*", action = "^productDetail$" });
            routes.MapRoute("Bao-gia", "Bao-gia/{tag}", new { controller = "baogia", action = "baoGiaList", tag = UrlParameter.Optional }, new { controller = "^b.*", action = "^baoGiaList$" }); 
            routes.MapRoute(name: "Gioi-thieu", url: "gioi-thieu", defaults: new { controller = "news", action = "introduction" });
             routes.MapRoute("Chi-tiet-tin", "Tin-tuc/{tag}", new { controller = "news", action = "NewsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^NewsDetail$" });
            routes.MapRoute("Danh-sach-tin", "2/{tag}", new { controller = "news", action = "newsList", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^newsList$" });

            routes.MapRoute("Tag", "tags/{tag}", new { controller = "product", action = "productTag", tag = UrlParameter.Optional }, new { controller = "^p.*", action = "^productTag$" });
            routes.MapRoute("TagNew", "TagNews/{tag}", new { controller = "news", action = "newsTag", tag = UrlParameter.Optional }, new { controller = "^n.*", action = "^newsTag$" });
            routes.MapRoute(
    name: "CmsRoute",
    url: "{*tag}",
    defaults: new { controller = "product", action = "productList" },
    constraints: new { tag = new CmsUrlConstraint() }
);
            routes.MapRoute(name: "Admin", url: "Admin", defaults: new { controller = "Login", action = "LoginIndex" });
            routes.MapRoute(name: "Gio-hang", url: "Gio-hang", defaults: new { controller = "Order", action = "OrderIndex" });
            routes.MapRoute(name: "Lien-he", url: "Lien-he", defaults: new { controller = "contact", action = "Index" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        
    }
    }
}
