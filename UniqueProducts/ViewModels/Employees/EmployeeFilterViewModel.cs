namespace UniqueProducts.ViewModels.Employees
{
    public class EmployeeFilterViewModel
    {
        public string SelectedPosition { get; }
        public string SelectedSurname { get; }
        public EmployeeFilterViewModel(string position, string surname)
        {
            SelectedPosition = position;
            SelectedSurname = surname;
        }
    }
}
