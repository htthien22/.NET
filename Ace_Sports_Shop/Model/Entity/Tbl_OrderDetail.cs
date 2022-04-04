namespace Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProductID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long OrderID { get; set; }

        public int? Quantity { get; set; }


        public string Size { get; set; }
        public string Color { get; set; }

        public int Price { get; set; }

        public virtual Tbl_Order Tbl_Order { get; set; }

        public virtual Tbl_Product Tbl_Product { get; set; }
    }
}
