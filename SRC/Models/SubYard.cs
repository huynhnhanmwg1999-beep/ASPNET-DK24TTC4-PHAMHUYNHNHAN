namespace QuanLySanBong.Models
{
    public class SubYard
    {
        public SubYard()
        {
            YardDetails = new HashSet<YardDetail>();
        }
        public int SubYardId { get; set; }
        public string SubYardName { get; set; }
        public virtual ICollection<YardDetail> YardDetails { get; set; }

    }
}
