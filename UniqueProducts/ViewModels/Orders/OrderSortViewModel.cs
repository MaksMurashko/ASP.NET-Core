namespace UniqueProducts.ViewModels.Orders
{
    public enum SortState
    {
        OrderNAsc,OrderNDesc,
        OrderDateAsc, OrderDateDesc,
        OrderClientAsc,OrderClientDesc,
        OrderProductAsc, OrderProductDesc,
        OrderAmountAsc, OrderAmountDesc,
        TotalPriceAsc, TotalPriceDesc,
        IsCompletedAsc, IsCompletedDesc,
        OrderEmployeeAsc,OrderEmployeeDesc,
    }

    public class OrderSortViewModel
    {
        public SortState OrderNSort { get; }
        public SortState OrderDateSort { get; }
        public SortState OrderClientSort { get; }
        public SortState OrderProductSort { get; }
        public SortState OrderAmountSort { get; }
        public SortState TotalPriceSort { get; }
        public SortState IsCompletedSort { get; }
        public SortState OrderEmployeeSort { get; }

        public SortState Current { get; }

        public OrderSortViewModel(SortState state)
        {
            OrderNSort = state == SortState.OrderNAsc ? SortState.OrderNDesc : SortState.OrderNAsc;
            OrderDateSort = state == SortState.OrderDateAsc ? SortState.OrderDateDesc : SortState.OrderDateAsc;
            OrderClientSort = state == SortState.OrderClientAsc ? SortState.OrderClientDesc : SortState.OrderClientAsc;
            OrderProductSort = state == SortState.OrderProductAsc ? SortState.OrderProductDesc : SortState.OrderProductAsc;
            OrderAmountSort = state == SortState.OrderAmountAsc ? SortState.OrderAmountDesc : SortState.OrderAmountAsc;
            TotalPriceSort = state == SortState.TotalPriceAsc ? SortState.TotalPriceDesc : SortState.TotalPriceAsc;
            IsCompletedSort = state == SortState.IsCompletedAsc ? SortState.IsCompletedDesc : SortState.IsCompletedAsc;
            OrderEmployeeSort = state == SortState.OrderEmployeeAsc ? SortState.OrderEmployeeDesc : SortState.OrderEmployeeAsc;

            Current = state;
        }
    }
}
