using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelerikSampleApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace TelerikSampleApp.Controllers
{
    public class GridController : Controller
    {
        //private readonly NorthWind2020Context _context;
        //public GridController(NorthWind2020Context context)
        //{
        //    _context = context;
        //}


        public static async Task<List<ContactItem>> GetContactsAsync()
        {
            return await Task.Run(async () =>
            {
                await using var context = new NorthWind2020Context();
                return await context.Contacts
                    .AsNoTracking()
                    .Include(contact => contact.ContactTypeIdentifierNavigation)
                    .Include(c => c.ContactDevices)
                    .ThenInclude(contactDevices => contactDevices.PhoneTypeIdentifierNavigation)
                    .Select(contact => new ContactItem
                    {
                        Id = contact.ContactId,
                        ContactName = contact.FirstName + " " + contact.LastName,
                        ContactType = contact.ContactTypeIdentifierNavigation.ContactTitle,
                        OfficePhone = contact.ContactDevices.FirstOrDefault(contactDevices => contactDevices.PhoneTypeIdentifier == 3).PhoneNumber ?? "(none)",
                        CellPhone = contact.ContactDevices.FirstOrDefault(contactDevices => contactDevices.PhoneTypeIdentifier == 2).PhoneNumber ?? "(none)",
                        HomePhone = contact.ContactDevices.FirstOrDefault(contactDevices => contactDevices.PhoneTypeIdentifier == 1).PhoneNumber ?? "(none)"
                    }).ToListAsync();
            });
        }

        public async Task<ActionResult> Contacts_Read([DataSourceRequest] DataSourceRequest request)
        {
            var dsResult = await GetContactsAsync();
            return Json(dsResult.ToDataSourceResult(request));
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            using var context = new NorthWind2020Context();
            return Json(context.Products.ToDataSourceResult(request));
        }

        public ActionResult Products_Create([DataSourceRequest] DataSourceRequest request, Product product)
        {
            using var context = new NorthWind2020Context();
            context.Products.Add(product);
            context.SaveChanges();

            return Json(new[] { product }.ToDataSourceResult(request));
        }

        public ActionResult Products_Destroy([DataSourceRequest] DataSourceRequest request, Product product)
        {
            using var context = new NorthWind2020Context();
            context.Products.Remove(product);
            context.SaveChanges();

            return Json(new[] { product }.ToDataSourceResult(request));
        }

        public ActionResult Products_Update([DataSourceRequest] DataSourceRequest request, Product product)
        {
            using var context = new NorthWind2020Context();
            context.Products.Update(product);
            context.SaveChanges();

            return Json(new[] { product }.ToDataSourceResult(request));
        }
    }
}
