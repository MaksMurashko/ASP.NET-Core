namespace UniqueProducts.ViewModels.Materials
{
    public enum SortState
    {
        MaterialCodeAsc,MaterialCodeDesc,
        MaterialNameAsc, MaterialNameDesc,
        MaterialDescriptAsc, MaterialDescriptDesc
    }

    public class MaterialSortViewModel
    {
        public SortState MaterialCodeSort { get; }
        public SortState MaterialNameSort { get; }
        public SortState MaterialDescriptSort { get; }
        public SortState Current { get; }

        public MaterialSortViewModel(SortState sortOrder)
        {
            MaterialCodeSort= sortOrder == SortState.MaterialCodeAsc ? SortState.MaterialCodeDesc : SortState.MaterialCodeAsc;
            MaterialNameSort = sortOrder == SortState.MaterialNameAsc ? SortState.MaterialNameDesc : SortState.MaterialNameAsc;
            MaterialDescriptSort = sortOrder == SortState.MaterialDescriptAsc ? SortState.MaterialDescriptDesc : SortState.MaterialDescriptAsc;

            Current = sortOrder;
        }
    }
}
