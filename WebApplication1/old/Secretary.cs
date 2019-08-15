using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker.Models;

namespace WebApplication1.Modules
{
    public interface ISecretary
    {
        void AddRecord(string dest, AppointmentModel record);
        void RemoveDestination(string dest);
        void AddDestination(string dest);
        List<DateTime> GetFreeDates();
        List<DateTime> GetFreeTimes();
    }
    public class Secretary : ISecretary
    {
        private Dictionary<string, List<AppointmentModel>> log;
        public Secretary()
        {
            log = new Dictionary<string, List<AppointmentModel>>();
            log["стоматолог"] = new List<AppointmentModel>();
            log["офис"] = new List<AppointmentModel>();
        }


        public void AddDestination(string dest)
        {
            log[dest] = new List<AppointmentModel>();
        }

        public void RemoveDestination(string dest)
        {
            log.Remove(dest);
        }
        public void AddRecord(string dest, AppointmentModel record)
        {
            var records = log[dest];
            records.Add(record);
            log[dest] = records;
        }

        public List<DateTime> GetFreeDates(string dest)
        {
            
        }

        public List<DateTime> GetFreeTimes()
        {
            
        }
    }
}
