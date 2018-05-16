using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLSTK.Models;

namespace QLSTK.Controllers
{
    [Authorize(Roles = "employee")]
    public class ChatBoxesController : Controller
    {
        private Entities db = new Entities();

        // GET: ChatBoxes
        public ActionResult Index()
        {
            var chatBoxes = db.ChatBoxes.Include(c => c.AspNetUser).Include(c => c.AspNetUser1);
            return View(chatBoxes.ToList());
        }

        // GET: ChatBoxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatBox chatBox = db.ChatBoxes.Find(id);
            if (chatBox == null)
            {
                return HttpNotFound();
            }
            return View(chatBox);
        }
        public ActionResult Chat()
        {
            return View();
        }
        // GET: ChatBoxes/Create
        public ActionResult Create()
        {
            ViewBag.UserID_Gui = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.UserID_Nhan = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ChatBoxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserID_Gui,TinNhan,ThoiGian,UserID_Nhan")] ChatBox chatBox)
        {
            if (ModelState.IsValid)
            {
                db.ChatBoxes.Add(chatBox);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID_Gui = new SelectList(db.AspNetUsers, "Id", "Email", chatBox.UserID_Gui);
            ViewBag.UserID_Nhan = new SelectList(db.AspNetUsers, "Id", "Email", chatBox.UserID_Nhan);
            return View(chatBox);
        }

        // GET: ChatBoxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatBox chatBox = db.ChatBoxes.Find(id);
            if (chatBox == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID_Gui = new SelectList(db.AspNetUsers, "Id", "Email", chatBox.UserID_Gui);
            ViewBag.UserID_Nhan = new SelectList(db.AspNetUsers, "Id", "Email", chatBox.UserID_Nhan);
            return View(chatBox);
        }

        // POST: ChatBoxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserID_Gui,TinNhan,ThoiGian,UserID_Nhan")] ChatBox chatBox)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatBox).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID_Gui = new SelectList(db.AspNetUsers, "Id", "Email", chatBox.UserID_Gui);
            ViewBag.UserID_Nhan = new SelectList(db.AspNetUsers, "Id", "Email", chatBox.UserID_Nhan);
            return View(chatBox);
        }

        // GET: ChatBoxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatBox chatBox = db.ChatBoxes.Find(id);
            if (chatBox == null)
            {
                return HttpNotFound();
            }
            return View(chatBox);
        }

        // POST: ChatBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChatBox chatBox = db.ChatBoxes.Find(id);
            db.ChatBoxes.Remove(chatBox);
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
        [ValidateAntiForgeryToken]
        public ActionResult tuvan([Bind(Include = "Id,UserID_Gui,TinNhan,ThoiGian,UserID_Nhan")] ChatBox chatBox)
        {
            if (ModelState.IsValid)
            {
                db.ChatBoxes.Add(chatBox);
                db.SaveChanges();
                return RedirectToAction("chonkhach", "ChatBoxes",new { nguoinhan =db.AspNetUsers.Single(g=>g.Id.Equals(chatBox.UserID_Nhan)).UserName });
            }
            return RedirectToAction("chonkhach", "ChatBoxes", new { nguoinhan = db.AspNetUsers.Single(g => g.Id.Equals(chatBox.UserID_Nhan)).UserName });
        }
        public ActionResult chonkhach(string nguoinhan)
        {
            if (nguoinhan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(db.AspNetUsers.Single(d=>d.UserName.Equals(nguoinhan)).Id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            ViewData["nguoinhan"] = nguoinhan;
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chonkhach([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Avatar,HoTen,MaTrangThai,MaLoaiTK,SoTienGuiBanDau,NgayMoSo,SoDu,DiaChi,CMND")] AspNetUser aspNetUser)
        {
            if(aspNetUser.HoTen != null && aspNetUser.MaLoaiTK !=null && aspNetUser.NgayMoSo != null && aspNetUser.SoTienGuiBanDau !=null && aspNetUser.SoDu != null && aspNetUser.CMND != null)
            {
                aspNetUser.MaTrangThai = 2;
            }
            else
            {
                aspNetUser.MaTrangThai = 1;
            }
            if (ModelState.IsValid)
            {
                aspNetUser.SoDu = aspNetUser.SoTienGuiBanDau;
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("chonkhach", new { nguoinhan = aspNetUser.Email});
            }
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }
    }
}
