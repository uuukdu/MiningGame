using System;
using System.Collections.Generic;

namespace MiningGame
{
    // 플레이어가 바라보는 방향 열거형
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Player
    {
        public string Name { get; set; } // 플레이어 닉네임
        public int Gold { get; set; } // 플레이어 보유 골드
        public Inventory Inventory { get; private set; } // 플레이어 인벤토리
        public Pickaxe EquippedPickaxe { get; set; } // 현재 장착 중인 곡괭이

        // 플레이어가 현재 바라보는 방향을 저장할 속성
        public Direction PlayerDirection { get; set; }

        public Player(string name)
        {
            Name = name;
            Gold = 1000;
            Inventory = new Inventory();
            EquippedPickaxe = Items.StonePickaxe;
            PlayerDirection = Direction.Down; // 시작할 때 기본적으로 아래쪽을 보도록 설정함
        }

        // 플레이어 정보 출력
        public void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine("==========[내 정보]==========");
            Console.WriteLine($"\n닉네임: {Name}");
            Console.WriteLine($"골드: {Gold} G");
            Console.WriteLine($"장착중인 곡괭이: {EquippedPickaxe.Name}");

            // ShowInventory() 메서드 호출
            Inventory.ShowInventory();
        }
    }
}