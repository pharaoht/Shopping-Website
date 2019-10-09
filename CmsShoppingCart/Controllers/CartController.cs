using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using CmsShoppingCart.Areas.Admin.Models;
using CmsShoppingCart.Areas.Admin.Models.Data;
using CmsShoppingCart.Areas.Admin.Models.ViewModels.Cart;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //Init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //Check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }
            //Calculate total and save to viewbag
            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;
            //Return view with list
            return View(cart);
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

        public ActionResult AddToCartPartial(int id)
        {
            //Init CartVM
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //Init CartVM
            CartVM model = new CartVM();

            using (Db db = new Db())
            {

                //Get the product
                ProductDTO product = db.Products.Find(id);

                //Check if product is already in cart
                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

                //If not, add new
                if(productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.ImageName
                    });
                }
                else
                { 
                    //If it is, increment
                    productInCart.Quantity++;
                }
            } 
            //Get total qty and price and add to model
            int qty = 0;
            decimal price = 0m;

            foreach(var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;

            //Save cart back to session
            Session["cart"] = cart;

            //Return partial view with model
            return PartialView(model);


        }
    }
}