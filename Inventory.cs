using System;
using System.Collections.Generic;
using System.Linq;

namespace MiningGame
{
    public class Inventory
    {
        // 광물(Key)과 개수(Value)를 저장
        private Dictionary<Mineral, int> _minerals;
        // 곡괭이들을 List로 저장
        private List<Pickaxe> _pickaxes;

        public Inventory()
        {
            // 메모리에 공간을 할당(초기화)
            _minerals = new Dictionary<Mineral, int>();
            _pickaxes = new List<Pickaxe>();
            // 게임 시작 시 돌 곡괭이를 인벤토리에 추가
            _pickaxes.Add(Items.StonePickaxe);
        }

        public void AddItem(Item item)
        {
            // 들어온 item이 Mineral 타입인지 확인
            if (item is Mineral mineral)
            {
                // 딕셔너리에 이미 해당 광물이 있는지 확인
                if (_minerals.ContainsKey(mineral))
                {
                    _minerals[mineral]++; // 있다면 개수만 1 증가
                }
                else
                {
                    _minerals.Add(mineral, 1); // 없다면 새로 추가하고 개수는 1
                }
                Console.WriteLine($"\n>> {mineral.Name} 1개 획득. (현재: {_minerals[mineral]}개)");
            }
            // 들어온 item이 Pickaxe 타입인지 확인
            else if (item is Pickaxe pickaxe)
            {
                // 리스트에 해당 곡괭이가 아직 없다면 추가
                if (!_pickaxes.Contains(pickaxe))
                {
                    _pickaxes.Add(pickaxe);
                    Console.WriteLine($"\n>> {pickaxe.Name}을(를) 획득했습니다.");
                }
            }
        }

        // 인벤토리 내용을 화면에 보여주는 기능
        public void ShowInventory()
        {
            Console.WriteLine("\n=========[인벤토리]=========");

            // 저장된 아이템의 개수가 0이라면
            if (_minerals.Count == 0) 
            {
                Console.WriteLine("보유한 광물이 없습니다.");
            }
            // 저장된 아이템이 있다면
            else
            {
                // 딕셔너리에 있는 모든 아이템에 대해 한번씩 코드를 실행
                foreach (var mineralPair in _minerals)
                {
                    // 광물의 이름과 개수 출력
                    Console.WriteLine($"- {mineralPair.Key.Name}: {mineralPair.Value}개");
                }
            }
            Console.WriteLine("============================");
        }

        // 플레이어가 특정 곡괭이를 가지고 있는지 확인
        public bool HasPickaxe(Pickaxe requiredPickaxe)
        {
            // 내가 가진 모든 곡괭이가 들어있는 _pickaxes 리스트에,
            // 광산에서 요구하는 곡괭이(requiredPickaxe)가 포함되어 있는지(.Contains) 확인 후 결과 반환
            return _pickaxes.Contains(requiredPickaxe);
        }
    }
}