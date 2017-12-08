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
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;

namespace Saturn_Budgeter.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Created = DateTimeOffset.Now;
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                user.HouseholdId = household.Id;
                household.Users.Add(user);
                db.Households.Add(household);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        //GET Households/Invite/5
        public ActionResult Invite(int? id)
        {
            ViewBag.HouseholdId = id;
            ViewBag.SenderId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite([Bind(Include ="HouseholdId,SenderId,RecipientEmail,RecipientFirstName,RecipientLastName,Message")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser recipient = db.Users.FirstOrDefault(user => user.Email == invitation.RecipientEmail);
                ApplicationUser sender = db.Users.FirstOrDefault(u => u.Id == invitation.SenderId);

                if(recipient != null)
                {
                    invitation.RecipientId = recipient.Id;
                    invitation.RecipientFirstName = recipient.FirstName;
                    invitation.RecipientLastName = recipient.LastName;
                }

                db.Invitations.Add(invitation);
                db.SaveChanges();

                try
                {
                    string currentUserId = User.Identity.GetUserId();
                    MailMessage email = new MailMessage(sender.Email, invitation.RecipientEmail)
                    {
                        Subject = String.Format("You have been invited to {0}'s household", sender.DisplayName),
                        Body = String.Format("{0} invited you to their household.\nYou can follow this link to join it: {1}",
                    sender.DisplayName, Url.Action("Join", null, null, Request.Url.Scheme)),
                        IsBodyHtml = true
                    };

                    PersonalEmail svc = new PersonalEmail();
                    await svc.SendAsync(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await Task.FromResult(0);
                }

                return RedirectToAction("Details", "Households", new { id = invitation.HouseholdId });
            }
            ViewBag.HouseholdId = invitation.HouseholdId;
            return View();
        }

        public ActionResult Join(int id)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == User.Identity.GetUserId());
            Household household = db.Households.FirstOrDefault(h => h.Id == id);
            Invitation invitation = db.Invitations.FirstOrDefault(i => i.RecipientId == user.Id);
            user.HouseholdId = id;
            household.Users.Add(user);
            household.Invitations.Remove(invitation);

            return RedirectToAction("Details", "Households", new { id = id });
        }

        [Authorize]
        public ActionResult Leave(int id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
            Household household = db.Households.FirstOrDefault(h => h.Id == id);
            household.Users.Remove(user);
            user.HouseholdId = null;

            return RedirectToAction("Index", "Home");
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Updated = DateTimeOffset.Now;
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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
