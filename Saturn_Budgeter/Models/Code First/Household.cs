using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saturn_Budgeter.Models
{
    public class Household
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public virtual ICollection<BankAccount> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Household()
        {
            Accounts = new HashSet<BankAccount>();
            Budgets = new HashSet<Budget>();
            Invitations = new HashSet<Invitation>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}