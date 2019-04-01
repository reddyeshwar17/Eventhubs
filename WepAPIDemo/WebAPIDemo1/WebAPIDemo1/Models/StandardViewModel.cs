using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPIDemo1.Models
{
    public class StandardViewModel
    {
        [Key]
        public int StandardId { get; set; }

        public string Name { get; set; }

        public ICollection<StudentViewModel> Students { get; set; }
    }
}