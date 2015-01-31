using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TennisNetwork.Data;
using TennisNetwork.Models;

namespace TennisNetwork.Controllers
{
    public class LevelsAdminController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserLevelsAdmin
        public async Task<ActionResult> Index()
        {
            return View(await db.UserLevels.ToListAsync());
        }

        // GET: UserLevelsAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLevel userLevel = await db.UserLevels.FindAsync(id);
            if (userLevel == null)
            {
                return HttpNotFound();
            }
            return View(userLevel);
        }

        // GET: UserLevelsAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserLevelsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Level,Description")] UserLevel userLevel)
        {
            if (ModelState.IsValid)
            {
                db.UserLevels.Add(userLevel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userLevel);
        }

        // GET: UserLevelsAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLevel userLevel = await db.UserLevels.FindAsync(id);
            if (userLevel == null)
            {
                return HttpNotFound();
            }
            return View(userLevel);
        }

        // POST: UserLevelsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Level,Description")] UserLevel userLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userLevel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userLevel);
        }

        // GET: UserLevelsAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLevel userLevel = await db.UserLevels.FindAsync(id);
            if (userLevel == null)
            {
                return HttpNotFound();
            }
            return View(userLevel);
        }

        // POST: UserLevelsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserLevel userLevel = await db.UserLevels.FindAsync(id);
            db.UserLevels.Remove(userLevel);
            await db.SaveChangesAsync();
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
