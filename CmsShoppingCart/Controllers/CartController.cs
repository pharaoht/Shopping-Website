using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Areas.Admin.Models.ViewModels.Cart;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CartPartial()
        {
            //Init CArtVm
            CartVM model = new CartVM();
            //Init quantity
            int qty = 0;
            //Init price
            decimal price = 0m;
            //Check for cart session
            if (Session["cart"] != null)
            {
                //Get total qty and price
                var list = (List<CartVM>) Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity* item.Price;
                }
            }
            else
            {  
                //Or set aty and price to 0
                model.Quantity = 0;
                model.Price = 0m;


            }
            //Return partial view with model 
            return PartialView(model);
        }
    }
}