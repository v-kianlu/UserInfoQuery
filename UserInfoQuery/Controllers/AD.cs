using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using UserInfoQuery.Models;
using YourProject.Models;
using YourProjectName.Models;

namespace UserInfoQuery.Controllers
{
    public class AD
    {
        private readonly string DOMAIN = "redmond.corp.microsoft.com";
        private readonly string GROUPSAM = "gststeam";

        private string name = "";
        private ApplicationDbContext db;

        public AD(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void GetContactGroupMembers()
        {
            List<UserPrincipal> group = new List<UserPrincipal>();

            try
            {
                PrincipalContext ad = new PrincipalContext(ContextType.Domain, DOMAIN);
                GroupPrincipal g = GroupPrincipal.FindByIdentity(ad, IdentityType.SamAccountName, GROUPSAM);
                PrincipalSearchResult<Principal> groupMembers = g.GetMembers();

                foreach (UserPrincipal user in groupMembers)
                {
                    AddContactGroupMember(user);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ", e);
            }

        }
        public void GetCurrentADUserContactsFromAD()
        {
            List<UserPrincipal> group = new List<UserPrincipal>();

            try
            {
                PrincipalContext ad = new PrincipalContext(ContextType.Domain, DOMAIN);
                GroupPrincipal g = GroupPrincipal.FindByIdentity(ad, IdentityType.SamAccountName, GROUPSAM);
                PrincipalSearchResult<Principal> groupMembers = g.GetMembers();
                
                foreach (UserPrincipal user in groupMembers)
                {
                    AddCurrentADUserContacts(user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ", e);
            }

        }

        private void AddCurrentADUserContacts(UserPrincipal userP)
        {
            string path = "LDAP://" + userP.DistinguishedName;



            CurrentADUserContact person = new CurrentADUserContact();

            DirectoryEntry de = new DirectoryEntry(DOMAIN);
            de.Path = path;
            de.AuthenticationType = AuthenticationTypes.Secure;
            DirectorySearcher search = new DirectorySearcher(de);
            SearchResult userD = search.FindOne();

            name = userP.SamAccountName;

            person.CreatedOn = DateTime.UtcNow;

            //gets AD info from the Principal way
            person.DisplayName = userP.DisplayName;
            person.AliasId = userP.SamAccountName;
            person.BusinessEmail = userP.EmailAddress;
            person.DistinguishedName = userP.DistinguishedName;

            //gets AD info from the DirectoryEntry way
            if (userD.Properties["telephonenumber"].Count == 1)
                person.BusinessPhone1 = userD.Properties["telephonenumber"][0].ToString();
            else if (userD.Properties["telephonnumber"].Count == 2)
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

            db.CurrentADUserContacts.Add(person);
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