using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebAPIDemo1.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("name=MyStudent")
        {

        }
        public DbSet<StudentViewModel> Student { get; set; }
        public DbSet<StandardViewModel> Students { get; set; }
        public DbSet<AddressViewModel> Address { get; set; }
    }
}