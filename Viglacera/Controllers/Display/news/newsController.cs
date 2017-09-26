using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viglacera.Models;
using PagedList;
using PagedList.Mvc;
namespace Viglacera.Controllers.Display.news
{
    public class newsController : Controller
    {
        private ViglaceraContext db = new ViglaceraContext();
        // GET: news
        public ActionResult Index()
        {
            return View();
        }
        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupNews.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = "<li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" title=\"" + ListMenu[i].Title + "\" href=\"/2/" + ListMenu[i].Tag + "\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\" \" /> </li> ›" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }

            }
            return nUrl;
        }
        public PartialViewResult partialNewsHomes()
        {
            var listNews = db.tblNews.Where(p => p.Active == true && p.idCate == 1).OrderByDescending(p => p.DateCreate).Take(6).ToList();
            StringBuilder result = new StringBuilder();
            StringBuilder result1 = new StringBuilder();
            for (int i=0;i<listNews.Count;i++)
            {
                if (i == 0)
                {
                    result.Append("<a href = \"/tin-tuc/"+ listNews[i].Tag+ "\" title = \""+listNews[i].Name+ "\" ><img src = \"" + listNews[i].Images + "\" alt = \"" + listNews[i].Name + "\" /></a >");
                    result.Append("<h4 ><a href = \"/tin-tuc/" + listNews[i].Tag + "\" title = \"" + listNews[i].Name + "\" > " + listNews[i].Name + "</a ></h4 >");
                    result.Append("<span >" + listNews[i].Description + "</ span >");
                }
                else
                {
                    result1.Append("<li><img src=\"" + listNews[i].Images + "\" alt=\"" + listNews[i].Name + "\" /><a href=\"/tin-tuc/" + listNews[i].Tag + "\" title=\"" + listNews[i].Name + "\">" + listNews[i].Name + "</a></li>");
                }
            }
            ViewBag.result = result.ToString();
            ViewBag.result1 = result1.ToString();
            return PartialView();
        }
        public ActionResult newsDetail(string tag)
        {
            var tblnews = db.tblNews.First(p => p.Tag == tag);
            int idUser = int.Parse(tblnews.idUser.ToString());
            ViewBag.Username = db.tblUsers.Find(idUser).UserName;
            int idCate = int.Parse(tblnews.idCate.ToString());
            var groupnews = db.tblGroupNews.First(p => p.id == idCate);
            ViewBag.NameMenu = groupnews.Name;
            ViewBag.Title = "<title>" + tblnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnews.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnews.Title + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + tblnews.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Viglacerahanoi.com/tin-tuc/" + StringClass.NameToTag(tag) + "\" />";

            meta += "<meta itemprop=\"name\" content=\"" + tblnews.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Viglacerahanoi.com" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnews.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Viglacerahanoi.com" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Descriptionss = tblnews.Description;
            ViewBag.Meta = meta;
            int id = int.Parse(tblnews.id.ToString());
            if (tblnews.Keyword != null)
            {
                string Chuoi = tblnews.Keyword;
                string[] Mang = Chuoi.Split(',');

                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {

                    string tabs = Mang[i].ToString();
                    var listnew = db.tblNews.Where(p => p.Keyword.Contains(tabs) && p.id != id && p.Active == true).ToList();
                    for (int j = 0; j < listnew.Count; j++)
                    {
                        araylist.Add(listnew[j].id);
                    }

                }


                var Lienquan = db.tblNews.Where(p => araylist.Contains(p.id) && p.Active == true && p.id != id).OrderByDescending(p => p.Ord).Take(3).ToList();
                string chuoinew = "";
                if (Lienquan.Count > 0)
                {

                    chuoinew += " <div class=\"Lienquan\">";
                    for (int i = 0; i > Lienquan.Count; i++)
                    {
                        chuoinew += "<a href=\"/tin-tuc/" + Lienquan[i].Tag + "\" title=\"" + Lienquan[i].Name + "\"><i class=\"fa fa-arrow - circle - o - right\" aria-hidden=\"true\"></i>  " + Lienquan[i].Name + " </a>";
                    }
                    chuoinew += "</div>";
                }
                ViewBag.chuoinew = chuoinew;


                //Load tin mới

            }

            string chuoinewnew = "";
            var NewsNew = db.tblNews.Where(p => p.Active == true && p.id != id).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < NewsNew.Count; i++)
            {
                chuoinewnew += "<li><a href=\"/tin-tuc/" + NewsNew[i].Tag + "\" title=\"" + NewsNew[i].Name + "\" rel=\"nofollow\">  <i class=\"fa fa-arrow - circle - o - right\" aria-hidden=\"true\"></i>  " + NewsNew[i].Name + " <span>" + NewsNew[i].DateCreate + "</span></a></li>";
            }
            ViewBag.chuoinewnews = chuoinewnew;

            //load tag
            string chuoitag = "";
            if (tblnews.Keyword != null)
            {
                string Chuoi = tblnews.Keyword;
                string[] Mang = Chuoi.Split(',');

                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {

                    chuoitag += "<h4><a href=\"/TagNews/" + StringClass.NameToTag(Mang[i]) + "\" title=\"" + Mang[i] + "\">" + Mang[i] + "</a></h4>";
                }
            }
            ViewBag.chuoitag = chuoitag;

            //Load root

            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›" + UrlNews(id) + "</ol><h2>" + tblnews.Name + "</h2>";
            return View(tblnews);
        }

        public ActionResult newsList(string tag, int? page)
        {
            var groupnew = db.tblGroupNews.First(p => p.Tag == tag);
            int idcate = groupnew.id;
            var listnews = db.tblNews.Where(p => p.idCate == idcate && p.Active == true).OrderByDescending(p => p.Ord).ToList();
            string chuoinewnew = "";
            var NewsNew = db.tblNews.Where(p => p.Active == true && p.idCate != idcate).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < NewsNew.Count; i++)
            {
                chuoinewnew += "<li><a href=\"/tin-tuc/" + NewsNew[i].Tag + "\" title=\"" + NewsNew[i].Name + "\" rel=\"nofollow\"> " + NewsNew[i].Name + " <span>" + NewsNew[i].DateCreate + "</span></a></li>";
            }
            ViewBag.chuoinewnews = chuoinewnew;
             
             const int pageSize = 10;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;

            ViewBag.Name = groupnew.Name;
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›" + UrlNews(idcate) + "</ol><h2>" + groupnew.Name + "</h2>";
            ViewBag.Title = "<title>" + groupnew.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnew.Keyword + "\" /> ";
            return View(listnews.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult newsTag(string tag, int? page)
        {

            
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;
            string name= db.tblNewsTags.Where(p => p.Tag == tag).Take(1).ToList()[0].Name;
            ViewBag.Name = name;
            var listidNews = db.tblNewsTags.Where(p => p.Tag == tag).Select(p => p.idn).ToList();
            var listnews = db.tblNews.Where(p => p.Active == true && listidNews.Contains(p.id)).OrderByDescending(p=>p.DateCreate).ToList();
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> › </ol><h2>" + name + "</h2>";
            ViewBag.Title = "<title>" + name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
            return View(listnews.ToPagedList(pageNumber, pageSize));   
        }
        public ActionResult introduction()
        {
            var groupnews = db.tblGroupNews.First(p => p.Tag == "gioi-thieu");
            int idCate = groupnews.id;
            var tblnews = db.tblNews.FirstOrDefault(p => p.idCate== idCate);
            int idUser = int.Parse(tblnews.idUser.ToString());
            ViewBag.Username = db.tblUsers.Find(idUser).UserName;
            ViewBag.NameMenu = groupnews.Name;
            ViewBag.Title = "<title>" + tblnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnews.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnews.Title + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + tblnews.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Viglacerahanoi.com/gioi-thieu\" />";

            meta += "<meta itemprop=\"name\" content=\"" + tblnews.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Viglacerahanoi.com" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnews.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Viglacerahanoi.com" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Descriptionss = tblnews.Description;
            ViewBag.Meta = meta;
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›" + UrlNews(idCate) + "</ol><h2>" + tblnews.Name + "</h2>";
            return View(tblnews);
        }

    }
}