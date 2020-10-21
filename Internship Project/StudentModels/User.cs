using System.ComponentModel.DataAnnotations;

namespace StudentModels
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string UserName { get; set; }
        [StringLength(255)]
        public string UserRole { get; set; }
        [StringLength(255)]
        public string UserEmail { get; set; }
        [StringLength(255)]
        public string UserPassword { get; set; }
    }
}
