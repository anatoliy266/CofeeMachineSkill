﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;


namespace AliseCofeemaker
{
    public interface IStatus
    {
        int CheckStatus();
    }

    public class StatusChecker : IStatus
    {
        private WebClient checker;
        public StatusChecker()
        {
            
        }
        public int CheckStatus()
        {
            checker = new WebClient();
            //checker.UseDefaultCredentials = true;
            //checker.Headers.Add(HttpRequestHeader.Allow, "GET");
            //checker.Headers.Add(HttpRequestHeader.Connection, "close");
            var result = Convert.ToInt32(checker.DownloadString("https://apigate.info/api/cofee"));
            
            return result;
        }
    }
}
