using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySanBong.Models
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }
        public int InvoiceId { get; set; }
        public int YardDetailId { get; set; }
        public DateTime DateBook { get; set; } // Time đặt sân
        public int? Price { get; set; }
        public virtual YardDetail? YardDetail { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}
