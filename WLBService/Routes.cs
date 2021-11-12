using Nancy;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
                var results = new List<DayEntry>();

                using (var connection = new SQLiteConnection("Data Source=WorkLifeTracker.sqlite;Version=3;"))
                {
                    connection.Open();

                    string sql = @"SELECT Date(StartTime) AS Day, 
                                    COUNT(*) AS Cnt,
                                    SUM(julianday(IIF(EndTime IS NULL, datetime('now', 'localtime'), EndTime)) - julianday(StartTime)) * 86400 AS TotalTime
                                FROM TimeEntries 
                                GROUP BY Date(StartTime)
                                ORDER BY Day DESC;";
                    
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            results.Add(new DayEntry()
                            {
                                Day = reader.GetString(0),
                                Count = reader.GetInt32(1),
                                TotalTime = reader.GetFloat(2),
                            });
                        }
                        reader.Close();
                    }
                }
                return View["home.html", results];
            });

            Get("/weeks", _ =>
            {
                var results = new List<WeekEntry>();

                using (var connection = new SQLiteConnection("Data Source=WorkLifeTracker.sqlite;Version=3;"))
                {
                    connection.Open();

                    string sql = @"SELECT Date(StartTime, 'weekday 0', '-1 days') AS Day, 
                                        COUNT(*) AS Cnt,
                                        SUM(julianday(IIF(EndTime IS NULL, datetime('now', 'localtime'), EndTime)) - julianday(StartTime)) * 86400 AS TotalTime
                                    FROM TimeEntries 
                                    GROUP BY Date(StartTime, 'weekday 0', '-1 days')
                                    ORDER BY Day DESC;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            results.Add(new WeekEntry()
                            {
                                Day = reader.GetString(0),
                                Count = reader.GetInt32(1),
                                TotalTime = reader.GetFloat(2),
                            });
                        }
                        reader.Close();
                    }
                }
                return View["week.html", results];
            });

            Get("/days/{date}", parameters =>
            {
                var results = new DayVM();

                results.Title = parameters.date;

                using (var connection = new SQLiteConnection("Data Source=WorkLifeTracker.sqlite;Version=3;"))
                {
                    connection.Open();

                    string sql = @"SELECT StartTime, StartReason, 
                                    IIF(EndTime IS NULL, datetime('now', 'localtime'), EndTime) AS EndTime, 
                                    IIF(EndReason IS NULL, '', EndReason) AS EndReason
                                FROM TimeEntries WHERE date(StartTime) = $date
                                ORDER BY StartTime DESC";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("$date", parameters.date);
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            results.TimeEntries.Add(new TimeEntry()
                            {
                                StartTime = reader.GetDateTime(0),
                                StartReason = reader.GetString(1),
                                EndTime = reader.GetDateTime(2),
                                EndReason = reader.GetString(3),
                            });
                        }
                        reader.Close();
                    }
                }
                return View["day.html", results];
            });
        }
    }
}
