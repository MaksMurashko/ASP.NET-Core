namespace UniqueProducts.ViewModels.Products
{
    public enum SortState
    {
        ProductCodeAsc,ProductCodeDesc,
        ProductNameAsc, ProductNameDesc,
        ProductDescriptAsc, ProductDescriptDesc,
        ProductWeightAsc, ProductWeightDesc,
        ProductDiameterAsc, ProductDiameterDesc,
        ProductColorAsc, ProductColorDesc,
        ProductMaterialAsc, ProductMaterialDesc,
        ProductPriceAsc, ProductPriceDesc,
    }

    public class ProductSortViewModel
    {
        public SortState ProductCodeSort { get; }
        public SortState ProductNameSort { get; }
        public SortState ProductDescriptSort { get; }
        public SortState ProductWeightSort { get; }
        public SortState ProductDiameterSort { get; }
        public SortState ProductColorSort { get; }
        public SortState ProductMaterialSort { get; }
        public SortState ProductPriceSort { get; }
        public SortState Current { get; }

        public ProductSortViewModel(SortState state)
        {
            ProductCodeSort = state == SortState.ProductCodeAsc ? SortState.ProductCodeDesc : SortState.ProductCodeAsc;
            ProductNameSort = state == SortState.ProductNameAsc ? SortState.ProductNameDesc : SortState.ProductNameAsc;
            ProductDescriptSort = state == SortState.ProductDescriptAsc ? SortState.ProductDescriptDesc : SortState.ProductDescriptAsc;
            ProductWeightSort = state == SortState.ProductWeightAsc ? SortState.ProductWeightDesc : SortState.ProductWeightAsc;
            ProductDiameterSort = state == SortState.ProductDiameterAsc ? SortState.ProductDiameterDesc : SortState.ProductDiameterAsc;
            ProductColorSort = state == SortState.ProductColorAsc ? SortState.ProductColorDesc : SortState.ProductColorAsc;
            ProductMaterialSort = state == SortState.ProductMaterialAsc ? SortState.ProductMaterialDesc : SortState.ProductMaterialAsc;
            ProductPriceSort = state == SortState.ProductPriceAsc ? SortState.ProductPriceDesc : SortState.ProductPriceAsc;

            Current = state;
        }
    }
}
