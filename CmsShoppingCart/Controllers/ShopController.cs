using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Areas.Admin.Models;
using CmsShoppingCart.Areas.Admin.Models.Data;
using CmsShoppingCart.Areas.Admin.Models.ViewModels.Shop;

namespace CmsShoppingCart.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }
        public ActionResult CategoryMenuPartial()
        {
            //Declare list of CategoryVM
            List<CategoryVM> categoryVMList;

            //Init the list
            using (Db db = new Db())
            {
                categoryVMList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVM(x))
                    .ToList();
            }
            //Return partial with list
            return PartialView(categoryVMList);
        }

        public ActionResult Category(string name)
        {
            //Declare a list of ProductVM
            List<ProductVM> productVMList;
            
            using (Db db = new Db())
            {
                //Get category ID
                CategoryDTO categoryDTO = db.Categories
                    .Where(x => x.Slug == name)
                    .FirstOrDefault();
                int catId = categoryDTO.Id;

                //Init the list
                productVMList = db.Products
                    .ToArray()
                    .Where(x => x.CategoryId == catId)
                    .Select(x => new ProductVM(x))
                    .ToList();
                //Get category name
                var productCat = db.Products
                    .Where(x => x.CategoryId == catId)
                    .FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;
            }
            //return view with list
            return View(productVMList);
        }
    }
}