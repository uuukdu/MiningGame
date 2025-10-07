using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningGame
{
    class Player
    {
        public string Name { get; set; }
        public int Gold { get; set; }
        public Inventory Inventory { get; set; }
        public Item CurrentPickaxe { get; set; } // Item 클래스를 곡괭이로 사용
    }
}
