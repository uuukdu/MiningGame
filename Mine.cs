using System;

namespace MiningGame
{
    public class Mine
    {
        public string Name { get; private set; }
        public Pickaxe RequiredPickaxe { get; private set; }
        public Mineral PrimaryMineral { get; private set; }

        internal Mine(string name, Pickaxe requiredPickaxe, Mineral primaryMineral)
        {
            Name = name;
            RequiredPickaxe = requiredPickaxe;
            PrimaryMineral = primaryMineral;
        }

        // 광산 입장 시 실행될 메서드
        public void EnterMine(Player player)
        {
            // 기능 추가 예정
            Console.Clear();
            Console.WriteLine($"\n>> {Name}");
            Console.ReadKey();
        }
    }

    // 광산 DB(?)
    public static class Mines
    {
        public static readonly Mine StoneMine = new Mine("돌 광산", Items.StonePickaxe, Items.Stone);
        public static readonly Mine IronMine = new Mine("철 광산", Items.IronPickaxe, Items.Iron);
        public static readonly Mine GoldMine = new Mine("금 광산", Items.GoldPickaxe, Items.Gold);
        public static readonly Mine DiamondMine = new Mine("다이아몬드 광산", Items.DiamondPickaxe, Items.Diamond);
    }
}