using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Viglacera.Models;
namespace Viglacera.Models
{
    public class CmsUrlConstraint : IRouteConstraint
    {
        private ViglaceraContext db = new ViglaceraContext();
        public bool Match(HttpContextBase httpContext, Route route, string parameterName,  RouteValueDictionary values, RouteDirection routeDirection)
        {
              if (values[parameterName] != null)
            {
                var tag = values[parameterName].ToString();
                 return db.tblGroupProducts.Any(p => p.Tag == tag);
            }
            return false;
        }
    }
}