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
using System.Net.Mail;
using System.Threading.Tasks;

namespace Saturn_Budgeter.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
        public ActionResult Index()
        {
            var invitations = db.Invitations.Include(i => i.Household).Include(i => i.Recipient).Include(i => i.Sender);
            return View(invitations.ToList());
        }

        // GET: Invitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,RecipientFirstName,RecipientLastName,RecipientEmail,SenderId,RecipientId,HouseholdId")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Invitations.Add(invitation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", invitation.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", invitation.SenderId);
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", invitation.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", invitation.SenderId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,RecipientFirstName,RecipientLastName,RecipientEmail,SenderId,RecipientId,HouseholdId")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", invitation.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", invitation.SenderId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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

        //GET Invitations/Invite/5
        public ActionResult Invite(int? id)
        {
            ViewBag.HouseholdId = id;
            ViewBag.SenderId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite([Bind(Include = "HouseholdId,SenderId,RecipientEmail,RecipientFirstName,RecipientLastName,Message")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser recipient = db.Users.FirstOrDefault(user => user.Email == invitation.RecipientEmail);
                ApplicationUser sender = db.Users.FirstOrDefault(u => u.Id == invitation.SenderId);

                if (recipient != null)
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
                    sender.DisplayName, Url.Action("Join", "Invitations", routeValues: new { id = invitation.HouseholdId }, protocol: Request.Url.Scheme)),
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

        //GET Invitations/Join/5 (householdid)
        public ActionResult Join(int id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
            HouseholdJoinViewModel model = new HouseholdJoinViewModel();
            model.Household = db.Households.FirstOrDefault(household => household.Id == id);
            model.Invitation = model.Household.Invitations.FirstOrDefault(invitation => invitation.RecipientEmail.ToLower() == user.Email.ToLower());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(int id, bool invitationAccepted)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
            Invitation invitation = db.Invitations.FirstOrDefault(i => i.RecipientEmail == user.Email);

            if (invitationAccepted)
            {
                Household household = db.Households.FirstOrDefault(h => h.Id == id);
                user.HouseholdId = id;
                household.Users.Add(user);
                household.Invitations.Remove(invitation);
                invitation.Accepted = true;
                db.Entry(user).State = EntityState.Modified;
                db.Entry(household).State = EntityState.Modified;
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                invitation.Accepted = false;
            }

            return RedirectToAction("Details", "Households", new { id = id });
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(int id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
            Household household = db.Households.FirstOrDefault(h => h.Id == id);
            household.Users.Remove(user);
            user.HouseholdId = null;

            return RedirectToAction("Index", "Home");
        }
    }
}
