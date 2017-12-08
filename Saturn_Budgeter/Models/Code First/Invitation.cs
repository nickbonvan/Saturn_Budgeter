using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saturn_Budgeter.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientEmail { get; set; }
        public bool? Accepted { get; set; }

        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}