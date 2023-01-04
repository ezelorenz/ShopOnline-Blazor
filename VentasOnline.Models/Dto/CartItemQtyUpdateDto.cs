using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasOnline.Models.Dto
{
    public class CartItemQtyUpdateDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
