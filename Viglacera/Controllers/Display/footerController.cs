using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viglacera.Models;
namespace Viglacera.Controllers.Display
{
    public class footerController : Controller
    {
        private ViglaceraContext db = new ViglaceraContext();
        // GET: footer
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult partialFooter()
        {
            var listAgency = db.tblHotlines.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<listAgency.Count;i++)
            {
                result.Append(" <div class=\"tearFooter\">");
                result.Append("<p class=\"name\">"+listAgency[i].Name+"</p>");
                result.Append(" <p class=\"p\">ĐC: " + listAgency[i].Address + "</p>");
                result.Append(" <p class=\"p\">ĐT: " + listAgency[i].Hotline + " - FAX: " + listAgency[i].Mobile + "</p>");
                result.Append(" </div>");
            }
            ViewBag.result = result.ToString();
            return PartialView(db.tblConfigs.FirstOrDefault());
        }
    }
}