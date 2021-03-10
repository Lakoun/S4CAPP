using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using S4CAPP.Data;
using S4CAPP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace S4CAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("LicenseList", "Home");
        }

        public async Task<IActionResult> LicenseList() {

             return View(await _context.Licenses.Include(p => p.Products).Include(s => s.Signature).ToListAsync());
        }

        public async Task<IActionResult> LicenseDetail(int? id)
        {
            if (id == null) {
                return RedirectToAction("LicenseList", "Home");
            }

            var license = _context.Licenses.Where(l => l.LicenseId == id).Include(p => p.Products).Include(s => s.Signature).SingleOrDefault();

            return View(license);
        }

        public IActionResult LicenseAdd()
        {

            return View();
        }

        [HttpPost]
        public  IActionResult LicenseAdd(string Name, string xmlData)
        {
            _context.Add<License>(GetLicense(Name, xmlData));
            _context.SaveChanges();      

            return RedirectToAction("LicenseList" , "Home");
        }

        public License GetLicense(string name, string xmlData)
        {
            XElement doc = XElement.Parse(xmlData);

            Signature s = new Signature { Salt = doc.Element("Signature").Element("Salt").Value, Hash = doc.Element("Signature").Element("Hash").Value };

            var pr = new List<Product>();

            switch (doc.FirstAttribute.Value.ToString())
            {
                case "1.0":
                    foreach (XElement e in doc.Descendants("Product"))
                    {
                        Product p = new Product { };
                        p.ProductIdentifier = e.FirstAttribute.Value;
                        p.ProductName = e.LastAttribute.Value;
                        pr.Add(p);
                    }
                    break;
                case "1.1":
                        foreach (XElement e in doc.Descendants("Product"))
                        {
                            Product p = new Product { };
                            p.ProductIdentifier = e.FirstAttribute.Value;
                            p.ProductName = e.FirstNode.ToString();
                            pr.Add(p);
                        }              
                    break;
                case "2.0":
                        foreach (XElement e in doc.Descendants("Product"))
                        {
                            Product p = new Product { };
                            p.ProductIdentifier = e.Element("id").Value;
                            p.ProductName = e.Element("name").Value;
                            pr.Add(p);
                        }                
                    break;
                default:
                    
                    break;
            }

            License l = new License { LicenseName = name, ProductCount = doc.Descendants("Product").Count(), Signature = s, Products = pr };

            return l;

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
