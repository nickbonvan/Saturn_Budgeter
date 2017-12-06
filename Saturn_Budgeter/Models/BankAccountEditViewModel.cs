using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Saturn_Budgeter.Models
{
    public class BankAccountEditViewModel
    {
        public BankAccount BankAccount { get; set; }
        public SelectList AccountTypes { get; set; }
    }
}