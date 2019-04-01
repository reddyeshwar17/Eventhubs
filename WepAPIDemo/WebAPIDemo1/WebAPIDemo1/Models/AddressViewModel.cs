using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebAPIDemo1.Models
{
   
    public class AddressViewModel
    {
        [Key]
        public int StudentId { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}