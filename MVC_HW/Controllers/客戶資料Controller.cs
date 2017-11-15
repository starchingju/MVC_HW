using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_HW.Models;
using ClosedXML.Excel;
using System.IO;

namespace MVC_HW.Controllers
{
    public class 客戶資料Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index(string keyword, string SearchId, string sort_name, string sort)
        {
            var data = db.客戶資料.Where(o => o.是否已刪除 == false);

            #region 搜尋
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Search = keyword;
                data = data.Where(o => o.客戶名稱.Contains(keyword));
            }
            #endregion

            #region 篩選資料
            if (!string.IsNullOrEmpty(SearchId))
            {
                data = data.Where(o => o.客戶分類 == SearchId);
            }
            #endregion

            # region 排序資料
            if (!string.IsNullOrEmpty(sort_name))
            {
                if (!string.IsNullOrEmpty(sort))
                {
                    ViewBag.Sort = string.Empty;

                    if (sort_name == "客戶名稱")
                        data = data.OrderByDescending(o => o.客戶名稱);
                    else if (sort_name == "統一編號")
                        data = data.OrderByDescending(o => o.統一編號);
                    else if (sort_name == "電話")
                        data = data.OrderByDescending(o => o.電話);
                    else if (sort_name == "傳真")
                        data = data.OrderByDescending(o => o.傳真);
                    else if (sort_name == "地址")
                        data = data.OrderByDescending(o => o.地址);
                    else if (sort_name == "Email")
                        data = data.OrderByDescending(o => o.Email);
                }
                else
                {
                    ViewBag.Sort = "desc";
                    //var temp = data.AsQueryable();
                    //var propertyInfo = typeof(客戶資料).GetProperty(keyword);
                    //temp = temp.OrderBy(x => propertyInfo.GetValue(x, null));

                    if (keyword == "客戶名稱")
                        data = data.OrderBy(o => o.客戶名稱);
                    else if (keyword == "統一編號")
                        data = data.OrderBy(o => o.統一編號);
                    else if (keyword == "電話")
                        data = data.OrderBy(o => o.電話);
                    else if (keyword == "傳真")
                        data = data.OrderBy(o => o.傳真);
                    else if (keyword == "地址")
                        data = data.OrderBy(o => o.地址);
                    else if (keyword == "Email")
                        data = data.OrderBy(o => o.Email);
                }
            }
            #endregion

            return View(data.ToList());
        }

        //匯出EXCEL
        public ActionResult FileExcel()
        {

            var data = db.客戶資料.Where(o => o.是否已刪除 == false).ToList();

            var MS = new MemoryStream();
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("客戶資料",1);

            ws.Cell(1, 1).Value = "客戶名稱";
            ws.Cell(1, 2).Value = "統一編號";
            ws.Cell(1, 3).Value = "電話";
            ws.Cell(1, 4).Value = "傳真";
            ws.Cell(1, 5).Value = "地址";
            ws.Cell(1, 6).Value = "Email";
            ws.Cell(1, 7).Value = "客戶分類";

            for (int i = 0; i < data.Count; i++)
            {
                ws.Cell(i + 2, 1).Value = data[i].客戶名稱;
                ws.Cell(i + 2, 2).Value = data[i].統一編號;
                ws.Cell(i + 2, 3).Value = data[i].電話;
                ws.Cell(i + 2, 4).Value = data[i].傳真;
                ws.Cell(i + 2, 5).Value = data[i].地址;
                ws.Cell(i + 2, 6).Value = data[i].Email;
                ws.Cell(i + 2, 7).Value = data[i].客戶分類;
            }
            
            //ws.Cell(1, 1).InsertData(data);

            //ws.Protect("LetMeEdit");
            wb.SaveAs(MS);
            MS.Seek(0, SeekOrigin.Begin);

            //if (data.Count() > 0)
            //{
            //return File("~/Image/客戶資料/客戶資料.xls", "application/vnd.ms-excel");
            return File(MS.ToArray(), "application/vnd.ms-excel", "客戶資料.xls");

            //}            

            //ws.Cell(1, 1).InsertData(客戶資料);
            //wb.SaveAs(memoryStream);

            //memoryStream.Seek(0, SeekOrigin.Begin);

            //return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "客戶資料.xlsx");


        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new List<SelectListItem>() {
                new SelectListItem(){Value="傳說中的海賊",Text="傳說中的海賊"},
                new SelectListItem(){Value="四皇",Text="四皇"},
                new SelectListItem(){Value="草帽大船團",Text="草帽大船團"}
            };
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.客戶資料.Add(客戶資料);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶分類 = new List<SelectListItem>() {
                new SelectListItem(){Value="傳說中的海賊",Text="傳說中的海賊"},
                new SelectListItem(){Value="四皇",Text="四皇"},
                new SelectListItem(){Value="草帽大船團",Text="草帽大船團"}
            };
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            //db.客戶資料.Remove(客戶資料);
            客戶資料.是否已刪除 = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult 客戶資料清單()
        {
            var db = new 客戶資料Entities();

            var result = from data in db.客戶資料
                         where data.是否已刪除 == false
                         select new 客戶資料清單VM()
                         {
                             Id = data.Id,
                             客戶名稱 = data.客戶名稱,
                             聯絡人數量 = data.客戶聯絡人.Where(o => o.是否已刪除 == false).Count(),
                             銀行帳戶數量 = data.客戶銀行資訊.Where(o => o.是否已刪除 == false).Count()
                         };

            return View(result);
        }

    }
}
