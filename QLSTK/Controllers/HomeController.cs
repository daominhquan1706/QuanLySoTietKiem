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
    [Authorize]
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chatadmin([Bind(Include = "Id,UserID_Gui,TinNhan,ThoiGian,UserID_Nhan")] ChatBox chatBox)
        {
            if (ModelState.IsValid)
            {
                db.ChatBoxes.Add(chatBox);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Avatar,HoTen,MaTrangThai,MaLoaiTK,SoTienGuiBanDau,NgayMoSo,SoDu,DiaChi,CMND")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }
        //thêm thông tin còn thiếu 
        public ActionResult themthongtinthieu(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["id"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult themthongtinthieu(string id,string hoten, string diachi,int cmnd)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(hoten != null && diachi != null)
            {
                var thongtinuser = db.AspNetUsers.Single(t => t.Id.Equals(id));
                thongtinuser.HoTen = hoten;
                thongtinuser.DiaChi = diachi;
                thongtinuser.CMND = cmnd;

                db.Entry(thongtinuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}