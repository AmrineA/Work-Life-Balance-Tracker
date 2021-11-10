using Nancy.Hosting.Self;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using WorkLifeBalanceTracker;
using WorkLifeBalanceTracker.Models;

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
            State.Entries = JsonConvert.DeserializeObject<List<DayEntry>>(File.ReadAllText(ConfigurationManager.AppSettings["logLocation"]));
            TrackStart("Service Starting");
            _host = new NancyHost(new Uri(ConfigurationManager.AppSettings["siteUrl"]));
            _host.Start();
        }

        protected override void OnStop()
        {
            TrackEnd("Service Stopping");
            _host.Stop();
            _host.Dispose();
        }

        private NancyHost _host;
        private DayEntry GetToday()
        {
            var entry = State.Entries.FirstOrDefault(e => e.Date == DateTime.Today);
            if (entry is null)
            {
                entry = new DayEntry() { Date = DateTime.Today };
                State.Entries.Add(entry);
            }
            return entry;
        }

        private void TrackStart(string reason)
        {
            State.TimeEntry = new TimeEntry() { StartTime = DateTime.Now, StartReason = reason };
            var entry = GetToday();
            entry.TimeEntries.Add(State.TimeEntry);
        }

        private void TrackEnd(string reason)
        {
            if (State.TimeEntry == null)
                return;

            State.TimeEntry.EndTime = DateTime.Now;
            State.TimeEntry.EndReason = reason;
            File.WriteAllText(ConfigurationManager.AppSettings["logLocation"], JsonConvert.SerializeObject(State.Entries));
            State.TimeEntry = null;
        }
    }
}
