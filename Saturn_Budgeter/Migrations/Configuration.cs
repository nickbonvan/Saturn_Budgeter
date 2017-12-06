namespace Saturn_Budgeter.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Saturn_Budgeter.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Saturn_Budgeter.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Saturn_Budgeter.Models.ApplicationDbContext db)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if(!db.Users.Any(user => user.Email == "nicholas.bonvan@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    FirstName = "Nicholas",
                    LastName = "Bonvan",
                    DisplayName = "Nicholas Bonvan",
                    Email = "nicholas.bonvan@gmail.com",
                    UserName = "nicholas.bonvan@gmail.com",
                    PhoneNumber = "5555555555"
                }, "Password-1");
            }

            db.Categories.AddOrUpdate(category => category.Id,
                new Category { Id = 1, Name = "Expense" },
                new Category { Id = 2, Name = "Income" });

            db.AccountTypes.AddOrUpdate(type => type.Id,
                new AccountType { Id = 1, Name = "Checking" },
                new AccountType { Id = 2, Name = "Savings" },
                new AccountType { Id = 3, Name = "Shared" });

            db.TransactionTypes.AddOrUpdate(type => type.Id,
                new TransactionType { Id = 1, Name = "Debit" },
                new TransactionType { Id = 2, Name = "Credit" });


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
