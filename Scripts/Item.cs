using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SanguineSyntax/InventorySystem/Item")]
public class Item : ScriptableObject
{
    new public string name = "Default Item";
    public Sprite icon = null;
    public int itemID;

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
}
