using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Saturn_Budgeter.Models;

namespace Saturn_Budgeter.Controllers
{
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetItems
        public ActionResult Index()
        {
            var budgetItems = db.BudgetItems.Include(b => b.Budget).Include(b => b.Category).Include(b => b.Household);
            return View(budgetItems.ToList());
        }

        // GET: BudgetItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // GET: Budgets/AddItem/5
        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.Id = id;
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Value,CategoryId")] BudgetItem item, int id)
        {
            if (ModelState.IsValid)
            {
                Budget budget = db.Budgets.FirstOrDefault(b => b.Id == id);
                item.Created = DateTimeOffset.Now;
                item.BudgetId = id;
                UpdateBalance(item);

                if (budget.HouseholdId != null)
                {
                    item.HouseholdId = budget.HouseholdId;
                }
                
                db.BudgetItems.Add(item);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Budgets", new { id = id });
        }

        private void UpdateBalance(BudgetItem item)
        {
            Budget budget = db.Budgets.FirstOrDefault(b => b.Id == item.BudgetId);
            if (item.Value >= 0)
            {
                Category category = db.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                if (category.Name == "Income")
                {
                    budget.Balance += item.Value;
                }
                else if (category.Name == "Expense")
                {
                    budget.Balance -= item.Value;
                }
            }
            else if (item.Value < 0)
            {
                item.Value = item.Value * (-1);
                Category category = db.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                if (category.Name == "Income")
                {
                    budget.Balance += item.Value;
                }
                else if (category.Name == "Expense")
                {
                    budget.Balance -= item.Value;
                }
            }
        }

        // GET: BudgetItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OldValue = budgetItem.Value;
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "BudgetName", budgetItem.BudgetId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budgetItem.CategoryId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budgetItem.HouseholdId);
            return View(budgetItem);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Value,Created,BudgetId,CategoryId")] BudgetItem budgetItem, decimal OldValue)
        {
            if (ModelState.IsValid)
            {
                if(budgetItem.Value != OldValue)
                {
                    UpdateBalance(budgetItem);
                }
                budgetItem.Updated = DateTimeOffset.Now;
                db.Entry(budgetItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Budgets", new { id = budgetItem.BudgetId });
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "BudgetName", budgetItem.BudgetId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budgetItem.CategoryId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budgetItem.HouseholdId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            db.BudgetItems.Remove(budgetItem);
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
