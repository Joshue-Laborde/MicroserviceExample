namespace ADMReestructuracion.Common.Data.Collection
{
    public class DataCollection<T>
    {
        public bool HasItems => Items != null && Items.Any();
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }
}
