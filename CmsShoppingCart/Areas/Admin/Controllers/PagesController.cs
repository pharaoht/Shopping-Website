using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Areas.Admin.Models;
using CmsShoppingCart.Areas.Admin.Models.Data;
using CmsShoppingCart.Areas.Admin.Models.ViewModels.Pages;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare List of PageVM
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                //Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();

            }
            //Return view with list

            return View(pagesList);
        }

        //Get:Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        //Post: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state (first thing you should do after submitting a form)
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Declare slug
                string slug;

                //Init pageDtO
                PageDTO dto = new PageDTO();

                //DTo title
                dto.Title = model.Title;

                //Check for and set slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists");
                    return View(model);
                }

                //Dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;


                //Save Dto
                db.Pages.Add(dto);
                db.SaveChanges();


            }
            //Set tempData message
            TempData["SM"] = "You have added a new page!";
            //Redirect
            return RedirectToAction("AddPage");


        }

        //Get:Admin/Pages/EditPage/Id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare pageVm
            PageVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);
                //Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }

                //Init pageVm
                model = new PageVM(dto);
            }

            //Return view with model
            return View(model);
        }

        //Get:Admin/Pages/EditPage/Id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Get page id
                int id = model.ID;

                //Init slug
                string slug = "Home";
                //Get the page
                PageDTO dto = db.Pages.Find(id);
                //DTO the title
                dto.Title = model.Title;
                //Check for slug and set it if need be
                if (model.Slug != "Home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //Make sure title and slug are unique
                if (db.Pages.Where(x => x.ID != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.ID != id).Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                //Save the DTO
                db.SaveChanges();
            }
            //Set TempData message
            TempData["SM"] = "You have edited the page!";
            //Redirect
            return RedirectToAction("EditPage");
        }

        //Get:Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //Declare PageVm
            PageVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist");
                }
                //Init PageVm
                model = new PageVM(dto);
            }

            //Return the view model
            return View(model);
        }

        //Get:Admin/Pages/DeletePageIid
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //remove the page
                db.Pages.Remove(dto);

                //save
                db.SaveChanges();



            }
            //redirect
            return RedirectToAction("Index");
        }

        //Post: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //Declare PageDto
                PageDTO dto;
                //Set sorting for each
                foreach (var pageid in id)
                {
                    dto = db.Pages.Find(pageid);
                    dto.Sorting = count;
                    db.SaveChanges();

                    count++;
                }
            }

        }

        //Get:Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Declare Model
            SidebarVM model;

            using (Db db = new Db())
            {
                //get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                //Init model
                model = new SidebarVM(dto);


            }
            //Return view with Model
            return View(model);
        }

        //Post: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                //Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //Dto the body
                dto.Body = model.Body;
                //Save
                db.SaveChanges();
            }
            //Set TempData MEssage
            TempData["SM"] = "You have edited the sidebar!";
            //Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}