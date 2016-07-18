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
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using YourProjectName.Models;

namespace UserInfoQuery.Controllers
{
    public class CurrentADUserContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CurrentADUserContacts
        public async Task<ActionResult> Index()
        {
            GetCurrentADUserContactsFromAD();
            db.SaveChanges();
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
        public async Task<ActionResult> Create([Bind(Include = "AliasId,DisplayName,DistinguishedName,Title,City,State,ZipCode,CountryOrRegion,Company,Location,Building,BusinessEmail,BusinessPhone1,BusinessPhone2,HomePhone1,HomePhone2,MobilePhone,Department,StreetAddress,CreatedOn,RowState")] CurrentADUserContact currentADUserContact)
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
        public async Task<ActionResult> Edit([Bind(Include = "AliasId,DisplayName,DistinguishedName,Title,City,State,ZipCode,CountryOrRegion,Company,Location,Building,BusinessEmail,BusinessPhone1,BusinessPhone2,HomePhone1,HomePhone2,MobilePhone,Department,StreetAddress,CreatedOn,RowState")] CurrentADUserContact currentADUserContact)
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


        private void GetCurrentADUserContactsFromAD()
        {
            List<UserPrincipal> group = new List<UserPrincipal>();
            string domain = "redmond.corp.microsoft.com";
            string groupSam = "gststeam";
            
            try
            {
                PrincipalContext ad = new PrincipalContext(ContextType.Domain, domain);
                GroupPrincipal g = GroupPrincipal.FindByIdentity(ad, IdentityType.SamAccountName, groupSam);
                PrincipalSearchResult<Principal> groupMembers = g.GetMembers();
                
                foreach(UserPrincipal user in groupMembers)
                {
                    AddCurrentADUserContacts(user);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ", e);
            }

        }

        //gets CurrentADUserContact's info and
        //adds to CurrentADUserContact to the CurrentADUserContacts table
        private void AddCurrentADUserContacts(UserPrincipal userP)
        {
            string domain = "redmond.corp.microsoft.com";
            string path = "LDAP://" + userP.DistinguishedName;

            CurrentADUserContact person = new CurrentADUserContact();

            DirectoryEntry de = new DirectoryEntry(domain);
            de.Path = path;
            de.AuthenticationType = AuthenticationTypes.Secure;
            DirectorySearcher search = new DirectorySearcher(de);
            SearchResult userD = search.FindOne();

            person.CreatedOn = DateTime.UtcNow;

            //gets AD info from the Principal way
            person.DisplayName = userP.DisplayName;
            person.AliasId = userP.SamAccountName;
            person.BusinessEmail = userP.EmailAddress;
            person.DistinguishedName = userP.DistinguishedName;

            //gets AD info from the DirectoryEntry way
            if (userD.Properties["telephonenumber"].Count == 1)
                person.BusinessPhone1 = userD.Properties["telephonenumber"][0].ToString();
            else
            {
                person.BusinessPhone2 = userD.Properties["telephonenumber"][1].ToString();
                person.BusinessPhone1 = userD.Properties["telephonenumber"][0].ToString();
            }
            if (userD.Properties["company"].Count == 1)
                person.Company = userD.Properties["company"][0].ToString();
            if (userD.Properties["department"].Count == 1)
                person.Department = userD.Properties["department"][0].ToString();
            if (userD.Properties["physicaldeliveryofficename"].Count == 1)
                person.Building = userD.Properties["physicaldeliveryofficename"][0].ToString();
            if (userD.Properties["mobile"].Count == 1)
                person.MobilePhone = userD.Properties["mobile"][0].ToString();

            //Missing table fields from AD
            //person.HomePhone1
            //person.HomePhone2
            //person.Location
            //person.CountryOrRegion
            //person.State
            //person.StreetAddress
            //person.City
            //person.Title;
            //person.ZipCode

            db.CurrentADUserContacts.Add(person);
            AddContactGroupMember(userP);
            
        }
        
        //Gets ContactGroupMember info and
        //adds ContactGroupMember to the ContactGroupMembers Table
        private void AddContactGroupMember(UserPrincipal userP)
        {
            foreach (GroupPrincipal userGroup in userP.GetGroups())
            {
                ContactGroupMember group = new ContactGroupMember();
                group.MemberAliasId = userP.SamAccountName;
                group.GroupAliasId = userGroup.SamAccountName;

                db.ContactGroupMembers.Add(group);
            }

        }

  
    }
}
