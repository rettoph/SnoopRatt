using SnoopRatt.QuickChart.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace SnoopRatt.QuickChart.Services
{
    public class QuickChartService
    {
        private HttpClient _client;
        private JsonSerializerOptions _options;

        public QuickChartService()
        {
            _client = new HttpClient();

            _options = new JsonSerializerOptions();
            _options.Converters.Add(new ChartTypeConverter());
            _options.Converters.Add(new DisplayConverter());
        }

        public async Task<Stream> GetStream(QuickChartRequest request)
        {
            var json = JsonSerializer.Serialize<object>(request, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://quickchart.io/chart", content);

            return response.Content.ReadAsStream();
        }
    }
}
