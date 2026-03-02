using System;
using System.Collections.Generic;

namespace QuanLySanBong.Models
{
    public class ReportViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public List<RevenueByMonth> RevenueByMonths { get; set; } = new List<RevenueByMonth>();
        public List<RevenueByPlayground> RevenueByPlaygrounds { get; set; } = new List<RevenueByPlayground>();
        public List<Invoice> RecentInvoices { get; set; } = new List<Invoice>();
        public string TopPlaygroundName { get; set; }
    }

    public class RevenueByMonth
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RevenueByPlayground
    {
        public string PlaygroundName { get; set; }
        public decimal Revenue { get; set; }
    }
}
