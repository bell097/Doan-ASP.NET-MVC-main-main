namespace Doan_ASP.NET_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image_detail
    {
        [Key]
        public int image_id { get; set; }

        public int product_id { get; set; }

        [Required]
        [StringLength(50)]
        public string product_image_detail { get; set; }

        public virtual Product Product { get; set; }
    }
}
