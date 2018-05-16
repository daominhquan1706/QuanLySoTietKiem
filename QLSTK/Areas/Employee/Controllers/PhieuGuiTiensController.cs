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
    public class PhieuGuiTiensController : Controller
    {
        private Entities db = new Entities();

        // GET: Employee/PhieuGuiTiens
        public ActionResult Index()
        {
            var phieuGuiTiens = db.PhieuGuiTiens.Include(p => p.AspNetUser);
            return View(phieuGuiTiens.ToList());
        }

        // GET: Employee/PhieuGuiTiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuGuiTien phieuGuiTien = db.PhieuGuiTiens.Find(id);
            if (phieuGuiTien == null)
            {
                return HttpNotFound();
            }
            return View(phieuGuiTien);
        }

        // GET: Employee/PhieuGuiTiens/Create
        public ActionResult Create()
        {
            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Employee/PhieuGuiTiens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieuGui,MaSTK,NgayGui,SoTienGui")] PhieuGuiTien phieuGuiTien)
        {
            if (ModelState.IsValid)
            {
                db.PhieuGuiTiens.Add(phieuGuiTien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuGuiTien.MaSTK);
            return View(phieuGuiTien);
        }

        // GET: Employee/PhieuGuiTiens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuGuiTien phieuGuiTien = db.PhieuGuiTiens.Find(id);
            if (phieuGuiTien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuGuiTien.MaSTK);
            return View(phieuGuiTien);
        }

        // POST: Employee/PhieuGuiTiens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieuGui,MaSTK,NgayGui,SoTienGui")] PhieuGuiTien phieuGuiTien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuGuiTien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSTK = new SelectList(db.AspNetUsers, "Id", "Email", phieuGuiTien.MaSTK);
            return View(phieuGuiTien);
        }

        // GET: Employee/PhieuGuiTiens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuGuiTien phieuGuiTien = db.PhieuGuiTiens.Find(id);
            if (phieuGuiTien == null)
            {
                return HttpNotFound();
            }
            return View(phieuGuiTien);
        }

        // POST: Employee/PhieuGuiTiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuGuiTien phieuGuiTien = db.PhieuGuiTiens.Find(id);
            db.PhieuGuiTiens.Remove(phieuGuiTien);
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
