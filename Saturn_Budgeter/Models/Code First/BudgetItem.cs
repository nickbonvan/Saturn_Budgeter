using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saturn_Budgeter.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public int? HouseholdId { get; set; }
        public int CategoryId { get; set; }
        public int BudgetId { get; set; }

        public virtual Household Household { get; set; }
        public virtual Category Category { get; set; }
        public virtual Budget Budget { get; set; }
    }
}