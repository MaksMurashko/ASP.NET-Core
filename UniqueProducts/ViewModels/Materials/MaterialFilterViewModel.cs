namespace UniqueProducts.ViewModels.Materials
{
    public class MaterialFilterViewModel
    {
        public string SelectedMaterialName { get; }
        public int SelectedMaterialCode { get; }
        public MaterialFilterViewModel(string name, int code)
        {
            SelectedMaterialName = name;
            SelectedMaterialCode = code;
        }
    }
}
