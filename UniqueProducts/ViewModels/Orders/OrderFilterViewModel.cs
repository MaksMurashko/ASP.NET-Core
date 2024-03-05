using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace UniqueProducts.ViewModels.Orders
{
    public class OrderFilterViewModel
    {
        public decimal SelectedOrderPrice { get; set; }
        public int SelectedOrderMonth { get; set; }

        public OrderFilterViewModel(decimal price, int month)
        {
            SelectedOrderPrice = price;
            SelectedOrderMonth = month;
        }
    }
}
