using System.ComponentModel.DataAnnotations;

namespace QuanLySanBong.Models
{
    public class User
    {
        public User()
        {
            Invoices = new HashSet<Invoice>();
        }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Phải nhập trường này")]
        public string Username { get; set; }
        [StringLength(100)]
        public string DisplayName { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string PhoneNumber { get; set; }
        public bool? LoaiUser { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
