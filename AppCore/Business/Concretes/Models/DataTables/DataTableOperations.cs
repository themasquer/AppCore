namespace AppCore.Business.Concretes.Models.DataTables
{
    public class DataTableOperations
    {
        public string DetailsUrl { get; set; }
        public string EditUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string DetailsText { get; set; } = "Details";
        public string EditText { get; set; } = "Edit";
        public string DeleteText { get; set; } = "Delete";
        public string DetailsTitle { get; set; } = "Details";
        public string EditTitle { get; set; } = "Edit";
        public string DeleteTitle { get; set; } = "Delete";
        public string DetailsCss { get; set; } = "operationlink";
        public string EditCss { get; set; } = "operationlink";
        public string DeleteCss { get; set; } = "operationlink";
        public string OperationLinksProperty { get; set; } = "OperationLinks";
        public string OperationKeyProperty { get; set; } = "Id";
    }
}
