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

        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite(int HouseholdId, string InviteeEmail, string Message)
        {
            if (ModelState.IsValid)
            {
                if(InviteeEmail == null || InviteeEmail == "")
                {
                    return RedirectToAction("Details", new { id = HouseholdId });
                }

                ApplicationUser Invitee = db.Users.FirstOrDefault(user => user.Email == InviteeEmail);
                Household household = db.Households.FirstOrDefault(h => h.Id == HouseholdId);
                Invitation invitation;
                if (Invitee.Email == "" || Invitee.Email == null)
                {
                    invitation = new Invitation
                    {
                        HouseholdId = HouseholdId,
                        Message = Message,
                        RecipientId = null,
                        SenderId = User.Identity.GetUserId()
                    };
                }
                else
                {
                    invitation = new Invitation
                    {
                        HouseholdId = HouseholdId,
                        Message = Message,
                        RecipientId = Invitee.Id,
                        SenderId = User.Identity.GetUserId()
                    };
                }

                household.Invitations.Add(invitation);

                try
                {
                    string currentUserId = User.Identity.GetUserId();
                    ApplicationUser currentUser = db.Users.FirstOrDefault(user => user.Id == currentUserId);
                    ApplicationUser toUser = db.Users.FirstOrDefault(user => user.Id == Invitee.Id);
                    var email = new MailMessage(ConfigurationManager.AppSettings["emailfrom"], toUser.Email)
                    {
                        Subject = String.Format("You have been invited to {0}'s household", currentUser.DisplayName),
                        Body = String.Format("{0} invited you to their household.\nYou can follow this link to join it: {1}",
                        currentUser.DisplayName, Url.Action("Household", "Join")),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await Task.FromResult(0);
                }

                return RedirectToAction("Details", "Households", new { id = HouseholdId });
            }
            return RedirectToAction("Details", "Households", new { id = HouseholdId });
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
