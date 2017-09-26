using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viglacera.Models;
namespace Viglacera.Controllers.Display.Header
{
    public class headerController : Controller
    {
        private ViglaceraContext db = new ViglaceraContext();
        // GET: header
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult partialBanner()
        {
            StringBuilder result = new StringBuilder();
            StringBuilder resultBaogia = new StringBuilder();
            var menuParent = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == null).OrderBy(p => p.Ord).ToList();
            for(int i=0;i<menuParent.Count;i++)
            {
                if(menuParent[i].Baogia==true)
                {
                    resultBaogia.Append("<li class=\"li2\">");
                    resultBaogia.Append("<a href = \"/bao-gia/" + menuParent[i].Tag + "\" title=\"" + menuParent[i].Name + "\">" + menuParent[i].Name + "</a>");
                    resultBaogia.Append(" </li>   ");
                }
                result.Append("<li class=\"li2\">");
                result.Append("<a href = \"/"+menuParent[i].Tag+"\" title=\""+menuParent[i].Name+ "\">" + menuParent[i].Name + "  </ a>");
                int id = menuParent[i].id;
                var menuChild = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == id).OrderBy(p => p.Ord).ToList();
                if(menuChild.Count>0)
                {
                    result.Append("<ul class=\"ul3\">");
                    for(int j=0;j<menuChild.Count;j++)
                    {
                        if (menuChild[j].Baogia == true)
                        {
                            resultBaogia.Append("<li class=\"li2\">");
                            resultBaogia.Append("<a href = \"/bao-gia/" + menuChild[j].Tag + "\" title=\"" + menuChild[j].Name + "\">" + menuChild[j].Name + "</a>");
                            
                            resultBaogia.Append(" </li>   ");
                        }
                        int idcate = menuChild[j].id;
                        var listChildChild = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idcate).OrderBy(p => p.Ord).ToList();
                        if (listChildChild.Count > 0)
                        {
                            result.Append("<li><a href = \"/" + menuChild[j].Tag + "\" title=\"" + menuChild[j].Name + "\">" + menuChild[j].Name + " <i class=\"fa fa-angle-right\" aria-hidden=\"true\"></i></a>");
                        }
                        else
                            result.Append("<li><a href = \"/" + menuChild[j].Tag + "\" title=\"" + menuChild[j].Name + "\">" + menuChild[j].Name + "</a>");
                        if (listChildChild.Count > 0)
                        {
                            result.Append("<ul class=\"ul4\">");
                            for (int k = 0; k < listChildChild.Count; k++)
                            {
                                result.Append("<li><a href = \"/" + listChildChild[k].Tag + "\" title=\"" + listChildChild[k].Name + "\">" + listChildChild[k].Name + "</a></li>");

                            }
                            result.Append("</ul>");
                        }
                        result.Append(" </li>");
                    }
                    result.Append("</ul>");
                    
                }
              
                result.Append(" </li>   ");
            }
            ViewBag.resultMenu = result.ToString();
            ViewBag.resultBaogia = resultBaogia.ToString();

            return PartialView(db.tblConfigs.FirstOrDefault());
        }
        public ActionResult CommandSearch(FormCollection collection)
        {
            Session["Search"] = collection["txtSearch"];

            return Redirect("/product/productSearch");
        }
    }
}