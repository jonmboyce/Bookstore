using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC3.DATA.EF;
using MVC3.UI.MVC.Models;

namespace MVC3.UI.MVC.Controllers
{
    public class BooksController : Controller
    {
        private BookStorePlusEntities db = new BookStorePlusEntities();

        #region Add to Cart - Shopping Cart Functionality

        [HttpPost]
        public ActionResult AddToCart(int qty, int bookId)
        {
            //CREATE **** AN EMPTY SHOPPING CART
            //CREATE A DICTIONARY IS A TYPED COLLECTION SIMILAR TO A LIST THAT STORES INFORMATION IN KEY VALUE PAIRS
            //THESE MUST BE UNIQUE TO THE COLLECTION
            //SYNTAX **    Dictionary<Key, value>
            //We will use the int as the key and the ShoppingCartViewModel
            Dictionary<int, ShoppingCartViewModel> shoppingCart = null;

            //Check the cart in Session (global)
            //if the session has anything in it, then assign its values to our local version
            if (Session["cart"] != null)
            {
                
                shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];
            }
            else
            {
                shoppingCart = new Dictionary<int, ShoppingCartViewModel>();
            }

            //get the product being added
            Book product = db.Books.Where(x => x.BookID == bookId).FirstOrDefault();
            //If not valid, return them to the books index
            if (product == null)
            {
                return RedirectToAction("Index");
                //Custom error page for invalid products could be added here
            }
            else
            {
                //Book is valid(product id was found and returned a book)
                //Create a shoppingCareViewModel Object
                ShoppingCartViewModel item = new ShoppingCartViewModel(qty, product);

                //Check if it is valid and one already exists in the cart, update the quantity in the local
                //shoppingCart is the dictionary from above, seeing if it already has the item, then we'll update
                if (shoppingCart.ContainsKey(product.BookID))
                {
                    shoppingCart[product.BookID].Qty += qty;
                }
                else //If it is not in the cart, add it locally
                {
                    shoppingCart.Add(product.BookID, item);
                }

                Session["cart"] = shoppingCart;
            }
            //send the shoppping cart to view products that have been added
            return RedirectToAction("Index", "ShoppingCart");

        }



        #endregion

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.BookRoyalty).Include(b => b.BookStatus).Include(b => b.Genre).Include(b => b.Publisher);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.BookRoyalties, "BookID", "BookID");
            ViewBag.BookStatusID = new SelectList(db.BookStatuses, "BookStatusID", "BookStatusName");
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,ISBN,BookTitle,Description,GenreID,Price,UnitsSold,PublishDate,PublisherID,BookImage,IsSiteFeature,IsGenreFeature,BookStatusID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.BookRoyalties, "BookID", "BookID", book.BookID);
            ViewBag.BookStatusID = new SelectList(db.BookStatuses, "BookStatusID", "BookStatusName", book.BookStatusID);
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName", book.GenreID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.BookRoyalties, "BookID", "BookID", book.BookID);
            ViewBag.BookStatusID = new SelectList(db.BookStatuses, "BookStatusID", "BookStatusName", book.BookStatusID);
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName", book.GenreID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,ISBN,BookTitle,Description,GenreID,Price,UnitsSold,PublishDate,PublisherID,BookImage,IsSiteFeature,IsGenreFeature,BookStatusID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.BookRoyalties, "BookID", "BookID", book.BookID);
            ViewBag.BookStatusID = new SelectList(db.BookStatuses, "BookStatusID", "BookStatusName", book.BookStatusID);
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName", book.GenreID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
