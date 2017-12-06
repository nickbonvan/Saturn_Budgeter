using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saturn_Budgeter.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public int? HouseholdId { get; set; }
        public string UserId { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }
}