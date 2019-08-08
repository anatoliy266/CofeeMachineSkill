using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AliseCofeemaker
{
    public enum CofeeStatus
    {
        grinding = 0,
        boiling,
        cleaning,
        artWorking,
        pouring,
        finished,
        outwork
    }

    public interface ICofee
    {
        CofeeStatus status { get; set; }
        void ChangeCofeeStatus();
    }

    public class CofeeMaker : ICofee
    {
        private CofeeStatus _status;
        public CofeeMaker()
        {
            _status = CofeeStatus.outwork;
        }

        public CofeeStatus status { get => _status; set => _status = value; }

        public void ChangeCofeeStatus()
        {
            Random r = new Random();
            int statusCode = r.Next(6);
            _status = (CofeeStatus)statusCode;
        }
    }

    
}
