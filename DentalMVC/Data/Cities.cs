using Dental.Data.Enum;

namespace Dental.Data
{
    public static class Cities
    {
        private readonly static List<CityPostalCode> cities = new List<CityPostalCode>()
        {
            new CityPostalCode(){ City = "Zagreb", PostalCode = 10000},
            new CityPostalCode(){ City = "Samobor", PostalCode = 10430}
        };

        public static List<CityPostalCode> GetCity(string startsWith = "")
        {
            return cities.Where(c => c.City.StartsWith(startsWith)).ToList();
        }

        public static List<CityPostalCode> GetPostalCode(int startsWith)
        {
            return cities.Where(c => c.PostalCode.ToString().StartsWith(startsWith.ToString())).ToList();
        }

    }
}
