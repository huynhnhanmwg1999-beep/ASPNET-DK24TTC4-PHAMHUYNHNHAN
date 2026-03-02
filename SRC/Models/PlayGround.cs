namespace QuanLySanBong.Models
{
    public class PlayGround
    {
        public PlayGround()
        {
            YardDetails = new HashSet<YardDetail>();
        }
        public int PlayGroundId { get; set;}
        public string PlayGroundName { get; set;}
        public string PhoneNumber { get; set;}
        public string Address { get; set;}
        public string Image { get; set;}
        public string? Description { get; set;}
        public virtual ICollection<YardDetail> YardDetails { get; set; }
    }
}
