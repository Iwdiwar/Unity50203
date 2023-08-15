using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    //Все кнопки UI инвентаря
    [SerializeField] List<Button> buttons = new List<Button>();   
    //Все объекты из папки Resources
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    //Имена объектов, которые мы подняли
    List<string> inventoryItems = new List<string>();
    //То что у нас в руке
    GameObject itemInArm;
    //Точка в которую спавнятся объекты из инвентаря
    [SerializeField]Transform itemPoint;
    //Сообщения инвентаря(Text)
    [SerializeField] TMP_Text warning;
    //Предметы игрока на сцене
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    private void Start()
    {
        //Получаем все возможные объекты инвентаря, которые лежат в папке Resources
        GameObject[] objArr = Resources.LoadAll<GameObject>("");
        //Заполняем список возможных предметов инвентаря
        resourceItems.AddRange(objArr);
        //Перебираем все кнопки инвентаря на сцене и кладём их в список
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    
    public void AddItem(Sprite img, string itemName, GameObject obj)
    {        
        //Если у нас полный инвентарь, то выводим об этом сообщение и прерываем скрипт
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        //Если в инвентаре уже есть такой предмет, то выводим об этом сообщение и прерываем скрипт
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        //Добавляем пимя предмета в инвентарь
        inventoryItems.Add(itemName);
        //получаем следующую свободную кнопку и её компонент Image
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        //выставляем в кнопку картинку предмета, который подняли
        buttonImage.sprite = img;
        //Уничтожаем объект, который подобрали
        Destroy(obj);
    }
    //Метод, который стирает все сообщения
    void WarningUpdate()
    {
        warning.text = "";
    }
    //Этот метод вызывается по нажатию кнопки
    public void UseItem(int itemPos)
    {           
        //Если мы нажали кнопку, в которой ничего нет, то прерываем скрипт
        if (inventoryItems.Count <= itemPos) return;
        //записываем имя объекта, который присвоен этой кнопке в переменную
        string item = inventoryItems[itemPos];
        //Вызываем метод взятия объекта из инвентаря и передаем имя объекта, который хотим взять
        GetItemFromInventory(item);
    }
    //Метод взятия объекта
    public void GetItemFromInventory(string itemName)
    {
        //Перебираем все объекты в папке Resources
        foreach (var resourceItem in resourceItems)
        {
            //Если имя объекта совпало с тем, который мы хотим взять,то
            if (resourceItem.name == itemName)
            {
                //Создаем переменную и кладем в неё объект имя которого совпадает с тем, что нам нужен. (Если такого объекта нет, то переменная примет значение null)
                GameObject putFind = playerItems.Find(x => x.name == itemName);
                //Если мы такой объект еще не доставали из инвентаря, то
                if (putFind == null)
                {
                    if(itemInArm != null)
                    {
                        itemInArm.SetActive(false);
                    }
                    //Спавним этот объект из папки Resources на сцену в точку itemPoint(в данном случае эта точка на правой руке)
                    var newItem = Instantiate(resourceItem.gameObject, itemPoint);
                    //Перемещаем этот объект в иерархию игрока
                    newItem.transform.parent = itemPoint;
                    //Даем ему имя 
                    newItem.name = itemName;
                    //Добавляем в список предметов игрока этот объект
                    playerItems.Add(newItem);
                    //Теперь говорим Юнити, что переменная itemInArm = этому объекту(то есть запоминаем то, что у нас сейчас взято из инвентаря)
                    itemInArm = newItem;
                }
                //Иначе если предмет, который мы хотим достать из инветаря уже взят игроком, то..
                else if (putFind.name == itemInArm.name)
                {
                    //Включаем его или отключаем
                    putFind.SetActive(!putFind.activeSelf);
                }
                //В остальных случаях, если предмет, который мы хотим достать уже был заспавнен на сцену и если он не совпадает с тем, что у нас взято сейчас, то..
                else
                {
                    //отключаем предмет, который у нас сейчас в руках
                    itemInArm.SetActive(false);
                    //включаем предмет, который хотим взять
                    putFind.SetActive(true);
                    //Теперь говорим Юнити, что переменная itemInArm = этому объекту(то есть запоминаем то, что у нас сейчас взято из инвентаря)
                    itemInArm = putFind;
                }
            }
        }
    }
}
