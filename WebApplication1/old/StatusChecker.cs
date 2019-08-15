using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using AliseCofeemaker.Controllers;
using AliseCofeemaker.Modules;
using AliseCofeemaker.Models;


namespace AliseCofeemaker.Services
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
            var result = Convert.ToInt32(checker.DownloadString("https://apigate.info/api/cofee"));
            
            return result;
        }
    }
}
