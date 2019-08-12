using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker.Models;

namespace WebApplication1.Modules
{
    public interface ISecretary
    {
        void AddRecord(AppointmentModel record);
        List<DateTime> GetFreeDates();
        List<DateTime> GetFreeTimes();
    }
    public class Secretary : ISecretary
    {
        private Dictionary<string, List<AppointmentModel>> log;
        public Secretary()
        {
            log = new Dictionary<string, List<AppointmentModel>>();
        }

        public void AddRecord(AppointmentModel record)
        {
            throw new NotImplementedException();
        }

        public List<DateTime> GetFreeDates()
        {
            throw new NotImplementedException();
        }

        public List<DateTime> GetFreeTimes()
        {
            throw new NotImplementedException();
        }
    }
}
