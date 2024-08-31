namespace InventoryUi.POC
{
    public class CityViewModel
    {
        public string SearchTerm { get; set; }
        public List<City> SuggestedCities { get; set; } = new List<City>();
    }
}
