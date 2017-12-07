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
        public ActionResult Create([Bind(Include = "Id,Name,Description,Balance,StartDate,EndDate")] Budget budget, string shared)
        {
            if (ModelState.IsValid)
            {
                if (budget.EndDate != null && budget.StartDate == null)
                {
                    ModelState.AddModelError("EndDate", "Budgets with an end date must have a start date");
                    return View(budget);
                }
                else if(budget.StartDate != null && budget.EndDate != null)
                {
                    if (!IsBefore(budget.StartDate.GetValueOrDefault(), budget.EndDate.GetValueOrDefault()))
                    {
                        ModelState.AddModelError("EndDate", "Start date must be before end date");
                        return View(budget);
                    }
                }

                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (shared == "true")
                {
                    budget.HouseholdId = user.HouseholdId;
                }
                budget.Created = DateTimeOffset.Now;
                budget.UserId = User.Identity.GetUserId();
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(budget);
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

            if(budget.StartDate != null)
            {
                ViewBag.OldStartDate = FormatDate(budget.EndDate.GetValueOrDefault());
            }
            else
            {
                ViewBag.OldStartDate = null;
            }

            if(budget.EndDate != null)
            {
                ViewBag.OldEndDate = FormatDate(budget.EndDate.GetValueOrDefault());
            }
            else
            {
                ViewBag.OldEndDate = null;
            }
            
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Balance,Created,StartDate,EndDate,HouseholdId,UserId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                if (budget.EndDate != null && budget.StartDate == null)
                {
                    if (budget.StartDate != null)
                    {
                        ViewBag.OldStartDate = FormatDate(budget.EndDate.GetValueOrDefault());
                    }
                    else
                    {
                        ViewBag.OldStartDate = null;
                    }

                    if (budget.EndDate != null)
                    {
                        ViewBag.OldEndDate = FormatDate(budget.EndDate.GetValueOrDefault());
                    }
                    else
                    {
                        ViewBag.OldEndDate = null;
                    }
                    ModelState.AddModelError("EndDate", "Budgets with an end date must have a start date");
                    return View(budget);
                }
                else if (budget.StartDate != null && budget.EndDate != null)
                {
                    if (!IsBefore(budget.StartDate.GetValueOrDefault(), budget.EndDate.GetValueOrDefault()))
                    {
                        if (budget.StartDate != null)
                        {
                            ViewBag.OldStartDate = FormatDate(budget.EndDate.GetValueOrDefault());
                        }
                        else
                        {
                            ViewBag.OldStartDate = null;
                        }

                        if (budget.EndDate != null)
                        {
                            ViewBag.OldEndDate = FormatDate(budget.EndDate.GetValueOrDefault());
                        }
                        else
                        {
                            ViewBag.OldEndDate = null;
                        }
                        ModelState.AddModelError("EndDate", "Start date must be before end date");
                        return View(budget);
                    }
                }

                budget.Updated = DateTimeOffset.Now;
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (budget.StartDate != null)
            {
                ViewBag.OldStartDate = FormatDate(budget.EndDate.GetValueOrDefault());
            }
            else
            {
                ViewBag.OldStartDate = null;
            }

            if (budget.EndDate != null)
            {
                ViewBag.OldEndDate = FormatDate(budget.EndDate.GetValueOrDefault());
            }
            else
            {
                ViewBag.OldEndDate = null;
            }

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

        private bool IsBefore(DateTimeOffset start, DateTimeOffset end)
        {
            if (start != null && end == null)
            {
                return true;
            }
            else if (start == null)
            {
                return false;
            }

            if (start.Year < end.Year)
            {
                return true;
            }
            else if (start.Year == end.Year && start.Month < end.Month)
            {
                return true;
            }
            else if (start.Year == end.Year && start.Month == end.Month && start.Day < end.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string FormatDate(DateTimeOffset date)
        {
            string formattedDate = "";
            if (date.Year < 1000)
            {
                if (date.Year > 0)
                {
                    int length = date.Year.ToString().Length;
                    string formattedString = date.Year.ToString();
                    for (; length < 4; length++)
                    {
                        formattedString = formattedString.Insert(0, "0");
                    }
                    formattedDate += formattedString + "-";
                }
                else
                {
                    formattedDate += "0000-";
                }
            }
            else if (date.Year > 9999)
            {
                formattedDate += "9999-";
            }
            else
            {
                formattedDate += date.Year.ToString() + "-";
            }

            if (date.Month < 10)
            {
                if (date.Month > 0)
                {
                    string formattedString = date.Month.ToString();
                    formattedString = formattedString.Insert(0, "0");
                    formattedDate += formattedString + "-";
                }
                else
                {
                    formattedDate += "01-";
                }
            }
            else
            {
                formattedDate += date.Month.ToString() + "-";
            }

            if (date.Day < 10)
            {
                if (date.Day > 0)
                {
                    string formattedString = date.Day.ToString();
                    formattedString = formattedString.Insert(0, "0");
                    formattedDate += formattedString;
                }
                else
                {
                    formattedDate += "01";
                }
            }
            else
            {
                formattedDate += date.Day.ToString();
            }

            return formattedDate;
        }

    }
}
