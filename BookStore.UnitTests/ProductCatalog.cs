using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using BookStore.WebUI.HtmlHelper;
using System.Web.Mvc; // for HtmlHelper
using BookStore.WebUI.Models;//for PagingInfo

namespace BookStore.UnitTests
{


    [TestClass]
    public class ProductCatalog
    {
        [TestMethod]
        public void CanPaginate()
        {
            //using Moq;
            //using BookStore.Domain.Abstract;
            //using BookStore.Domain.Entities;

            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]{
                new Book {ISBN=1000,Title="Book1" },
                new Book {ISBN=1010,Title="Book2" },
                new Book {ISBN=1020,Title="Book3" },
                new Book {ISBN=1030,Title="Book4" },
                new Book {ISBN=1040,Title="Book5" }
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            ////Act

            //test 1
            //IEnumerable<Book> result = (IEnumerable<Book>)controller.List(2).Model;

            ////Asssert
            //Book[] bookArray = result.ToArray();
            //Assert.IsTrue(bookArray.Length == 2);
            //Assert.AreEqual(bookArray[0].Title, "Book4");
            //Assert.AreEqual(bookArray[1].Title, "Book5");

            //test 2
            //Act
            //IEnumerable<Book> result = (IEnumerable<Book>)controller.List(1).Model;
            //BookListViewModel result = (BookListViewModel)controller.List(1).Model;
            BookListViewModel result = (BookListViewModel)controller.List(null, 1).Model;

            //Asssert
            //Book[] bookArray = result.ToArray();
            Book[] bookArray = result.Books.ToArray();
            Assert.IsTrue(bookArray.Length == 3);
            Assert.AreEqual(bookArray[0].Title, "Book1");
            Assert.AreEqual(bookArray[1].Title, "Book2");
            Assert.AreEqual(bookArray[2].Title, "Book3");
        }

        [TestMethod]
        public void Can_Generate_Page_links()
        {
            //using System.Web.Mvc; // for HtmlHelper
            //using BookStore.WebUI.Models;//for PagingInfo

            //Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 14,
                ItemsPerPage = 5
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            String expectedResult = "<a class=\"btn btn-default\" href=\"Page1\">1</a>"
                                   + "<a class=\"btn btn-default btn-primary selected\" href=\"Page2\">2</a>"
                                   + "<a class=\"btn btn-default\" href=\"Page3\">3</a>";
            //Act
            String result = myHelper.PageLinks(pagingInfo, pageUrlDelegate).ToString();

            //Asssert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Operating System" },
                new Book {ISBN=2,Title="Web Application" },
                new Book {ISBN=3,Title="Android Mobile Application" },
                new Book {ISBN=4,Title="Database Systems" },
                new Book {ISBN=5,Title="MIS" }
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            //Act
            //BookListViewModel result = (BookListViewModel)controller.List(2).Model;
            BookListViewModel result = (BookListViewModel)controller.List(null, 2).Model;

            //Asssert
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPage, 2);

        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Operating System" ,Specialization="CS"},
                new Book {ISBN=2,Title="Web Application",Specialization="IS" },
                new Book {ISBN=3,Title="Android Mobile Application",Specialization="IS" },
                new Book {ISBN=4,Title="Database Systems",Specialization="IS" },
                new Book {ISBN=5,Title="MIS",Specialization="IS" }
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            //Act
            Book[] result = ((BookListViewModel)controller.List("IS", 2).Model).Books.ToArray();

            //Asssert
            Assert.AreEqual(result.Length, 1);
            Assert.IsTrue(result[0].Title == "MIS" && result[0].Specialization == "IS");
        }

        [TestMethod]
        public void Can_Create_Specialization()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Operating System" ,Specialization="CS"},
                new Book {ISBN=2,Title="Web Application",Specialization="IS" },
                new Book {ISBN=3,Title="Android Mobile Application",Specialization="IS" },
                new Book {ISBN=4,Title="Database Systems",Specialization="IS" },
                new Book {ISBN=5,Title="MIS",Specialization="IS" }
            });
            NavController controller = new NavController(mock.Object);

            //Act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            //Asssert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0] == "CS" && result[1] == "IS");
        }

        [TestMethod]
        public void Indicates_Selected_Spec()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Operating System" ,Specialization="CS"},
                new Book {ISBN=2,Title="Web Application",Specialization="IS" },
                new Book {ISBN=3,Title="Android Mobile Application",Specialization="IS" },
                new Book {ISBN=4,Title="Database Systems",Specialization="IS" },
                new Book {ISBN=5,Title="MIS",Specialization="IS" }
            });
            NavController controller = new NavController(mock.Object);

            //Act
            string result = controller.Menu("IS").ViewBag.SelectedSpec;

            //Asssert
            Assert.AreEqual("IS",result);
        }

        [TestMethod]
        public void Genrate_Spec_Specific_Book_Count()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]{
                new Book {ISBN=1,Title="Operating System" ,Specialization="CS"},
                new Book {ISBN=2,Title="Web Application",Specialization="IS" },
                new Book {ISBN=3,Title="Android Mobile Application",Specialization="IS" },
                new Book {ISBN=4,Title="Database Systems",Specialization="CS" },
                new Book {ISBN=5,Title="MIS",Specialization="IS" }
            });
            BookController controller = new BookController(mock.Object);

            //Act
            int result1 = ((BookListViewModel)controller.List("IS").Model).PagingInfo.TotalItems;
            int result2 = ((BookListViewModel)controller.List("CS").Model).PagingInfo.TotalItems;

            //Asssert
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);
        }
    }
}

