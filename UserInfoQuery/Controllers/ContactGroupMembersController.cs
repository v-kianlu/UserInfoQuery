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
using YourProjectName.Models;

namespace UserInfoQuery.Controllers
{
    public class ContactGroupMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContactGroupMembers
        public async Task<ActionResult> Index()
        {
            return View(await db.ContactGroupMembers.ToListAsync());
        }

        // GET: ContactGroupMembers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactGroupMember contactGroupMember = await db.ContactGroupMembers.FindAsync(id);
            if (contactGroupMember == null)
            {
                return HttpNotFound();
            }
            return View(contactGroupMember);
        }

        // GET: ContactGroupMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactGroupMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GroupAliasId,MemberAliasId")] ContactGroupMember contactGroupMember)
        {
            if (ModelState.IsValid)
            {
                db.ContactGroupMembers.Add(contactGroupMember);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contactGroupMember);
        }

        // GET: ContactGroupMembers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactGroupMember contactGroupMember = await db.ContactGroupMembers.FindAsync(id);
            if (contactGroupMember == null)
            {
                return HttpNotFound();
            }
            return View(contactGroupMember);
        }

        // POST: ContactGroupMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GroupAliasId,MemberAliasId")] ContactGroupMember contactGroupMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactGroupMember).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contactGroupMember);
        }

        // GET: ContactGroupMembers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactGroupMember contactGroupMember = await db.ContactGroupMembers.FindAsync(id);
            if (contactGroupMember == null)
            {
                return HttpNotFound();
            }
            return View(contactGroupMember);
        }

        // POST: ContactGroupMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ContactGroupMember contactGroupMember = await db.ContactGroupMembers.FindAsync(id);
            db.ContactGroupMembers.Remove(contactGroupMember);
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
            ad.GetContactGroupMembers();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [ActionName("Clear")]
        public async Task<ActionResult> ClearTable()
        {
            db.ContactGroupMembers.RemoveRange(db.ContactGroupMembers);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
