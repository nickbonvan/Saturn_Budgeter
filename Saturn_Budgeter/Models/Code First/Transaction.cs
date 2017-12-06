using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saturn_Budgeter.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool Reconciled { get; set; }
        
        public int AccountId { get; set; }
        public string UserId { get; set; }
        public int TransactionTypeId { get; set; }

        public virtual BankAccount Account { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual TransactionType Type { get; set; }
    }
}