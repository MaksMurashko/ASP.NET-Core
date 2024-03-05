namespace UniqueProducts.ViewModels.Clients
{
    public class ClientFilterViewModel
    {
        public string SelectedCompany { get; }
        public string SelectedPhone { get; }

        public ClientFilterViewModel(string selectedCompany, string selectedPhone)
        {
            SelectedCompany = selectedCompany;
            SelectedPhone = selectedPhone;
        }
    }
}
