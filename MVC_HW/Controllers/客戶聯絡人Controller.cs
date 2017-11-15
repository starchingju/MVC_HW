using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_HW.Models;

namespace MVC_HW.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶聯絡人
        public ActionResult Index(string keyword1, string keyword2, string SearchId, string sort_name, string sort)
        {
            var data = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(o => o.是否已刪除 == false);

            #region 搜尋
            if (!string.IsNullOrEmpty(keyword1) || !string.IsNullOrEmpty(keyword2))
            {
                ViewBag.Search1 = keyword1;
                ViewBag.Search2 = keyword2;
                data = data.Where(o => (o.職稱.Contains(keyword1)) && (o.姓名.Contains(keyword2)));
            }
            #endregion

            #region 選資料
            if (SearchId != "" && SearchId != null)
            {
                data = data.Where(o => o.職稱.Contains(SearchId));
            }
            #endregion

            #region 排序資料
            if (!string.IsNullOrEmpty(sort_name))
            {
                if (!string.IsNullOrEmpty(sort))
                {
                    ViewBag.Sort = string.Empty;

                    if (sort_name == "職稱")
                        data = data.OrderByDescending(o => o.職稱);
                    else if (sort_name == "姓名")
                        data = data.OrderByDescending(o => o.姓名);
                    else if (sort_name == "Email")
                        data = data.OrderByDescending(o => o.Email);
                    else if (sort_name == "手機")
                        data = data.OrderByDescending(o => o.手機);
                    else if (sort_name == "電話")
                        data = data.OrderByDescending(o => o.電話);
                    else if (sort_name == "客戶名稱")
                        data = data.OrderByDescending(o => o.客戶資料.客戶名稱);
                }
                else
                {
                    ViewBag.Sort = "desc";

                    if (sort_name == "職稱")
                        data = data.OrderBy(o => o.職稱);
                    else if (sort_name == "姓名")
                        data = data.OrderBy(o => o.姓名);
                    else if (sort_name == "Email")
                        data = data.OrderBy(o => o.Email);
                    else if (sort_name == "手機")
                        data = data.OrderBy(o => o.手機);
                    else if (sort_name == "電話")
                        data = data.OrderBy(o => o.電話);
                    else if (sort_name == "客戶名稱")
                        data = data.OrderBy(o => o.客戶資料.客戶名稱);
                }
            }
            #endregion

            return View(data.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new List<SelectListItem>() {
                new SelectListItem(){Value="0",Text="xpy0928"},
                new SelectListItem(){Value="1",Text="cnblogs"}
            };


            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,客戶分類")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                //該段語法已轉移至Model內
                //if (db.客戶聯絡人.Any(o => o.客戶Id == 客戶聯絡人.客戶Id && o.Email == 客戶聯絡人.Email.Trim()))
                //{
                //    ModelState.AddModelError("Email", "同一客戶下的聯絡人，其Email不可重複！");
                //}
                //else
                //{
                db.客戶聯絡人.Add(客戶聯絡人);
                db.SaveChanges();
                return RedirectToAction("Index");
                //}
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                if (db.客戶聯絡人.Any(o => o.客戶Id == 客戶聯絡人.客戶Id && o.Id != 客戶聯絡人.Id && o.Email == 客戶聯絡人.Email.Trim()))
                {
                    ModelState.AddModelError("Email", "同一客戶下的聯絡人，其Email不可重複！");
                }
                else
                {
                    db.Entry(客戶聯絡人).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //db.客戶聯絡人.Remove(客戶聯絡人);
            客戶聯絡人.是否已刪除 = true;
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
    }
}
