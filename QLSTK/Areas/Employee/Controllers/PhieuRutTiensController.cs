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
    public class PhieuRutTiensController : Controller
    {
        private Entities db = new Entities();

        // GET: Employee/PhieuRutTiens
        public ActionResult Index()
        {
            var phieuRutTiens = db.PhieuRutTiens.Include(p => p.AspNetUser);
            return View(phieuRutTiens.ToList());
        }

        // GET: Employee/PhieuRutTiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuRutTien phieuRutTien = db.PhieuRutTiens.Find(id);
            if (phieuRutTien == null)
            {
                return HttpNotFound();
            }
            return View(phieuRutTien);
        }

        // GET: Employee/PhieuRutTiens/Create
        public ActionResult Create()
        {
            ViewBag.MaSTK = new SelectList(db.AspNetUsers.Where(b=>b.SoDu >0 ), "Id", "Email");
            return View();
        }

        // POST: Employee/PhieuRutTiens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieuRut,MaSTK,SoTienRut,NgayRut,TienLai")] PhieuRutTien phieuRutTien)
        {

            var listuser = db.AspNetUsers.ToList();
            var listloaitk = db.LoaiTietKiems.ToList();
            var userRutTien = listuser.Single(z => z.Id.Equals(phieuRutTien.MaSTK));
            int maloaitk = int.Parse(userRutTien.MaLoaiTK.ToString());
            int laisuat = int.Parse(listloaitk.Single(z => z.MaLoaiTK.Equals(maloaitk)).LaiSuat.ToString());
            phieuRutTien.TienLai = int.Parse((userRutTien.SoDu * laisuat).ToString());
            if (ModelState.IsValid && userRutTien.SoDu > phieuRutTien.SoTienRut)
            {
                userRutTien.SoDu = userRutTien.SoDu - phieuRutTien.SoTienRut;
                db.Entry(userRutTien).State = EntityState.Modified;
                db.SaveChanges();

                db.PhieuRutTiens.Add(phieuRutTien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuRutTien.MaSTK);
            return View(phieuRutTien);
        }

        // GET: Employee/PhieuRutTiens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuRutTien phieuRutTien = db.PhieuRutTiens.Find(id);
            if (phieuRutTien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuRutTien.MaSTK);
            return View(phieuRutTien);
        }

        // POST: Employee/PhieuRutTiens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieuRut,MaSTK,SoTienRut,NgayRut")] PhieuRutTien phieuRutTien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuRutTien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuRutTien.MaSTK);
            return View(phieuRutTien);
        }

        // GET: Employee/PhieuRutTiens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuRutTien phieuRutTien = db.PhieuRutTiens.Find(id);
            if (phieuRutTien == null)
            {
                return HttpNotFound();
            }
            return View(phieuRutTien);
        }

        // POST: Employee/PhieuRutTiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuRutTien phieuRutTien = db.PhieuRutTiens.Find(id);
            db.PhieuRutTiens.Remove(phieuRutTien);
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
