using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCS.Core.Contracts;
using OSCS.Core.Models;
using OSCS.Core.ViewModels;
using OSCS.WebUI;
using OSCS.WebUI.Controllers;

namespace OSCS.WebUI.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            productContext.Insert(new Product());
            HomeController controller = new HomeController(productContext, productCategoryContext);

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel) result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
        }

    }
}
