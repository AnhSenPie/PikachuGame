
using UnityEngine;

public static class ItemData
{
    public static Item[] Items { get; private set; }
    private static Item[] Item1;
    private static Item[] Item2;
    

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {    
        Item1 = Resources.LoadAll<Item>("lolItems/");  
        Item2 = Resources.LoadAll<Item>("leafItems/");
    }
    public static void getItems(int index)
    {
        switch (index)
        {
            case 0:
                Items = Item1;
                break;
                case 1:
                Items = Item2;
                break;
            default:
                break;
        }
    }
}
