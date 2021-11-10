using Nancy.Hosting.Self;
using System;
using System.Configuration;
using System.Data.SQLite;
using System.ServiceProcess;

namespace WorkLifeBalanceTracker
{
    public partial class TrackerService : ServiceBase
    {
        public TrackerService()
        {
            InitializeComponent();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    TrackStart("Session Logon");
                    break;
                case SessionChangeReason.SessionLogoff:
                    TrackEnd("Session Logoff");
                    break;
                case SessionChangeReason.RemoteConnect:
                    TrackStart("Remote Connect");
                    break;
                case SessionChangeReason.RemoteDisconnect:
                    TrackEnd("Remote Disconnect");
                    break;
                case SessionChangeReason.SessionLock:
                    TrackEnd("Session Lock");
                    break;
                case SessionChangeReason.SessionUnlock:
                    TrackStart("Session Unlock");
                    break;
                default:
                    break;
            }
        }

        protected override void OnShutdown()
        {
            TrackEnd("System Shutdown");
        }

        protected override void OnStart(string[] args)
        {
            TrackStart("Service Starting");
            _host = new NancyHost(new Uri(ConfigurationManager.AppSettings["siteUrl"]));
            _host.Start();
            using (var connection = new SQLiteConnection("Data Source=WorkLifeTracker.sqlite;Version=3;"))
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS TimeEntries (StartTime DateTime, StartReason VARCHAR(20), EndTime DateTime, EndReason VARCHAR(20))";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    command.ExecuteNonQuery();
            }
        }

        protected override void OnStop()
        {
            TrackEnd("Service Stopping");
            _host.Stop();
            _host.Dispose();
        }

        private NancyHost _host;

        private void TrackStart(string reason)
        {
            using (var connection = new SQLiteConnection("Data Source=WorkLifeTracker.sqlite;Version=3;"))
            {
                connection.Open();

                string sql = @"UPDATE TimeEntries
                                SET StartTime = $StartTime,
                                    StartReason = $StartReason
                                WHERE EndTime IS NULL;

                                INSERT INTO TimeEntries(StartTime, StartReason)
                                SELECT $StartTime, $StartReason
                                WHERE NOT EXISTS(SELECT 1 FROM TimeEntries WHERE EndTime IS NULL);";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("$StartTime", DateTime.Now);
                    command.Parameters.AddWithValue("$StartReason", reason);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void TrackEnd(string reason)
        {
            using (var connection = new SQLiteConnection("Data Source=WorkLifeTracker.sqlite;Version=3;"))
            {
                connection.Open();

                string sql = @"UPDATE TimeEntries
                                SET EndTime = $EndTime,
                                    EndReason = $EndReason
                                WHERE EndTime IS NULL;";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("$EndTime", DateTime.Now);
                    command.Parameters.AddWithValue("$EndReason", reason);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
