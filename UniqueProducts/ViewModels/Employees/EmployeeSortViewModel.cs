namespace UniqueProducts.ViewModels.Employees
{
    public enum SortState
    {
        EmployeeNameAsc, EmployeeNameDesc,
        EmployeeSurnameAsc, EmployeeSurnameDesc,
        EmployeeMidNameAsc, EmployeeMidNameDesc,
        EmployeePositionAsc, EmployeePositionDesc,
    }

    public class EmployeeSortViewModel
    {
        public SortState NameSort { get; }

        public SortState SurnameSort { get; }

        public SortState MidNameSort { get; }

        public SortState PositionSort { get; }

        public SortState Current { get; }

        public EmployeeSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.EmployeeNameAsc ? SortState.EmployeeNameDesc : SortState.EmployeeNameAsc;
            SurnameSort = sortOrder == SortState.EmployeeSurnameAsc ? SortState.EmployeeSurnameDesc : SortState.EmployeeSurnameAsc;
            MidNameSort = sortOrder == SortState.EmployeeMidNameAsc ? SortState.EmployeeMidNameDesc : SortState.EmployeeMidNameAsc;
            PositionSort = sortOrder == SortState.EmployeePositionAsc ? SortState.EmployeePositionDesc : SortState.EmployeePositionAsc;

            Current = sortOrder;
        }
    }
}
