using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseCofeemaker.Modules
{
    public interface ILot
    {
        void AddActor(string actor);
        string GetActor(); 
    }

    public class LotManager : ILot
    {
        private List<string> actors;
        public LotManager()
        {
            actors = new List<string>();
        }

        /// <summary>
        /// Добление учасника розыгрыша
        /// </summary>
        /// <param name="actor"></param>
        public void AddActor(string actor)
        {
            actors.Add(actor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetActor()
        {
            Random r = new Random();
            int nActor = r.Next(actors.Count);
            return actors[nActor];
        }
        /// <summary>
        /// Очистка списка участников
        /// </summary>
        public void Clear()
        {
            actors.Clear();
        }
    }
}
