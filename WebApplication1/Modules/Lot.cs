using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseCofeemaker.Modules
{
    public interface ILot
    {
        void AddMember(string member);
        string GetMember();
    }

    public class Lot : ILot
    {
        private List<string> lotStorage;

        public void AddMember(string member)
        {
            lotStorage.Add(member);
        }

        public string GetMember()
        {
            if (lotStorage.Count > 0)
            {
                Random r = new Random();
                var i = r.Next(lotStorage.Count);
                return lotStorage[i];
            } else
            {
                return "Не из кого выбирать";
            }
        }
    }
}
