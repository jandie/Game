using System.Collections.Generic;
using DeGame.Classes;

namespace DeGame.Repository
{
    interface IGameRepo
    {
        List<Map> LoadAllMaps();

        bool SaveAllMaps(List<Map> _maps);
    }
}
