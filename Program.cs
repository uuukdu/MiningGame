using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Menu 클래스의 객체(인스턴스)를 생성
            Menu menu = new Menu();

            // 생성된 menu 객체의 Run 메서드를 호출
            menu.Run();
        }
    }
}