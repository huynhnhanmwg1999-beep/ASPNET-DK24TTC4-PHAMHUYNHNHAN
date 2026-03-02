namespace QuanLySanBong.Models
{
    public class Cart
    {
        public Cart()
        {
            CartDetails = new HashSet<CartDetail>();
        }
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int? Total { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
