using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viglacera.Models;
namespace Viglacera.Controllers.Default
{
    public class DefaultController : Controller
    {
        // GET: Default
        private ViglaceraContext db = new ViglaceraContext();
        public ActionResult Index()
        {
            tblConfig config = db.tblConfigs.First();
            ViewBag.Title = "<title>" + config.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + config.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.Keywords + "\" /> ";
            ViewBag.h1 = "<h1 class=\"h1\">" + config.Title + "</h1>";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://viglacerahanoi.com\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + config.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + config.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://viglacerahanoi.com" + config.Logo + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + config.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://viglacerahanoi.com" + config.Logo + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Loiloc\" />";
            meta += "<meta property=\"og:description\" content=\"" + config.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            ViewBag.chuoi = config.Content;
            return View();
        }
        public PartialViewResult partialSlide()
        {
            var listImages = db.tblImages.Where(p => p.Active == true && p.idCate == 1).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<listImages.Count;i++)
            {
                result.Append("<a href = \""+listImages[i].Url+"\" title = \""+listImages[i].Name+ "\" > <img src = \"" + listImages[i].Images + "\" data-thumb = \"" + listImages[i].Images + "\" alt = \"" + listImages[i].Name + "\" /></a>");
            }
            ViewBag.result = result.ToString();
            var tblVideo = db.tblVideos.FirstOrDefault();

            return PartialView(tblVideo);
        }
        public PartialViewResult partialVideoAndCatalogue()
        {
            StringBuilder resultPrize = new StringBuilder();
            StringBuilder resultCatalogue = new StringBuilder();
            var listImages = db.tblImages.Where(p => p.Active == true && (p.idCate == 3 || p.idCate == 4)).OrderBy(p => p.Ord).ToList();
            for(int i=0;i<listImages.Count;i++)
            {
                if(listImages[i].idCate==3)
                {
                    resultCatalogue.Append("<a href = \""+ listImages[i].Url + "\" title = \"" + listImages[i].Name + "\" ><img src = \"" + listImages[i].Images + "\" alt = \"" + listImages[i].Name + "\" /></a >");
                }
                else
                {
                    resultPrize.Append("<a href = \"" + listImages[i].Url + "\" title = \"" + listImages[i].Name + "\" ><img src = \"" + listImages[i].Images + "\" alt = \"" + listImages[i].Name + "\" /></a >");

                }
            }
            ViewBag.resultPrize = resultPrize.ToString();
            ViewBag.resultCatalogue = resultCatalogue.ToString();
            return PartialView();
        }
        public PartialViewResult partialdefault()
        {
            return PartialView(db.tblConfigs.First());
        }
    }
}