using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ninject;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.Domain.Concrete;
using System.Configuration;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Infrastructure.Concrete;

namespace BookStore.WebUI.Infrastructure
{
    class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            ////using Moq; // for Mock
            ////using BookStore.Domain.Abstract; // for IBookRepository
            ////using BookStore.Domain.Entities; // for Book

            //*** Static data
            //Mock<IBookRepository> mock=new Mock<IBookRepository>();
            //mock.Setup(b=>b.Books).Returns(
            //    new List<Book> { new Book {Title="SQL Server DB",Price=50M },
            //                     new Book {Title="Asp.net MVC 5",Price=90M },
            //                     new Book {Title="Web Client",Price=87M }});
            //kernel.Bind<IBookRepository>().ToConstant(mock.Object);

            //*** dynamic data
            //using BookStore.Domain.Concrete; // for IBookRepository
            // using System.Configuration; //for ConfigurationManager
            //using BookStore.Domain.Abstract; // for IOrderProcessor
            EmailSettings emailSettings=new EmailSettings
            {
                WriteAsFile=bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"]??"false")
            };

            kernel.Bind<IBookRepository>().To<EFBookRepository>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("setting",emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

        }
    }
}
