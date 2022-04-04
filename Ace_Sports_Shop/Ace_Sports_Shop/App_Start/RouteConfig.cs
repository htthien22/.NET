using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ace_Sports_Shop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
              new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Product Category",
                url: "do-bong-da-{id}",
                defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "Ace_Sports_Shop.Controllers" }
            );
            routes.MapRoute(
                name: "Product Category gym",
                url: "do-tap-gym-{id}",
                defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "Ace_Sports_Shop.Controllers" }
            );
            routes.MapRoute(
                name: "Product Category yoga",
                url: "do-tap-yoga-{id}",
                defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "Ace_Sports_Shop.Controllers" }
            );
            routes.MapRoute(
                name: "Product Category pk",
                url: "phu-kien-{id}",
                defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "Ace_Sports_Shop.Controllers" }
            );
            routes.MapRoute(
                name: "Product",
                url: "san-pham/{Metatitle}-{id}",
                defaults: new { controller = "Product", action = "ProductDetails", id = UrlParameter.Optional },
                namespaces: new[] { "Ace_Sports_Shop.Controllers" }
            );

            routes.MapRoute(
               name: "Search",
               url: "Tim-kiem",
               defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
               namespaces: new[] { "Ace_Sports_Shop.Controllers" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "Lien-he/",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Ace_Sports_Shop.Controllers" }
           );

            routes.MapRoute(
              name: "Register",
              url: "Dang-ky",
              defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "Ace_Sports_Shop.Controllers" }
          );
            routes.MapRoute(
              name: "CheckPayment",
              url: "xac-nhan-don-hang",
              defaults: new { controller = "", action = "", id = UrlParameter.Optional },
              namespaces: new[] { "Ace_Sports_Shop.Controllers" }
          );
            routes.MapRoute(
              name: "DelPayment",
              url: "huy-don-hang",
              defaults: new { controller = "", action = "", id = UrlParameter.Optional },
              namespaces: new[] { "Ace_Sports_Shop.Controllers" }
          );
            routes.MapRoute(
               name: "logout",
               url: "Home",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Ace_Sports_Shop.Controllers" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Ace_Sports_Shop.Controllers" }
            );
        }
    }
}
