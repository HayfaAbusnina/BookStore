using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Data.Entity;

namespace BookStore.Domain.Concrete
{
    class EFDbContext:DbContext
    {
        //using System.Data.Entity;
        //using BookStore.Domain.Entities;
        public DbSet<Book> Books { set; get; }
    }
}
