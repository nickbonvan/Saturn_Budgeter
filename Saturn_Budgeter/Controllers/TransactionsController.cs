﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Saturn_Budgeter.Models;
using Microsoft.AspNet.Identity;

namespace Saturn_Budgeter.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.Type).Include(t => t.User);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create/4
        //Bank account id
        public ActionResult Create(int id)
        {
            ViewBag.AccountId = id;
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Amount,Reconciled,AccountId,TransactionTypeId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.UserId = User.Identity.GetUserId();
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Details", "BankAccounts", new { id = transaction.AccountId });
            }

            return View(transaction);
        }

        // GET: Transactions/Void/5
        public ActionResult Void(int id)
        {
            Transaction model = db.Transactions.FirstOrDefault(transaction => transaction.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Void(int id, string voided)
        {
            Transaction transaction = db.Transactions.FirstOrDefault(t => t.Id == id);
            if (ModelState.IsValid)
            {
                if (voided == "true")
                {
                    transaction.Void = true;
                    db.Entry(transaction).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "BankAccounts", new { id = id });
                }
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.AccountId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Amount,Reconciled,AccountId,UserId,TransactionTypeId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "BankAccounts", new { id = transaction.AccountId });
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.AccountId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
