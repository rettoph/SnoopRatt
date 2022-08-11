using SnoopRatt.App.Entities;
using SnoopRatt.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Extensions
{
    public static class PingEnumerableExtensions
    {
        public static IEnumerable<Streak> GetStreaks(this IEnumerable<Ping> pings)
        {
            if(pings is null)
            {
                return Enumerable.Empty<Streak>();
            }    

            var streaks = new List<Streak>();
            var items = new List<Ping>();

            foreach (var periods in pings.Where(x => x.Status != RoleMentionStatus.None).GroupBy(x => x.Period))
            {
                foreach (var users in periods.GroupBy(x => x.UserId))
                {
                    var date = DateTime.MinValue;

                    items.Clear();

                    foreach (var mention in users.OrderBy(x => x.TimeStamp))
                    {
                        if(mention.Status != RoleMentionStatus.OnTime)
                        {
                            AddStreak(streaks, items);
                        }
                        else if(items.Count() == 0)
                        {
                            items.Add(mention);
                        }
                        else if((mention.TimeStamp.Date - date.Date).Days == 1)
                        {
                            items.Add(mention);
                        }
                        else
                        {
                            AddStreak(streaks, items);
                            items.Add(mention);
                        }

                        date = mention.TimeStamp.Date;
                    }

                    AddStreak(streaks, items);
                }
            }

            return streaks;
        }

        private static void AddStreak(List<Streak> streaks, List<Ping> pings)
        {
            if (pings.Count() > 1)
            {
                streaks.Add(new Streak()
                {
                    First = pings.Min(x => x.TimeStamp),
                    Last = pings.Max(x => x.TimeStamp),
                    Pings = pings.OrderBy(x => x.TimeStamp).ToArray()
                });
            }

            pings.Clear();
        }
    }
}
