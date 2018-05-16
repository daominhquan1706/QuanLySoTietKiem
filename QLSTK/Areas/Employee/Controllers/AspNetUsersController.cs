using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using QLSTK.Models;

namespace QLSTK.Areas.Employee.Controllers
{
    [Authorize(Roles = "employee")]
    public class AspNetUsersController : Controller
    {
        private Entities db = new Entities();

        // GET: Employee/AspNetUsers
        public ActionResult Index()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.LoaiTietKiem).Include(a => a.LoaiTrangThai);
            return View(aspNetUsers.ToList());
        }

        // GET: Employee/AspNetUsers/Details/5
        public ActionResult Details(string id)
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
            return View(aspNetUser);
        }

        // GET: Employee/AspNetUsers/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai");
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai");
            return View();
        }

        // POST: Employee/AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Avatar,HoTen,MaTrangThai,MaLoaiTK,SoTienGuiBanDau,NgayMoSo,SoDu,DiaChi,CMND")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }

        // GET: Employee/AspNetUsers/Edit/5
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
            ViewData["id"] = id;
            ViewBag.MaLoaiTK = new SelectList(db.LoaiTietKiems, "MaLoaiTK", "TenLoai", aspNetUser.MaLoaiTK);
            ViewBag.MaTrangThai = new SelectList(db.LoaiTrangThais, "MaTrangThai", "TenTrangThai", aspNetUser.MaTrangThai);
            return View(aspNetUser);
        }

        // POST: Employee/AspNetUsers/Edit/5
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

        // GET: Employee/AspNetUsers/Delete/5
        public ActionResult Delete(string id)
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
            return View(aspNetUser);
        }

        // POST: Employee/AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
        public ActionResult ruttien(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["username"] = username;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ruttien([Bind(Include = "MaPhieuRut,MaSTK,SoTienRut,NgayRut,TienLai")] PhieuRutTien phieuRutTien)
        {
            if (ModelState.IsValid)
            {
                var listuser = db.AspNetUsers.ToList();
                var listloaitk = db.LoaiTietKiems.ToList();
                var userRutTien = listuser.Single(z => z.Id.Equals(phieuRutTien.MaSTK));
                int sothangketungaymoso = int.Parse(userRutTien.NgayMoSo.ToString().Split('/')[0].ToString())-DateTime.Now.Month;
                if (sothangketungaymoso < 0) { sothangketungaymoso = sothangketungaymoso * (-1); }
                string tenloaitk = listloaitk.Single(o => o.MaLoaiTK.Equals(userRutTien.MaLoaiTK)).TenLoai;
                int sothangtk = listloaitk.Single(o => o.MaLoaiTK.Equals(userRutTien.MaLoaiTK)).SoThang;
                double laisuat = 0;
                if (tenloaitk== "không kỳ hạn" )
                {
                    laisuat = 0.0015 * sothangketungaymoso;
                    if(phieuRutTien.SoTienRut== int.Parse(userRutTien.SoDu.ToString()))
                    {
                        userRutTien.MaTrangThai = 1; // Đóng sổ nếu rút sạch
                    }
                }
                else if (tenloaitk == "3 tháng")
                {
                    phieuRutTien.SoTienRut = int.Parse(userRutTien.SoDu.ToString());
                    userRutTien.MaTrangThai = 1; // Đóng sổ nếu rút sạch
                    laisuat = 0.005 * sothangketungaymoso;
                }
                else if (tenloaitk == "6 tháng")
                {
                    phieuRutTien.SoTienRut = int.Parse(userRutTien.SoDu.ToString());
                    userRutTien.MaTrangThai = 1; // Đóng sổ nếu rút sạch
                    laisuat = 0.0055 * sothangketungaymoso;
                }
                else { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                double tienlai = (double)phieuRutTien.SoTienRut * laisuat;
                phieuRutTien.TienLai = int.Parse(tienlai.ToString());

            
                    //userRutTien.SoDu = userRutTien.SoDu - phieuRutTien.SoTienRut;
                    db.Entry(userRutTien).State = EntityState.Modified;
                    db.SaveChanges();

                    db.PhieuRutTiens.Add(phieuRutTien);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }

            ViewData["username"] = db.AspNetUsers.Single(a=>a.Id.Equals(phieuRutTien.MaSTK));
            return View();
        }
        public ActionResult guitien(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["username"] = username;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult guitien([Bind(Include = "MaPhieuGui,MaSTK,NgayGui,SoTienGui")] PhieuGuiTien phieuGuiTien)
        {
            AspNetUser userGuiTien = db.AspNetUsers.Single(d => d.Id.Equals(phieuGuiTien.MaSTK));
            if (ModelState.IsValid)
            {
                //userGuiTien.SoDu = userGuiTien.SoDu + phieuGuiTien.SoTienGui;
                //db.Entry(userGuiTien).State = EntityState.Modified;
                db.SaveChanges();

                db.PhieuGuiTiens.Add(phieuGuiTien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuGuiTien.MaSTK);
            return View(phieuGuiTien);
        }
        

    }
}
