namespace UniqueProducts.ViewModels
{
    public class PaginationViewModel<T, M, K>
    {
        public IEnumerable<T> Items { get; }
        public PageViewModel PageViewModel { get; }

        public K SortViewModel { get; }

        public M FilterViewModel { get; }
        public PaginationViewModel(IEnumerable<T> items, PageViewModel viewModel, M filterViewModel, K sortModel)
        {
            Items = items;
            PageViewModel = viewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortModel;
        }
    }
}
