﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CCS
{
    public class FileLogger : ILogger
    {
        public void Log()
        {
            Console.WriteLine("File Logged");
        }
       
    }
    public class DataBaseLogger : ILogger
    {
        public void Log()
        {
            Console.WriteLine("Database Logged");
        }
    }
}
