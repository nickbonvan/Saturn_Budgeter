using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saturn_Budgeter.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public decimal Balance { get; set; }
        
        public int AccountTypeId { get; set; }
        public int? HouseholdId { get; set; }
        public string UserId { get; set; }

        public virtual Household Household { get; set; }
        public virtual AccountType Type { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }


        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}