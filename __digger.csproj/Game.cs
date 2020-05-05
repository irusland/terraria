using System.Windows.Forms;

namespace Digger
{
    public class Game
    {
        private const string mapWithPlayerTerrain = @"
TTT T
TTP T
T T T
TT TT";

        private const string mapWithPlayerTerrainSackGold = @"
PTTGTT TS
TST  TSTT
TTTTTTSTT
T TSTS TT
T TTTG ST
TSTSTT TT";

        private const string mapWithPlayerTerrainSackGoldMonster = @"
PTTGTT TST
TST  TSTTM
TTT TTSTTT
T TSTS TTT
T TTTGMSTS
T TMT M TS
TSTSTTMTTT
S TTST  TG
 TGST MTTT
 T  TMTTTT";

        public readonly ICreature[,] Map;
        public int Scores { get; set; }
        public bool IsOver { get; set; }

        public Keys KeyPressed { get; set; }
        public int MapWidth => Map.GetLength(0);
        public int MapHeight => Map.GetLength(1);

        public Game(ICreature[,] map)
        {
            Map = map;
        }

        public static Game Create()
        {
            var map = CreatureMapCreator.CreateMap(mapWithPlayerTerrain);
            return new Game(map);
        }
    }
}