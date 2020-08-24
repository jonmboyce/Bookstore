using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVC3.DATA.EF;

namespace MVC3.UI.MVC.Models
{
    //adding this viewmodel to combine domain-related info and other info(qty) nneded for shopping cart.
    //Hence: ViewModel
    public class ShoppingCartViewModel
    {
        [Range(1, int.MaxValue)]//ensure the value is greater than zero and up to the max value of the datatype
        public int Qty { get; set; }
        public Book Product { get; set; }
        
        //Fully qualified ctor to make it easy to store all the info **needs to be same as the class
        public ShoppingCartViewModel(int qty, Book product)
        {
            Qty = qty;
            Product = product;
        }

    }
}