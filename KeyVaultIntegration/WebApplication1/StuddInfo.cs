using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class StuddInfo
    {
        public List<StudentInfo> list { get; set; }

        public StuddInfo()
        {
           this.list = new List<StudentInfo>();
            StudentInfo info = new StudentInfo();
            info = new StudentInfo
            {
                Id = 101,
                Name = "Eswar",
                Email = "Reddy@r.com",
                Address = "Hyd"
            };
            list.Add(info);
            info = new StudentInfo
            {
                Id = 102,
                Name = "Eswar Reddy",
                Email = "EswarReddy@r.com",
                Address = "Hyd"
            };
            list.Add(info);
        }
    }   
}