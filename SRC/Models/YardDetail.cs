namespace QuanLySanBong.Models
{
    public class YardDetail
    {
        public YardDetail() {
            InvoiceDetails = new HashSet<InvoiceDetail>();
            CartDetails = new HashSet<CartDetail>();
        }
        public int YardDetailId { get; set; }
        public int PlayGroundId { get; set; }
        public int SubYardId { get; set; }
        public int TimeSlotId { get; set; }
        public int Price { get; set; }
        public virtual PlayGround? PlayGround { get; set; }
        public virtual SubYard? SubYard { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
