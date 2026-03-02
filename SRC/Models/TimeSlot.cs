using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySanBong.Models
{
    public class TimeSlot
    {
        public TimeSlot() {
            YardDetails = new HashSet<YardDetail>();
        }
        public int TimeSlotId { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public virtual ICollection<YardDetail> YardDetails { get; set; }
    }
}
