namespace QuanLySanBong.Models
{
    public class CartDetail
    {
        public int CartDetailId { get; set; }
        public int CartId { get; set; }
        public virtual Cart? Cart { get; set; }
        public int YardDetailId { get; set; }
        public DateTime DateBook { get; set; } // Time đặt sân
        public virtual YardDetail? YardDetail { get; set; }
        public int Price { get; set; }
    }
}
