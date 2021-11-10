using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLifeBalanceTracker.Models;

namespace WorkLifeBalanceTracker
{
    public class RoutesModule : NancyModule
    {
        public RoutesModule()
        {
            Get("/", _ =>
            {
                return View["home.html", State.Entries.OrderByDescending(e => e.Date)];
            });

            Get("/{date}", parameters => {

                return View["day.html", State.Entries.FirstOrDefault(e => e.Title == parameters.date)];
            });
        }
    }
}
