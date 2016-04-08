using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeGame.Repository
{
    interface IGameRepo
    {
        List<Map> LoadAllMaps();

        bool SaveAllMaps(List<Map> _maps);
    }
}
