using System;
using SQLite;

namespace CashFlow.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string title { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public string image { get; set; }
        public string note { get; set; }
    }
}

