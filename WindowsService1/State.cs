﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerService.Models;

namespace TrackerService
{
    public static class State
    {
        public static List<DayEntry> Entries = new List<DayEntry>();

        public static TimeEntry TimeEntry;
    }
}
