using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrafficDensityApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string apiKey = "57821jvsSOgwJSP3fM8ohN5jm50rRkSPGzh0qOfOe2Wc4mlsXaFEJQQJ99AHACYeBjFiG8lGAAAgAZMPS28l"; //api key
            var distanceCalculator = new DistanceCalculator();

            bool continueProgram = true;

            while (continueProgram)
            {
                
                Console.WriteLine("Lütfen trafik verisini almak istediğiniz şehrin adını girin:");
                string cityName = Console.ReadLine();

                
                string geocodingUrl = $"https://atlas.microsoft.com/search/address/json?api-version=1.0&subscription-key={apiKey}&query={cityName}";

                HttpClient client = new HttpClient();
                try
                {
                    
                    var geocodingResponse = await client.GetStringAsync(geocodingUrl);
                    var geocodingData = JsonConvert.DeserializeObject<dynamic>(geocodingResponse);
                    string latitude = geocodingData.results[0].position.lat;
                    string longitude = geocodingData.results[0].position.lon;
                    string cityCoordinates = $"{latitude},{longitude}";

                    
                    string trafficUrl = $"https://atlas.microsoft.com/traffic/flow/segment/json?api-version=1.0&subscription-key={apiKey}&style=relative&zoom=10&query={cityCoordinates}";
                    var trafficResponse = await client.GetStringAsync(trafficUrl);

                    
                    var trafficData = JsonConvert.DeserializeObject<TrafficData>(trafficResponse);

                    
                    if (trafficData != null)
                    {
                        var data = trafficData.FlowSegmentData;      
                        Console.WriteLine("FRC: " + data.Frc);
                        Console.WriteLine("Current Speed: " + data.CurrentSpeed + " km/h");
                        Console.WriteLine("Free Flow Speed: " + data.FreeFlowSpeed + " km/h");
                        Console.WriteLine("Current Travel Time: " + data.CurrentTravelTime + " seconds");
                        Console.WriteLine("Free Flow Travel Time: " + data.FreeFlowTravelTime + " seconds");
                        Console.WriteLine("Confidence: " + data.Confidence);
                        Console.WriteLine("Road Closure: " + data.RoadClosure);
                        

                       
                        var coordinates = data.Coordinates.Coordinate;
                        if (coordinates.Count > 1)
                        {
                            double lat1 = coordinates[0].Latitude;
                            double lon1 = coordinates[0].Longitude;
                            double lat2 = coordinates[1].Latitude;
                            double lon2 = coordinates[1].Longitude;

                            double distance = distanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);
                            Console.WriteLine($"İki nokta arasındaki mesafe: {distance} km");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Veri alınamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                }
                finally
                {
                    
                    client.Dispose();
                }

                
                Console.WriteLine("Başka bir şehir için trafik verisi almak istiyor musunuz?");
                Console.WriteLine("1. Evet");
                Console.WriteLine("2. Hayır");
                

                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                       
                        continue;
                    case "2":
                        
                        continueProgram = false;
                        break;
                    case "3":
                        
                        continueProgram = false;
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim");
                        continueProgram = false;
                        break;
                }
            }
        }
    }
}
