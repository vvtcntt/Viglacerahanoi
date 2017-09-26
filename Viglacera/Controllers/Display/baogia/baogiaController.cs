using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viglacera.Models;
namespace Viglacera.Controllers.Display.baogia
{
    public class baogiaController : Controller
    {
        // GET: baogia
        private ViglaceraContext db = new ViglaceraContext();

        // GET: baogia
        public ActionResult Index()
        {
            //    var listBaoGia = db.tblGroupProducts.Where(p => p.Active == true && p.Baogia == true).OrderBy(p => p.Ord).ToList();
            //    StringBuilder resultBaoGia = new StringBuilder();
            //    for (int i = 0; i < listBaoGia.Count; i++)
            //    {
            //        resultBaoGia.Append("<div class=\"tear_Bg\">");
            //        resultBaoGia.Append("<a href=\"/bao-gia/" + listBaoGia[i].Tag + "\" title=\"Báo giá " + listBaoGia[i].Name + "\"><img src = \"/Content/Display/Icon/bao-gia-loi-loc-nuoc.jpg\" alt=\"Báo giá " + listBaoGia[i].Name + "\" /></a>");
            //        resultBaoGia.Append("<a href=\"/bao-gia/" + listBaoGia[i].Tag + "\" title=\"Báo giá " + listBaoGia[i].Name + "\">" + listBaoGia[i].Name + "</a>");
            //        resultBaoGia.Append("</div>");
            //    }
            //    ViewBag.resultBaoGia = resultBaoGia.ToString();
            //    ViewBag.Title = "<title>Bảng báo giá lõi lọc nước Kangaroo, Karofi, Geyser... mới nhất năm " + DateTime.Now.Year + "</title>";
            //    ViewBag.Description = "<meta name=\"description\" content=\"Bảng giá lõi lọc nước tổng hợp các thương hiệu nổi tiếng như Kangaroo, karofi,geyser... chính hãng cam kết sản phẩm lõi lọc nước giá rẻ nhất.Báo giá được câp nhật liên tục\"/>";
            //    ViewBag.Keyword = "<meta name=\"keywords\" content=\"Bảng Báo giá sản phẩm lõi lọc nước\" /> ";
            //    ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a><i></i> Báo giá lõi lọc nước";

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
        public ActionResult baoGiaList(string tag)
        {
            tblConfig tblconfig = db.tblConfigs.First();

            tblGroupProduct groupproduct = db.tblGroupProducts.FirstOrDefault(p => p.Tag == tag);
            int idmenu = int.Parse(groupproduct.id.ToString());
            List<string> Mang = new List<string>();
            Mang = Arrayid(idmenu);
            Mang.Add(idmenu.ToString());
            ViewBag.name = groupproduct.Name;
             string moth = "";
            int moths = int.Parse(DateTime.Now.Month.ToString());
            if (moths <= 3)
            {
                moth = "Tháng 1,2,3 ";
            }
            else if (moths > 3 && moths <= 6)
            {
                moth = "Tháng 4,5,6 ";
            }
            else if (moths > 6 && moths <= 9)
            {
                moth = "Tháng 7,8,9 ";
            }
            else if (moths >= 9 && moths <= 12)
            {
                moth = "Tháng 10,11,12 ";
            }
            ViewBag.Title = "<title>Bảng báo giá " + groupproduct.Name + " " + moth + "năm " + DateTime.Now.Year + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\" Bảng báo giá  " + groupproduct.Name + " chính hãng Viglacera. Báo giá mới nhất sản phẩm "+groupproduct.Name+" mới nhất được cập nhật ngày "+DateTime.Now.Day+" tháng "+DateTime.Now.Month+" năm "+DateTime.Now.Year+".\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Bảng Báo giá sản phẩm " + groupproduct.Name + "\" /> ";
            string chuoi = "";
            var listproduct = db.tblProducts.Where(p => p.Active == true &&Mang.Contains(p.idCate.ToString())).OrderBy(p => p.idCate).ToList();
            for (int i = 0; i < listproduct.Count; i++)
            {

                chuoi += "<tr>";
                chuoi += "<td class=\"Ords\">" + (i + 1) + "</td>";
                chuoi += "<td class=\"Names\">";
                chuoi += "<a href=\"/" + listproduct[i].Tag + ".htm\" title=\"" + listproduct[i].Name + "\">" + listproduct[i].Name + "</a></span>";
                 chuoi += " </td>";
                chuoi += "<td class=\"Codes\"> " + listproduct[i].Code + " </td>";
                chuoi += "<td class=\"Prices\">" + string.Format("{0:#,#}", listproduct[i].PriceSale) + "đ</td>";
                chuoi += "<td class=\"Qualitys\">01</td>";
                chuoi += "<td class=\"SumPrices\">" + string.Format("{0:#,#}", listproduct[i].PriceSale) + "đ</td>";
                chuoi += "<td class=\"Images\"><a href=\"/" + listproduct[i].Tag + ".htm\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" alt=\"" + listproduct[i].Name + "\" title=\"" + listproduct[i].Name + "\"/></a<</td>";
                chuoi += "</tr>";
            }
            ViewBag.chuoi = chuoi;
            ViewBag.nUrl = " <ol itemscope itemtype=\"http://schema.org/BreadcrumbList\" >  <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"><a itemprop=\"item\" href=\"/\"> <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> › </ol><h2>Bảng báo giá " + groupproduct.Name + "</h2>";

            return View(tblconfig);
        }
    }
}