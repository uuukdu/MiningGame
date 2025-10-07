using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningGame
{
    // 아이템 종류
    public enum ItemType
    {
        Mineral,
        Pickaxe
    }

    // 곡괭이 종류
    public enum PickaxeType
    {
        Stone,
        Iron,
        Gold
    }

    // 광물 종류
    public enum MineralType
    {
        Dirt,
        Stone,
        Iron,
        Gold
    }

    class Item
    {
        public string Name { get; set; } // 아이템 이름
        public ItemType Type { get; set; } // 아이템 종류
        public int Price { get; set; } // 상점 구매 가격
        public int SellPrice { get; set; } // 상점 판매 가격
    }
}
