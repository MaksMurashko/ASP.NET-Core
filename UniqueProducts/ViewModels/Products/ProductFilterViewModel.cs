namespace UniqueProducts.ViewModels.Products
{
    public class ProductFilterViewModel
    {
        public string SelectedProductName { get; }

        public ProductFilterViewModel(string name)
        {
            SelectedProductName = name;
        }
    }
}
