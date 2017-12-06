using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saturn_Budgeter.Controllers;

namespace Saturn_Budgeter_Tests.Controllers
{
    [TestClass]
    public class BankAccountControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            BankAccountsController controller = new BankAccountsController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
