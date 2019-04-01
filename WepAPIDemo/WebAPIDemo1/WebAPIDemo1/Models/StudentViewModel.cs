using System.ComponentModel.DataAnnotations;

namespace WebAPIDemo1.Models
{
    public class StudentViewModel
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AddressViewModel Address { get; set; }
        public StandardViewModel Standard { get; set; }
    }
}