namespace UniqueProducts.ViewModels.Clients
{
    public enum SortState
    {
        CompanyAsc, CompanyDesc,
        RepresentativeAsc, RepresentativeDesc,
        PhoneAsc, PhoneDesc,
        CompanyAddressAsc, CompanyAddressDesc
    }

    public class ClientSortViewModel
    {
        public SortState CompanySort { get; }

        public SortState RepresentativeSort { get; }

        public SortState PhoneSort { get; }

        public SortState CompanyAddressSort { get; }

        public SortState Current { get; }

        public ClientSortViewModel(SortState state)
        {
            CompanySort = state == SortState.CompanyAsc ? SortState.CompanyDesc : SortState.CompanyAsc;
            RepresentativeSort = state == SortState.RepresentativeAsc ? SortState.RepresentativeDesc : SortState.RepresentativeAsc;
            PhoneSort = state == SortState.PhoneAsc ? SortState.PhoneDesc : SortState.PhoneAsc;
            CompanyAddressSort = state == SortState.CompanyAddressAsc ? SortState.CompanyAddressDesc : SortState.CompanyAddressAsc;

            Current = state;
        }
    }
}
