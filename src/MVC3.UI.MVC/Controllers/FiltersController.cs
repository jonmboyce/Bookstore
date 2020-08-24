using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC3.DATA.EF;//added for our entity framework domain models
using System.Data.Entity;//added for entity framework
using PagedList;//added by going to tools then searched for pagedlist and downloaded Paged.List.MVC and the other one to the ui layer

namespace MVC3.UI.MVC.Controllers
{
    public class FiltersController : Controller
    {
        private BookStorePlusEntities db = new BookStorePlusEntities(); //this lets us access the database information

        // GET: Filters
        public ActionResult Index()
        {
            return View();
        }

        //Server side filter action
        public ActionResult AuthorsQs(string searchFilter)
        {
            if (String.IsNullOrEmpty(searchFilter))
            {
                //if optional search isn't used, return all records
                return View(db.Authors.ToList());
            } 
            else
            {
                //if optional search is used, compare it to the first and last name. Using linq method syntax
                string searchUpCase = searchFilter.ToUpper();
                List<Author> searchResults = db.Authors
                                            .Where(a => a.FirstName.ToUpper().Contains(searchUpCase) ||
                                            a.LastName.ToUpper().Contains(searchUpCase))
                                            .OrderBy(a => a.FirstName)
                                            .ThenBy(a => a.LastName)
                                            .ToList();

                //Method sytax example above ^^^^^^ or Query syntax example below
                List<Author> searchResults2 = (from a in db.Authors
                                               where a.FirstName.ToUpper().Contains(searchUpCase) ||
                                               a.LastName.ToUpper().Contains(searchUpCase)
                                               orderby a.FirstName, a.LastName
                                               select a).ToList();
                                        

                return View(searchResults);
            }
           
        }

        public ActionResult LabMagazinesQS(string searchFilter)
        {
            //check if the string is null or empty and if it is, return them the whole list again
            if (String.IsNullOrEmpty(searchFilter)){
                return View(db.Magazines.ToList());
            }
            else
            {
                //string searchLowCase = searchFilter.ToLower();
                //List<Magazine> searchResults = (from m in db.Magazines
                //                                where m.Category.ToLower().Contains(searchLowCase)
                //                                select m).ToList();

                string searchUpCase = searchFilter.ToUpper();
                List<Magazine> searchResults = db.Magazines
                                            .Where(m => m.Category.ToUpper().Contains(searchUpCase))
                                            .OrderBy(m => m.MagazineTitle)
                                            .ToList();

                return View(searchResults);
            }

        }//end of magazine category search action

        public ActionResult BooksMVCPaging(string searchString, string currentFilter, int page = 1)
        {
            int pageSize = 5;
            var books = db.Books.OrderBy(b => b.BookTitle).ToList();

            #region search with paging
            //We are tracking if it's a new search(Go to page 1 with results)
            //or if it's a previous search (track with currentFilter and use paging based on the last request)
            /* 
             *In the Action-
             *  SearchString only gets assigned new searches
             *  If searchString has a value, then it is a new search - update the page to 1(1st page of results)
             *  
             *  Else - If searchString is null, assing searchString to use the current value (either null/empty or previously tracked old serach)
             */
            #endregion

            if (searchString != null) 
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Check if the searchString is not null/empty. If it is not, it has a value, use the filter to grab the new data set

            if (!String.IsNullOrEmpty(searchString)) 
            {
                books = (from b in books
                         where b.BookTitle.ToLower().Contains(searchString.ToLower())
                         orderby b.BookTitle
                         select b).ToList();

                //books = db.Books
                //    .Where(b => b.BookTitle.ToLower().Contains(searchString.ToLower()))
                //    .OrderBy(b => b.BookTitle)
                //    .ToList();
            }

            //set up a viewbag variable for passing CurrentFilter based on whatever searchString is now.
            ViewBag.CurrentFilter = searchString;

            return View(books.ToPagedList(page, pageSize));

        }

        public ActionResult LabMagazinesMVCPaging(string magazineSearch, string currentFilter, int page = 1)
        {
            int pageSize = 5;
            var magazines = db.Magazines.OrderBy(m => m.MagazineTitle).ToList();

            if (magazineSearch != null)
            {
                page = 1;
            } else
            {
                magazineSearch = currentFilter;
            } //so if the magazineSearch hasn't been done (loads up a second page from first search) it hits the if and it's page one
            //if there has been a value entered it's assigned to currentFilter

            if (!String.IsNullOrEmpty(magazineSearch))
            {
                magazines = (from m in magazines
                             where m.MagazineTitle.ToLower().Contains(magazineSearch.ToLower())
                             orderby m.MagazineTitle
                             select m).ToList();
            }
            ViewBag.CurrentFilter = magazineSearch;
            return View(magazines.ToPagedList(page,pageSize)); //i don't understand this line and where the parameters came from 
        }

    }
}