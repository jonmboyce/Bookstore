﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC3.DATA.EF;

namespace MVC3.UI.MVC.Controllers
{
    public class PublishersController : Controller
    {
        private BookStorePlusEntities db = new BookStorePlusEntities();

        #region AJAX Delete
        [HttpPost]
        public JsonResult AjaxDelete(int id)
        {
            //retrieve the publisher from the db
            Publisher pub = db.Publishers.Find(id);

            //remove the publisher
            db.Publishers.Remove(pub);

            //save changes
            db.SaveChanges();

            //Create message to send back to the UI as a json result
            var message = $"Deleted Publisher {pub.PublisherName} from the database!";
            return Json(
                new
                {
                    id = id,
                    message = message
                });
        }
        #endregion

        #region AJAX Details

        [HttpGet]
        public PartialViewResult PublisherDetails(int id)
        {
            //Retrieve the publisher by its id
            Publisher pub = db.Publishers.Find(id);

            //Return a partial view to the browser with the publisher object
            return PartialView(pub);

            //set up this view
            //1)right click and add a partial view
            //2)scaffold details
            //3)select partial view



        }



        #endregion

        #region AJAX Create
        //Add Publisher to the database via AJAX and return results
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PublisherCreate(Publisher publisher)
        {
            db.Publishers.Add(publisher);
            db.SaveChanges();

            return Json(publisher);
        }

        #endregion

        #region AJAX Edit - GET(SHow the form) and POST (process the form)
        public PartialViewResult PublisherEdit(int id)
        {
            Publisher pub = db.Publishers.Find(id);
            return PartialView(pub);
        }
        //GET above post below
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Publisher publisher)
        {
            db.Entry(publisher).State = EntityState.Modified;
            db.SaveChanges();
            return Json(publisher);
        }




        #endregion


        // GET: Publishers
        public ActionResult Index()
        {
            return View(db.Publishers.ToList());
        }

        // GET: Publishers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // GET: Publishers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PublisherID,PublisherName,City,State,IsActive")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Publishers.Add(publisher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublisherID,PublisherName,City,State,IsActive")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            db.Publishers.Remove(publisher);
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
