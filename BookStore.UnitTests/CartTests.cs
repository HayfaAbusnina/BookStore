using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Domain.Entities;
using System.Linq;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_NewLines()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };

            //Act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2,3);
            CartLine[] result = target.lines.ToArray();

            //Assert
            Assert.AreEqual(result[0].Book, b1);
            Assert.AreEqual(result[1].Book, b2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Items()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };

            //Act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b1,5);

            CartLine[] result = target.lines.OrderBy(cl=>cl.Book.ISBN).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 3);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };
            Book b3 = new Book { ISBN = 3, Title = "C#" };

            //Act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);
            target.AddItem(b2, 1);

            target.RemoveLine(b2);
            //Assert
            Assert.AreEqual(target.lines.Where(cl => cl.Book == b2).Count(), 0);
            Assert.AreEqual(target.lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" ,Price=100M};
            Book b2 = new Book { ISBN = 2, Title = "Oracle",Price=50M };
            Book b3 = new Book { ISBN = 3, Title = "C#",Price=70M };

            //Act
            Cart target = new Cart();
            target.AddItem(b1,1);
            target.AddItem(b2, 2);
            target.AddItem(b3);

            decimal result =target.ComupteTotalValue();
            //Assert
            Assert.AreEqual(result,270M);

        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };
            Book b3 = new Book { ISBN = 3, Title = "C#" };

            //Act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);
            target.AddItem(b2, 1);

            target.Clear();
            //Assert
            Assert.AreEqual(target.lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Asp.net MVC",Specialization="Programming" }
            }.AsQueryable()
            );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object,null);


            //Act
            target.AddToCart(cart, 1, null);

            //Assert
            Assert.AreEqual(cart.lines.Count(), 1);
            Assert.AreEqual(cart.lines.ToArray()[0].Book.Title, "Asp.net MVC");
        }

        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Asp.net MVC",Specialization="Programming" }
            }.AsQueryable()
            );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);
            
            //Act
            RedirectToRouteResult result=target.AddToCart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Adding_View_Cart_Content()
        {
            //Arrange
            Cart cart = new Cart();
            CartController target = new CartController(null, null);

            //Act
            CartIndexViewModel result =(CartIndexViewModel) target.Index(cart,"myUrl").ViewData.Model;

            //Assert
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Canot_Checkout_Empty_Cart()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetail shippingDetail = new ShippingDetail();
            CartController target = new CartController(null, mock.Object);

            //Act
            ViewResult result = target.Checkout(cart, shippingDetail);

            //Assert
            //mock.Verify(m=>m.ProcessOrder)
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Canot_Checkout_InValid_ShippingDetail()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(),1);
            ShippingDetail shippingDetail = new ShippingDetail();
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");

            //Act
            ViewResult result = target.Checkout(cart, shippingDetail);

            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetail>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetail shippingDetail = new ShippingDetail();
            CartController target = new CartController(null, mock.Object);

            //Act
            ViewResult result = target.Checkout(cart, shippingDetail);

            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetail>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);

        }
    }
}
