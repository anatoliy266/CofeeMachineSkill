using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseCofeemaker.Models
{
    //запись на прием
    public class AppointmentModel
    {
        //Место, в которое была сделана запись на прием
        public string Destination { get; set; }

        //Дата приема
        public DateTime Date { get; set; }

        //Время приема
        public DateTime Time { get; set; }

        //Продолжительность приема
        public int MeetingDuration { get; set; }
    }
}
