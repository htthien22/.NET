namespace Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Product()
        {
            Tbl_OrderDetail = new HashSet<Tbl_OrderDetail>();
        }

        public long ID { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(4000)]      
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        [Required]
        public int Price { get; set; }
        [Required]
        public int PromotionPrice { get; set; }


        public int Quantity { get; set; }

        public long? CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        public int Warranty { get; set; }

        public DateTime? CreatedDate { get; set; }


        public DateTime? ModifiedDate { get; set; }



        public bool? Status { get; set; }

        public bool? FlashSale { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_OrderDetail> Tbl_OrderDetail { get; set; }

        public virtual Tbl_ProductCategory Tbl_ProductCategory { get; set; }

    }
}
