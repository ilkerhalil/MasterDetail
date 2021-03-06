﻿using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MasterDetail.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string AddressBlock
        {
            get
            {
                var addressBlock = $"{Address}<br/>{City}, {State} {ZipCode}".Trim();
                return addressBlock == "<br/>," ? string.Empty : Address;
            }
        }

        public List<WorkOrder> WorkOrders { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

    }
}