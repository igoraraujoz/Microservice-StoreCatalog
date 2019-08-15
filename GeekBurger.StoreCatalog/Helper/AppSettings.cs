namespace GeekBurger.StoreCatalog.Helper
{
    public class AppSettings
    {
        public ProductsApiSettings ProductsApiSettings { get; set; }
        public ProductionApiSettings ProductionApiSettings { get; set; }
        public IngredientsApiSettings IngredientsApiSettings { get; set; }
        public LojaSettings LojaSettings { get; set; }
    }
    public class ProductsApiSettings
    {
        public string Url { get; set; }
    }

    public class ProductionApiSettings
    {
        public string Url { get; set; }
    }

    public class IngredientsApiSettings
    {
        public string Url { get; set; }
    }

    public class LojaSettings
    {
        public string Nome { get; set; }
    }
}
