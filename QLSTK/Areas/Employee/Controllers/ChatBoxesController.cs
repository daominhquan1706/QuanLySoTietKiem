using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLSTK.Models;

namespace QLSTK.Areas.Employee.Controllers
{
    [Authorize(Roles = "employee")]
    public class ChatBoxesController : Controller
    {
        private Entities db = new Entities();
        // GET: Employee/ChatBoxes
        public ActionResult Chat()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult tuvan([Bind(Include = "Id,UserID_Gui,TinNhan,ThoiGian,UserID_Nhan")] ChatBox chatBox)
        {
            if (ModelState.IsValid)
            {
                db.ChatBoxes.Add(chatBox);
                db.SaveChanges();
                return RedirectToAction("chonkhach", "ChatBoxes", new { nguoinhan = db.AspNetUsers.Single(g => g.Id.Equals(chatBox.UserID_Nhan)).UserName });
            }
            return RedirectToAction("chonkhach", "ChatBoxes", new { nguoinhan = db.AspNetUsers.Single(g => g.Id.Equals(chatBox.UserID_Nhan)).UserName });
        }
        public ActionResult chonkhach(string nguoinhan)
        {
            if (nguoinhan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(db.AspNetUsers.Single(d => d.UserName.Equals(nguoinhan)).Id);
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
            if (aspNetUser.HoTen != null && aspNetUser.MaLoaiTK != null  && aspNetUser.SoTienGuiBanDau != null && aspNetUser.SoDu != null && aspNetUser.CMND != null)
            {
                aspNetUser.MaTrangThai = 2;
                aspNetUser.NgayMoSo = DateTime.Now;
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
                return RedirectToAction("chonkhach", new { nguoinhan = aspNetUser.Email });
            }
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }
    }
}