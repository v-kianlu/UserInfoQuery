using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserInfoQuery.Models;
using YourProject.Models;

namespace UserInfoQuery.Controllers
{
    public class CurrentADUserContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CurrentADUserContacts
        public async Task<ActionResult> Index()
        {
            return View(await db.CurrentADUserContacts.ToListAsync());
        }

        // GET: CurrentADUserContacts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentADUserContact currentADUserContact = await db.CurrentADUserContacts.FindAsync(id);
            if (currentADUserContact == null)
            {
                return HttpNotFound();
            }
            return View(currentADUserContact);
        }

        // GET: CurrentADUserContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurrentADUserContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AliasId,DisplayName,DistinguishedName,Company,Building,BusinessEmail,BusinessPhone1,BusinessPhone2,MobilePhone,Department,CreatedOn")] CurrentADUserContact currentADUserContact)
        {
            if (ModelState.IsValid)
            {
                db.CurrentADUserContacts.Add(currentADUserContact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(currentADUserContact);
        }

        // GET: CurrentADUserContacts/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentADUserContact currentADUserContact = await db.CurrentADUserContacts.FindAsync(id);
            if (currentADUserContact == null)
            {
                return HttpNotFound();
            }
            return View(currentADUserContact);
        }

        // POST: CurrentADUserContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AliasId,DisplayName,DistinguishedName,Company,Building,BusinessEmail,BusinessPhone1,BusinessPhone2,MobilePhone,Department,CreatedOn")] CurrentADUserContact currentADUserContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currentADUserContact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(currentADUserContact);
        }

        // GET: CurrentADUserContacts/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentADUserContact currentADUserContact = await db.CurrentADUserContacts.FindAsync(id);
            if (currentADUserContact == null)
            {
                return HttpNotFound();
            }
            return View(currentADUserContact);
        }

        // POST: CurrentADUserContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            CurrentADUserContact currentADUserContact = await db.CurrentADUserContacts.FindAsync(id);
            db.CurrentADUserContacts.Remove(currentADUserContact);
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

        [ActionName("GetInfo")]
        public async Task<ActionResult> GetUserInfoFromAD()
        {
            AD ad = new AD(db);
            ad.GetCurrentADUserContactsFromAD();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [ActionName("Clear")]
        public async Task<ActionResult> ClearTable()
        {
            db.CurrentADUserContacts.RemoveRange(db.CurrentADUserContacts);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
