using System;
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
    [Authorize]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            var budgets = db.Budgets.Include(b => b.Household);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BudgetName,BudgetDescription,BudgetBalance,StartDate,EndDate")] Budget budget, string shared)
        {
            if (ModelState.IsValid)
            {
                if(!isBefore(budget.StartDate.GetValueOrDefault(), budget.EndDate.GetValueOrDefault()))
                {
                    ModelState.AddModelError("EndDate", "Start date must be before end date");
                    return View(budget);
                }
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (shared == "true")
                {
                    budget.HouseholdId = user.HouseholdId;
                }
                budget.BudgetCreated = DateTimeOffset.Now;
                budget.UserId = User.Identity.GetUserId();
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(budget);
        }

        private bool isBefore(DateTimeOffset start, DateTimeOffset end)
        {
            if(start != null && end == null)
            {
                return true;
            }
            else if(start == null)
            {
                return false;
            }

            if(start.Year < end.Year)
            {
                return true;
            }
            else if(start.Year == end.Year && start.Month < end.Month)
            {
                return true;
            }
            else if(start.Year == end.Year && start.Month == end.Month && start.Day < end.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Budgets/AddItem/5
        [Authorize]
        public ActionResult AddItem(int? id)
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
        public ActionResult AddItem([Bind(Include = "Name,Description,Value,CategoryId")] BudgetItem item, int id)
        {
            if (ModelState.IsValid)
            {
                Budget budget = db.Budgets.FirstOrDefault(b => b.Id == id);
                if(budget.HouseholdId != null)
                {
                    item.HouseholdId = budget.HouseholdId;
                }
                if(item.Value >= 0)
                {
                    Category category = db.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                    if(category.Name == "Income")
                    {
                        budget.BudgetBalance += item.Value;
                    }
                    else if(category.Name == "Expense")
                    {
                        budget.BudgetBalance -= item.Value;
                    }
                }
                else if (item.Value < 0)
                {
                    item.Value = item.Value * (-1);
                    Category category = db.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                    if (category.Name == "Income")
                    {
                        budget.BudgetBalance += item.Value;
                    }
                    else if (category.Name == "Expense")
                    {
                        budget.BudgetBalance -= item.Value;
                    }
                }
                item.Created = DateTimeOffset.Now;
                item.BudgetId = id;
                db.BudgetItems.Add(item);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Budgets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BudgetName,BudgetDescription,BudgetBalance,BudgetCreated,Updated,StartDate,EndDate,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
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
