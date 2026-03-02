using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySanBong.Models
{
    public class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now; // time tạo hóa đơn
        public int? Total { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
