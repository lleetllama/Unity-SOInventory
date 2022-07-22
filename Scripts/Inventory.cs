using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SanguineSyntax
{
    [System.Serializable]
    public class Inventory : MonoBehaviour
    {
        public delegate void OnItemChange();
        public OnItemChange onItemChange = delegate { };
        string entityID;

        [SerializeField]
        List<Item> inventoryItems = new List<Item>();

        private void Start()
        {
            InventoryManager.Instance.CheckInInventory(this);

            if (InventoryManager.Instance.InventoryHasSave(this))
            {
                InventoryManager.Instance.LoadInventory(this);
            }
        }

        public void SetOwnerID(string owner)
        {
            entityID = owner;
        }

        public string GetID()
        {
            return entityID;
        }

        public int ItemCount()
        {
            return inventoryItems.Count;
        }

        public void AddItem(Item item)
        {
            inventoryItems.Add(item);
            onItemChange.Invoke();
        }

        public void RemoveItem(Item item)
        {
            inventoryItems.Remove(item);
            onItemChange.Invoke();
        }

        public void DestructiveClearInventory()
        {
            foreach (Item item in inventoryItems)
            {
                RemoveItem(item);
            }
        }

        public Item[] ItemArray()
        {
            return inventoryItems.ToArray();
        }
    }
}
