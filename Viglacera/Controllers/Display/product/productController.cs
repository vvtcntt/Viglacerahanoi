using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viglacera.Models;
using PagedList;
using PagedList.Mvc;
namespace Viglacera.Controllers.Display.product
{
    public class productController : Controller
    {
        private ViglaceraContext db = new ViglaceraContext();
        // GET: product
        public ActionResult Index()
        {
           
            return View();
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        public List<string> ArrayidParent(int idParent)
        {

            var ListMenu = db.tblGroupProducts.FirstOrDefault(p => p.id == idParent);
            if (ListMenu != null)
            {
                Mangphantu.Add(ListMenu.id.ToString());
                string Parentid = ListMenu.ParentID.ToString();
                if (Parentid != null && Parentid != "")
                {
                    int id = int.Parse(Parentid);
                    ArrayidParent(id);
                }

            }



            return Mangphantu;
        }
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = "<li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" title=\"" + ListMenu[i].Title + "\" href=\"/" + ListMenu[i].Tag + "\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"" + (ListMenu[i].Level + 2) + "\" /> </li> ›" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        public PartialViewResult partialProductHomes()
        {
            var listProduct = db.tblProducts.Where(p => p.Active == true).OrderByDescending(p => p.DateCreate).Take(20).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<listProduct.Count;i++)
            {
                result.Append("<div class=\"item\">");
                result.Append(" <div class=\"img\">");
                result.Append("<a href = \"/"+listProduct[i].Tag+ ".html\" title=\"" + listProduct[i].Name + "\"><img src = \"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" /></a>");
                result.Append("</div>");
                result.Append("<h3><a href = \"/" + listProduct[i].Tag + ".html\" title=\"" + listProduct[i].Name + "\">" + listProduct[i].Name + "</a></h3>");
                result.Append("</div>");
            }ViewBag.result = result.ToString();
            return PartialView();
        }
        public ActionResult productDetail(string tag)
        {
            var tblproduct = db.tblProducts.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + tblproduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblproduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblproduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblproduct.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Viglacerahanoi.com/" + StringClass.NameToTag(tag) + ".htm\" />";
            string meta = "";
            tblproduct.Visit = tblproduct.Visit + 1;
            db.SaveChanges();
            ViewBag.visit = tblproduct.Visit;
            meta += "<meta itemprop=\"name\" content=\"" + tblproduct.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblproduct.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Viglacerahanoi.com" + tblproduct.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblproduct.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Viglacerahanoi.com" + tblproduct.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblproduct.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; int idcate = int.Parse(tblproduct.idCate.ToString());
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›" + UrlProduct(idcate) + "</ol><h2>" + tblproduct.Title + "</h2>";
            string chuoitag = "";
            if (tblproduct.Keyword != null)
            {
                string Chuoi = tblproduct.Keyword;
                string[] Mang = Chuoi.Split(',');
                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {
                    string tagsp = StringClass.NameToTag(Mang[i]);
                    chuoitag += "<h4><a href=\"/tags/" + tagsp + "\" title=\"" + Mang[i] + "\">" + Mang[i] + "</a></h4>";
                }
            }
            ViewBag.chuoitag = chuoitag;
             var listIdCre= db.tblGroupCriterias.Where(p => p.idCate == idcate).Select(p=>p.idCri).ToList();
            int id = tblproduct.id;
             var ListCri = db.tblCriterias.Where(p => listIdCre.Contains(p.id) && p.Active == true).ToList();
            string chuoi = "";
            #region[Lọc thuộc tính]
            for (int i = 0; i < ListCri.Count; i++)
            {
                int idCre = int.Parse(ListCri[i].id.ToString());
                var ListCr = (from a in db.tblConnectCriterias
                              join b in db.tblInfoCriterias on a.idCre equals b.id
                              where a.idpd == id && b.idCri == idCre && b.Active == true
                              select new
                              {
                                  b.Name,
                                  b.Url,
                                  b.Ord
                              }).OrderBy(p => p.Ord).ToList();
                if (ListCr.Count > 0)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + ListCri[i].Name + "</td>";
                    chuoi += "<td>";
                    int dem = 0;
                    string num = "";
                    if (ListCr.Count > 1)
                        num = "⊹ ";
                    foreach (var item in ListCr)
                        if (item.Url != null && item.Url != "")
                        {
                            chuoi += "<a href=\"" + item.Url + "\" title=\"" + item.Name + "\">";
                            if (dem == 1)
                                chuoi += num + item.Name;
                            else
                                chuoi += num + item.Name;
                            dem = 1;
                            chuoi += "</a>";
                        }
                        else
                        {
                            if (dem == 1)
                                chuoi += num + item.Name + "</br> ";
                            else
                                chuoi += num + item.Name + "</br> "; ;
                            dem = 1;
                        }
                    chuoi += "</td>";
                    chuoi += " </tr>";
                }
            }
            #endregion
            ViewBag.chuoi = chuoi;
            StringBuilder result = new StringBuilder();
            result.Append("<div class=\"tearProduct\">");
            result.Append("<div class=\"nvarProduct\">");
            result.Append("<span>Sản phẩm liên quan khác</span>");
            result.Append("</div>");
            result.Append("<div class=\"contentTearProduct\">");
  
            int count1 = 0;
            int count2 = 0;
            List<int> Mangid = new List<int>();
              var listProduct1= db.tblProducts.Where(p => p.Active == true && p.idCate == idcate && p.id<id).OrderByDescending(p => p.id).Take(5).Select(p=>p.id).ToList();
            var listProduct2 = db.tblProducts.Where(p => p.Active == true && p.idCate == idcate && p.id > id).OrderBy(p => p.id).Take(5).Select(p => p.id).ToList();
            if(listProduct1.Count<5)
            {
                count1 = 5-listProduct1.Count;
                  listProduct2 = db.tblProducts.Where(p => p.Active == true && p.idCate == idcate && p.id > id).Take(5+count1).OrderBy(p => p.id).Select(p => p.id).ToList();
             }
            if (listProduct2.Count < 5)
            {
                count2 = 5 - listProduct2.Count;
                 listProduct1 = db.tblProducts.Where(p => p.Active == true && p.idCate == idcate && p.id< id).Take(5 + count2).OrderByDescending(p => p.id).Select(p => p.id).ToList();
                 
            }
            var listProduct = db.tblProducts.Where(p => p.Active == true  && (listProduct1.Contains(p.id) || listProduct2.Contains(p.id))).OrderByDescending(p => p.Ord).ToList();
            for (int j = 0; j < listProduct.Count; j++)
            {
                result.Append("<div class=\"tearPd\">");
                result.Append("<div class=\"img\">");
                result.Append("<a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\"><img src = \"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" /></a>");
                result.Append("</div>");
                result.Append(" <a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a> ");
                result.Append("<p>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " vnđ</p>");
                result.Append("</div> ");
            }
            result.Append("</div>");
            result.Append("</div>");
            ViewBag.result = result.ToString();
            StringBuilder resultInvolve = new StringBuilder();
            var listProductCodeInvolve = db.tblProductInvolves.Where(p => p.idP == id).Select(p=>p.Code).ToList();
            var listProductInvolve = db.tblProducts.Where(p => p.Active == true && listProductCodeInvolve.Contains(p.Code)).OrderBy(p => p.Ord).ToList();
            for(int i=0;i<listProductInvolve.Count;i++)
            {
                resultInvolve.Append("  <div class=\"tearInvolve\">");
                resultInvolve.Append(" 	<a href = \"/"+listProductInvolve[i].Tag+ ".htm\" title=\"" + listProductInvolve[i].Name + "\" target=\"_blank\"><img src = \"" + listProductInvolve[i].ImageLinkThumb + "\" alt=\"" + listProductInvolve[i].Name + "\" /></a>");
                resultInvolve.Append("  <a href = \"/" + listProductInvolve[i].Tag + ".htm\" target=\"_blank\" title=\"" + listProductInvolve[i].Name + "\">" + listProductInvolve[i].Name + "</a>");
                resultInvolve.Append("  <p>Giá sản phẩm : <span>"+string.Format("{0:#,#}",listProductInvolve[i].PriceSale)+"đ</span> </p>");
                resultInvolve.Append("   </div>");
            }
            ViewBag.resultInvolve = resultInvolve;
            return View(tblproduct);
        }
        public ActionResult productList(string tag, int? page)
        {
            StringBuilder result = new StringBuilder();
            var Groupproduct = db.tblGroupProducts.FirstOrDefault(p => p.Tag == tag);
            int id = Groupproduct.id;
            ViewBag.content = Groupproduct.Content;
            ViewBag.Headshort = Groupproduct.Content;
            ViewBag.h1 = Groupproduct.Name;
            ViewBag.Title = "<title>" + Groupproduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + Groupproduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + Groupproduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Groupproduct.Keyword + "\" /> ";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Viglacerahanoi.com/" + StringClass.NameToTag(tag) + "\" />";
            meta += "<meta itemprop=\"name\" content=\"" + Groupproduct.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + Groupproduct.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"" + Groupproduct.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + Groupproduct.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›" + UrlProduct(id) + "</ol><h2>" + Groupproduct.Title + "</h2>";
             //Phân trang
            const int pageSize = 20;
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
            var listMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == id).OrderBy(p => p.Ord).ToList();
            var listProducty = db.tblProducts.Take(0).ToList();

            if (listMenu.Count>0)
            {
                for (int i = 0; i < listMenu.Count; i++)
                {
                    result.Append("<div class=\"tearProduct\">");
                    result.Append("<div class=\"nvarProduct\">");
                    result.Append("<h2><a href = \"/" + listMenu[i].Tag + "\" title=\"" + listMenu[i].Name + "\">" + listMenu[i].Name + "</a></h2>");
                    result.Append("</div>");
                    result.Append("<div class=\"contentTearProduct\">");
                    int idCate = listMenu[i].id;
                    var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderBy(p => p.Ord).Take(10).ToList();
                     for (int j = 0; j < listProduct.Count; j++)
                    {
                        result.Append("<div class=\"tearPd\">");
                        result.Append("<div class=\"img\">");
                        result.Append("<a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\"><img src = \"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" /></a>");
                        result.Append("</div>");
                        result.Append("<h3><a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                        result.Append("<p>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " vnđ</p>");
                        result.Append("</div> ");
                    }
                    result.Append("</div>");
                    var kiemtra = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderBy(p => p.Ord).ToList();
                    if (kiemtra.Count >10)
                    result.Append("<div class=\"viewDetail\"><a href = \"/" + listMenu[i].Tag + "\" title=\"" + listMenu[i].Name + "\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i> Xem thêm <span>" + (kiemtra.Count-10)+"</span> sản phẩm</a></div>");
                    result.Append("</div>");
                }
 
            }
            else
            {
                
                    result.Append("<div class=\"tearProduct\">");
                    result.Append("<div class=\"nvarProduct\">");
                    result.Append("<h2><a href = \"/" + Groupproduct.Tag + "\" title=\"" + Groupproduct.Name + "\">" + Groupproduct.Name + "</a></h2>");
                    result.Append("</div>");
                    result.Append("<div class=\"contentTearProduct\">");
                     var listProducts = db.tblProducts.Where(p => p.Active == true && p.idCate == id).OrderBy(p => p.Ord).ToList();
                    var listProduct = listProducts.ToPagedList(pageNumber, pageSize);
                    for (int j = 0; j < listProduct.Count; j++)
                    {
                        result.Append("<div class=\"tearPd\">");
                        result.Append("<div class=\"img\">");
                        result.Append("<a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\"><img src = \"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" /></a>");
                        result.Append("</div>");
                        result.Append("<h3><a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                        result.Append("<p>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " vnđ</p>");
                        result.Append("</div> ");
                    }
                    result.Append("</div>");
                    result.Append("</div>"); ViewBag.result = result.ToString();
                return View(listProduct);

            }

            ViewBag.result = result.ToString();

            return View(listProducty.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult productTag(string tag, int? page)
        {
            string name = db.tblProductTags.Where(p => p.Tag == tag).Take(1).ToList()[0].Name;
            const int pageSize = 20;
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
            var tbltags = db.tblTags.Where(p => p.Tag == tag && p.Active == true).ToList();
            if (tbltags.Count > 0)
            {
                ViewBag.Title = "<title>" + tbltags[0].Name + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tbltags[0].Name + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"" + tbltags[0].Description + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tbltags[0].Keyword + "\" /> ";
                string meta = "";
                meta += "<meta itemprop=\"name\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta itemprop=\"description\" content=\"" + tbltags[0].Description + "\" />";
                meta += "<meta itemprop=\"image\" content=\"\" />";
                meta += "<meta property=\"og:title\" content=\"" + tbltags[0].Name + "\" />";
                meta += "<meta property=\"og:type\" content=\"product\" />";
                meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta property=\"og:image\" content=\"\" />";
                meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
                meta += "<meta property=\"og:description\" content=\"" + tbltags[0].Description + "\" />";
                meta += "<meta property=\"fb:admins\" content=\"\" />"; ViewBag.Meta = meta;
                ViewBag.Headshort = tbltags[0].Content;
            }
            else
            {
                ViewBag.Title = "<title>" + name + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + name + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
                string meta = "";
                meta += "<meta itemprop=\"name\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta itemprop=\"description\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"image\" content=\"\" />";
                meta += "<meta property=\"og:title\" content=\"" + name + "\" />";
                meta += "<meta property=\"og:type\" content=\"product\" />";
                meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta property=\"og:image\" content=\"\" />";
                meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
                meta += "<meta property=\"og:description\" content=\"" + name + "\" />";
                meta += "<meta property=\"fb:admins\" content=\"\" />"; ViewBag.Meta = meta;
            }


            var listIdProduct = db.tblProductTags.Where(p => p.Tag == tag).Select(p => p.idp).ToList();

            var listProducts = db.tblProducts.Where(p => p.Active == true && listIdProduct.Contains(p.id)).OrderBy(p => p.PriceSale).ToList();
            ViewBag.h1 = name;
            StringBuilder result = new StringBuilder();
            result.Append("<div class=\"tearProduct\">");
            result.Append("<div class=\"nvarProduct\">");
            result.Append("<h1><a href = \"/tags/" + tag + "\" title=\"" + name + "\">" + name + "</a></h1>");
            result.Append("</div>");
            result.Append("<div class=\"contentTearProduct\">");
             var listProduct = listProducts.ToPagedList(pageNumber, pageSize);
            for (int j = 0; j < listProduct.Count; j++)
            {
                result.Append("<div class=\"tearPd\">");
                result.Append("<div class=\"img\">");
                result.Append("<a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\"><img src = \"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" /></a>");
                result.Append("</div>");
                result.Append("<h3><a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                result.Append("<p>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " vnđ</p>");
                result.Append("</div> ");
            }
            result.Append("</div>");
            result.Append("</div>"); ViewBag.result = result.ToString();
              
            ViewBag.result = result.ToString();
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›</ol><h2>" + name + "</h2>";
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult productSearch(string tag, int? page)
        {
            
            const int pageSize = 20;
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
            if (Session["Search"] != null && Session["Search"] != "")
            {
                string name = Session["Search"].ToString();
                ViewBag.Title = "<title>" + name + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + name + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
                string meta = "";
                meta += "<meta itemprop=\"name\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta itemprop=\"description\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"image\" content=\"\" />";
                meta += "<meta property=\"og:title\" content=\"" + name + "\" />";
                meta += "<meta property=\"og:type\" content=\"product\" />";
                meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta property=\"og:image\" content=\"\" />";
                meta += "<meta property=\"og:site_name\" content=\"http://Viglacerahanoi.com\" />";
                meta += "<meta property=\"og:description\" content=\"" + name + "\" />";
                meta += "<meta property=\"fb:admins\" content=\"\" />"; ViewBag.Meta = meta;  
                var listProducts = db.tblProducts.Where(p=>p.Active==true &&(p.Name.Contains(name) || p.Description.Contains(name))).ToList();
                ViewBag.h1 = name;
                StringBuilder result = new StringBuilder();
                result.Append("<div class=\"tearProduct\">");
                result.Append("<div class=\"nvarProduct\">");
                result.Append("<h1><a href = \"/tags/" + tag + "\" title=\"" + name + "\">" + name + "</a></h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentTearProduct\">");
                var listProduct = listProducts.ToPagedList(pageNumber, pageSize);
                for (int j = 0; j < listProduct.Count; j++)
                {
                    result.Append("<div class=\"tearPd\">");
                    result.Append("<div class=\"img\">");
                    result.Append("<a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\"><img src = \"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" /></a>");
                    result.Append("</div>");
                    result.Append("<h3><a href = \"/" + listProduct[j].Tag + ".html\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>");
                    result.Append("<p>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " vnđ</p>");
                    result.Append("</div> ");
                }
                result.Append("</div>");
                result.Append("</div>"); ViewBag.result = result.ToString();

                ViewBag.result = result.ToString(); ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> ›</ol><h2>" + name + "</h2>";
                return View(listProduct.ToPagedList(pageNumber, pageSize));

            }
            return View( );
        }

    }
}