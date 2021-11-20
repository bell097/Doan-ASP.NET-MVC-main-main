namespace Doan_ASP.NET_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BillDetail")]
    public partial class BillDetail
    {
        [Key]
        public int billdetail_id { get; set; }

        public int bill_id { get; set; }

        public int product_id { get; set; }

        public long price { get; set; }

        public int soluong { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual Product Product { get; set; }

        public string productname;
    }
}
