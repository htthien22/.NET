using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ViewModelOrder
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? CustomerID { get; set; }

        [StringLength(50)]
        public string ShipName { get; set; }

        [StringLength(50)]
        public string ShipMobile { get; set; }

        public string Size { get; set; }
        [StringLength(50)]
        public string ShipAddress { get; set; }

        public int? Quantity { get; set; }

        [StringLength(50)]
        public string ShipEmail { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal? Price { get; set; }

        [MaxLength(256)]
        public string PaymentMethod { set; get; }

        public bool Status { set; get; }
    }
}
