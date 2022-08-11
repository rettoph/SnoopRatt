using Image = SixLabors.ImageSharp.Image;

using Discord;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Entities;
using SnoopRatt.QuickChart;
using SnoopRatt.QuickChart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Extensions
{
    public static class QuickChartServiceExtensions
    {
        public static async Task<Image> GetImage(this QuickChartService service, QuickChartRequest request)
        {
            using (var stream = await service.GetStream(request))
            {
                return Image.Load(stream);
            }
        }

        public static async Task<Image> GetSecondsLineChart(this QuickChartService service, IEnumerable<Ping> mentions)
        {
            List<DataSet> dataSets = new List<DataSet>();

            foreach(var users in mentions.Where(x => x.Status == RoleMentionStatus.OnTime).GroupBy(x => x.UserId))
            {
                dataSets.Add(new DataSet()
                {
                    Label = users.Key,
                    Data = Enumerable.Range(0, 60).Select(x => (float?)users.Count(m => m.TimeStamp.Second == x))
                });
            }

            return await service.GetImage(new QuickChartRequest()
            {
                Width = 300,
                Height = 180,
                Chart = new Chart()
                {
                    Type = ChartType.Line,
                    Data = new Data()
                    {
                        Labels = Enumerable.Range(0, 60).Select(x => (object)x),
                        DataSets = dataSets
                    },
                    Options = new Options()
                    {
                        Title = new Title()
                        {
                            Text = "4:20 Seconds Distribution",
                            Display = Display.True
                        },
                        Plugins = new Plugins()
                        {
                            Legend = false,
                        }
                    }
                }
            });
        }

        public static async Task<Image> GetPieChart<TKey>(this QuickChartService service, Dictionary<TKey, int> data)
            where TKey : notnull
        {
            return await service.GetImage(new QuickChartRequest()
            {
                Chart = new Chart()
                {
                    Type = ChartType.Doughnut,
                    Data = new Data()
                    {
                        Labels = data.Select(x => x.Key.ToString()),
                        DataSets = new[]
                        {
                            new DataSet()
                            {
                                Data = data.Select(x => (float?)x.Value)
                            }
                        }
                    },
                    Options = new Options()
                    {
                        Plugins = new Plugins()
                        {
                            Legend = false,
                            DataLabels = new DataLabels()
                            {
                                Display = Display.Auto,
                                BackgroundColor = "#ccc",
                                BorderRadius = 3,
                                Font = new Font()
                                {
                                    Color = "red",
                                    Weight = "bold"
                                }
                            },
                            DoughnutLabel = new DoughnutLabel()
                            {
                                Labels = new[]
                                {
                                    new Label()
                                    {
                                        Text = data.Sum(x => x.Value).ToString(),
                                        Font = new Font()
                                        {
                                            Size = 20,
                                            Weight = "bold"
                                        }
                                    },
                                    new Label()
                                    {
                                        Text = "Total"
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
