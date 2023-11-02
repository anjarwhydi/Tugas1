using System.Data;

namespace Tugas.ViewModels
{
    public class JqueryDatatableParam
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataTableSearch Search { get; set; }
        public int orderColumn { get; set; }
        public string orderDirection { get; set; }
    }

    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool IsRegex { get; set; }
    }
}