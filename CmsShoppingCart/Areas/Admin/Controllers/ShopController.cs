using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Areas.Admin.Models;
using CmsShoppingCart.Areas.Admin.Models.Data;
using CmsShoppingCart.Areas.Admin.Models.ViewModels.Shop;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //Declare a list of models
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                //Init the list
                categoryVMList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select((x => new CategoryVM(x)))
                    .ToList();

            }
            //Rreturn view with list
            return View(categoryVMList);
        }
        //Post: Admin/Shop/AddNewCategory
        [HttpPost]
         public string AddNewCategory(string catName)
         {
            //Declare id
            string  id;

            using (Db db = new Db())
            {

                //Check that the category name is unique
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";

                //Init Dto
                CategoryDTO dto = new CategoryDTO();

                //Add to Dto
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                //Save Dto
                db.Categories.Add(dto);
                db.SaveChanges();

                //get the id
                id = dto.Id.ToString();
            }

            //Return the id
            return id;
         }

         //Post: Admin/Shop/ReorderCategories
         [HttpPost]
         public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                //Set initial count
                int count = 1;

                //Declare PageDto
                CategoryDTO dto;

                //Set sorting for each category
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;

                }
            }
        }
         //Get:Admin/Shop/DeleteCategory/id
         public ActionResult DeleteCategory(int id)
         {
             using (Db db = new Db())
             {
                 //Get the Category
                 CategoryDTO dto = db.Categories.Find(id);

                 //remove the Category
                 db.Categories.Remove(dto);

                 //Save
                 db.SaveChanges();



             }
             //redirect
             return RedirectToAction("Categories");
         }
         
         //POST: Admin/Shop/RenameCategory
         [HttpPost]
         public string RenameCategory(string newCatName, int id)
         {
             using (Db db = new Db())
             {
                 //check category name is unique
                 if (db.Categories.Any(x => x.Name == newCatName))
                     return "titletaken";
                 //Get dto
                 CategoryDTO dto = db.Categories.Find(id);
                 //edit dto
                 dto.Name = newCatName;
                 dto.Slug = newCatName.Replace(" ", "-").ToLower();
                 //save
                 db.SaveChanges();

             }
             //return
             return "ok";
         }
    }
}