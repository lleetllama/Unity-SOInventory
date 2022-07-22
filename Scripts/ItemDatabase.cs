using UnityEngine;

[CreateAssetMenu(
    fileName = "Item Database",
    menuName = "SanguineSyntax/InventorySystem/Item Database"
)]
public class ItemDatabase : ScriptableObject
{
    public Item[] itemArray;

    public Item FetchItemByID(int itemID)
    {
        return itemArray[itemID];
    }

    // This function is called when the script is loaded or a value
    // is changed in the Inspector (Called in the editor only).
    public void OnValidate()
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            itemArray[i].itemID = i;
        }
    }
}
