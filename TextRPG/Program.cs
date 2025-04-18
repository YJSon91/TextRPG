using System;
using System.Collections.Generic;

internal class Program
{
    static string name;
    static string job = "Novice";
    static int lv = 1;
    static int atk = 10;
    static int def = 5;
    static int health = 100;
    static int gold = 1500;

    static List<Item> _shopItems = new List<Item>(); // 상점 아이템 목록
    static List<Item> _inventory = new List<Item>(); // 인벤토리 (보유 중인 아이템)

    static void Main(string[] args)
    {
        ItemData();

        Console.WriteLine("던전시커에 오신 것을 환영합니다.");
        Console.WriteLine("이름을 알려주세요.\n");
        name = Console.ReadLine();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"입력하신 이름은 {name}입니다.");
            Console.WriteLine("이름을 저장하시겠습니까?");
            Console.WriteLine("1. 저장");
            Console.WriteLine("2. 취소\n");

            if (int.TryParse(Console.ReadLine(), out int selectInput))
            {
                switch (selectInput)
                {
                    case 1:
                        Console.WriteLine("원하시는 행동을 입력해주세요\n");
                        SelectAction();
                        break;
                    case 2:
                        Main(args);
                        break;
                }

                if (selectInput >= 1 && selectInput <= 2)
                    break;
                else
                    Console.WriteLine("잘못된 입력입니다. '1'에서 '2' 사이의 숫자를 입력하세요\n");
            }
        }
    }

    static void SelectAction()
    {
        while (true)
        {
            Console.WriteLine($"안녕하세요, {name}님!\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n");

            if (int.TryParse(Console.ReadLine(), out int selectInput))
            {
                switch (selectInput)
                {
                    case 1:
                        ViewStatus();
                        break;
                    case 2:
                        Inventory();
                        break;
                    case 3:
                        Store();
                        break;
                }

                if (selectInput >= 1 && selectInput <= 3)
                    break;
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다. '1'에서 '3' 사이의 숫자를 입력하세요.\n");
                }
            }
        }
    }

    static void ViewStatus()
    {
        while (true)
        {
            int totalAtk = atk;
            int totalDef = def;

            foreach (var item in _inventory)
            {
                if (item.isEquipped)
                {
                    totalAtk += item.itemAtk;
                    totalDef += item.itemDef;
                }
            }

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine($"공격력 : {totalAtk}");
            Console.WriteLine($"방어력 : {totalDef}");
            Console.WriteLine($"체 력 : {health}");
            Console.WriteLine($"Gold : {gold} G\n");
            Console.WriteLine("0. 나가기\n");

            if (int.TryParse(Console.ReadLine(), out int selectInput) && selectInput == 0)
            {
                Console.Clear();
                SelectAction();
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 나가기를 입력하세요.\n");
            }
        }
    }

    static void Inventory()
    {
        while (true)
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            if (_inventory.Count == 0)
            {
                Console.WriteLine("보유한 아이템이 없습니다.");
            }
            else
            {
                foreach (var item in _inventory)
                {
                    item.PrintItemStatDescription(true);
                }
            }

            Console.WriteLine("\n1. 장착관리");
            Console.WriteLine("0. 나가기\n");

            if (int.TryParse(Console.ReadLine(), out int selectInput))
            {
                switch (selectInput)
                {
                    case 0:
                        Console.Clear();
                        SelectAction();
                        return;
                    case 1:
                        Equip();
                        return;
                }
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다. '0'에서 '1' 사이의 숫자를 입력하세요.\n");
            }
        }
    }

    static void Equip()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("장착관리");
            Console.WriteLine("장착 또는 해제할 아이템을 선택하세요.\n");

            for (int i = 0; i < _inventory.Count; i++)
            {
                _inventory[i].PrintItemStatDescription(true, i + 1);
            }

            Console.WriteLine("\n0. 나가기\n");

            if (int.TryParse(Console.ReadLine(), out int selectInput))
            {
                if (selectInput == 0)
                {
                    Console.Clear();
                    Inventory();
                    break;
                }
                else if (selectInput >= 1 && selectInput <= _inventory.Count)
                {
                    var selectedItem = _inventory[selectInput - 1];

                    if (selectedItem.isEquipped)
                    {
                        selectedItem.isEquipped = false;
                        atk -= selectedItem.itemAtk;
                        def -= selectedItem.itemDef;
                        Console.WriteLine($"\n{selectedItem.itemName}을(를) 해제했습니다.");
                    }
                    else
                    {
                        selectedItem.isEquipped = true;
                        atk += selectedItem.itemAtk;
                        def += selectedItem.itemDef;
                        Console.WriteLine($"\n{selectedItem.itemName}을(를) 장착했습니다.");
                    }

                    Console.WriteLine("아무 키나 누르면 계속합니다...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }

    static void Store()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine($"[보유 골드] {gold} G\n");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < _shopItems.Count; i++)
            {
                var item = _shopItems[i];
                string status = _inventory.Any(inv => inv.itemName == item.itemName) ? "구매완료" : $"{item.itemPrice}G";
                Console.WriteLine($"{i + 1}. {item.itemName} | 공격력: {item.itemAtk} | 방어력: {item.itemDef} | {status}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요: ");

            if (int.TryParse(Console.ReadLine(), out int selectInput))
            {
                if (selectInput == 0)
                {
                    Console.Clear();
                    return;
                }
                else if (selectInput == 1)
                {
                    BuyItem();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }

    static void BuyItem()
    {
        Console.Clear();
        Console.WriteLine("아이템 구매");
        Console.WriteLine("[구매 가능한 아이템 목록]");

        for (int i = 0; i < _shopItems.Count; i++)
        {
            var item = _shopItems[i];
            string status = _inventory.Any(inv => inv.itemName == item.itemName) ? "구매완료" : $"{item.itemPrice}G";
            Console.WriteLine($"{i + 1}. {item.itemName} | 공격력: {item.itemAtk} | 방어력: {item.itemDef} | {status}");
        }

        Console.Write("\n구매할 아이템 번호를 입력하세요 (0. 취소): ");
        if (int.TryParse(Console.ReadLine(), out int input))
        {
            if (input == 0)
            {
                return;
            }
            else if (input >= 1 && input <= _shopItems.Count)
            {
                var selectedItem = _shopItems[input - 1];

                if (_inventory.Any(i => i.itemName == selectedItem.itemName))
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else if (gold >= selectedItem.itemPrice)
                {
                    gold -= selectedItem.itemPrice;
                    _inventory.Add(selectedItem);
                    Console.WriteLine("구매를 완료했습니다.");
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public class Item
    {
        public string itemName { get; set; }
        public string itemDescription { get; set; }
        public int itemAtk { get; set; }
        public int itemDef { get; set; }
        public int itemPrice { get; set; }
        public bool isEquipped { get; set; }
        public static int itemCount = 0;

        public Item() { }

        public Item(string itemName, string itemDescription, int itemAtk, int itemDef, int itemPrice, bool isEquipped = false)
        {
            this.itemName = itemName;
            this.itemDescription = itemDescription;
            this.itemAtk = itemAtk;
            this.itemDef = itemDef;
            this.itemPrice = itemPrice;
            this.isEquipped = isEquipped;
        }

        public void PrintItemStatDescription(bool includeIndex = false, int index = 0)
        {
            string equipStatus = isEquipped ? "[E]" : "";
            string indexStr = includeIndex ? $"{index}. " : "";
            Console.WriteLine($"{indexStr}{itemName} {equipStatus} | 공격력: {itemAtk} | 방어력: {itemDef} | 가격: {itemPrice}G");
        }
    }

    private static void ItemData()
    {
        _shopItems.Add(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 5, 1000));
        _shopItems.Add(new Item("낡은 검", "무난한 무기다. 초보자들을 위한 검.", 2, 0, 600));
        _shopItems.Add(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 5, 0, 1600));
        _shopItems.Add(new Item("무쇠 갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, 1500));
        _shopItems.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, 3500));
        _shopItems.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 3500));
    }
      
}
