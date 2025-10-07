using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningGame
{
    public abstract class Item
    {
        public string Name { get; set; }
    }

    public class Mineral : Item
    {
        // 광물 클래스에만 필요한 판매 가격 속성
        public int SellPrice { get; set; }
    }

    public class Pickaxe : Item
    {
        // 곡괭이 클래스에만 필요한 구매 가격 속성
        public int Price { get; set; }
    }

    public static class Items
    {
        // 광물
        public static readonly Mineral Stone = new Mineral { Name = "돌", SellPrice = 5 };
        public static readonly Mineral Iron = new Mineral { Name = "철", SellPrice = 10 };
        public static readonly Mineral Gold = new Mineral { Name = "금", SellPrice = 15 };
        public static readonly Mineral Diamond = new Mineral { Name = "다이아몬드", SellPrice = 20 };

        // 곡괭이
        public static readonly Pickaxe StonePickaxe = new Pickaxe { Name = "돌 곡괭이", Price = 0 };
        public static readonly Pickaxe IronPickaxe = new Pickaxe { Name = "철 곡괭이", Price = 100 };
        public static readonly Pickaxe GoldPickaxe = new Pickaxe { Name = "금 곡괭이", Price = 200 };
        public static readonly Pickaxe DiamondPickaxe = new Pickaxe { Name = "다이아몬드 곡괭이", Price = 300 };
    }
}