using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC3.UI.MVC.Models;

namespace MVC3.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //create a local version o fthe shopping cart from the Session(global) version
            //If the value is null or the count is 0 create an empty instance and provide no cart items.
            var shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];

            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                //new empty instance of the local shopping cart to pass to the view
                //(strongly typed view MUST have an instance of our shopping cart in order to load)
                shoppingCart = new Dictionary<int, ShoppingCartViewModel>();
                ViewBag.Message = "There are no books in your cart";
            }
            else
            {
                ViewBag.Message = null;
            }

            return View(shoppingCart);
        }
        
        public ActionResult UpdateCart(int bookID, int qty)
        {
            //retrieve cart from session and assign it to our local dictionary
            Dictionary<int, ShoppingCartViewModel> shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];

            //update teh qty in the local storage
            shoppingCart[bookID].Qty = qty;

            //return the local cart to session
            Session["cart"] = shoppingCart;

            //logic to display a message if they update to No items in their cart
            if(shoppingCart.Count == 0)
            {
                ViewBag.Message = "There are no books in your cart.";
            }

            //return View("Index") - the code in the Index Action will not run - the care totals will not change
            //in fact, it may even cause an error because no shoppingCart (dictionary) was sent to the view

            //RedirectToAction (index) will make sure the code runs in that method and returns the view with updated data
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            //retrieve cart from session and assign it to our local dictionary same as above
            Dictionary<int, ShoppingCartViewModel> shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];

            //call the remove() from the dictionary class
            shoppingCart.Remove(id);

            //set up the viewbag message for no results
            if(shoppingCart.Count == 0)
            {
                ViewBag.Message = "There are no books in your cart";
                // set current global session to null
                Session["cart"] = null;
            }

            return RedirectToAction("Index");

        }
    }

}