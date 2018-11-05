using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Domain.Abstract;
using Moq;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Book1" },
                new Book {ISBN=2,Title="Book2" },
                new Book {ISBN=3,Title="Book3" }
            });
           AdminController target = new AdminController(mock.Object);

            //Act
            Book[] result =((IEnumerable<Book>) target.Index().ViewData.Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("Book1", result[0].Title);
            Assert.AreEqual("Book2", result[1].Title);
            Assert.AreEqual("Book3", result[2].Title);
        }

        [TestMethod]
        public void Can_Edit_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Book1" },
                new Book {ISBN=2,Title="Book2" },
                new Book {ISBN=3,Title="Book3" }
            });
            AdminController target = new AdminController(mock.Object);

            //Act
            Book b1 = target.Edit(1).ViewData.Model as Book;
            Book b2 = (Book)target.Edit(2).ViewData.Model;
            Book b3 = target.Edit(3).ViewData.Model as Book;
         
            //Assert
            Assert.AreEqual("Book1", b1.Title);
            Assert.AreEqual(2, b2.ISBN);
            Assert.AreEqual("Book3", b3.Title);
        }

        [TestMethod]
        public void Can_not_Edit_Not_Existis_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Book1" },
                new Book {ISBN=2,Title="Book2" },
                new Book {ISBN=3,Title="Book3" }
            });
            AdminController target = new AdminController(mock.Object);

            //Act
            Book b4 = (Book)target.Edit(4).ViewData.Model;

            //Assert
            Assert.IsNull(b4);

        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book =new Book {Title="Test Book" };
            //Act

            ActionResult result = target.Edit(book);
            //Assert
            mock.Verify(b=>b.SaveBook(book));
            Assert.IsNotInstanceOfType(result,typeof(ViewResult));

        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "Test Book" };
            target.ModelState.AddModelError("error","error");
            //Act

            ActionResult result = target.Edit(book);
            //Assert
            mock.Verify(b => b.SaveBook(It.IsAny<Book>()),Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public void Can_Delete_Valid_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            Book book = new Book {ISBN=1,  Title = "Test Book" };
            mock.Setup(m => m.Books).Returns(new Book[]{             
                new Book {ISBN=2,Title="Book2" },
                new Book {ISBN=3,Title="Book3" },
                book
            });
            AdminController target = new AdminController(mock.Object);
            
            //Act

            target.Delete(1);
            //Assert
            mock.Verify(b => b.DeleteBook(book.ISBN));

        }


    }
}
