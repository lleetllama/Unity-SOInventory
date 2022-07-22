using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Sirenix.OdinInspector;

namespace SanguineSyntax
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public delegate void OnItemChange();
        public OnItemChange onItemChange = delegate { };

        string save_path;
        public string gameSaveID; // needs set from a gamesave manager.

        [SerializeField]
        ItemDatabase database;

        BinaryFormatter formatter = new BinaryFormatter();

        [SerializeField]
        List<Inventory> persistedInventory = new List<Inventory>();

        [SerializeField]
        List<Inventory> garbageStaging = new List<Inventory>();

        void Awake()
        {
            save_path = Application.persistentDataPath + "/" + gameSaveID;
        }

        /// <summary>
        /// save inventories, purge all dead/orphan inventories
        /// </summary>
        /// <param name="gameSaveID"></param>
        [Button]
        public void SaveAllInventory()
        {
            Directory.CreateDirectory(save_path);

            foreach (Inventory inv in persistedInventory)
            {
                SaveInventory(inv);
            }
            // purge all dead/orphaned inventories since last save
            foreach (Inventory inv in garbageStaging)
            {
                DeleteInventory(inv);
            }
        }

        /// <summary>
        /// "check in" an inventory for tracking
        /// </summary>
        /// <param name="inventory"></param>
        public void CheckInInventory(Inventory inventory)
        {
            persistedInventory.Add(inventory);
        }

        /// <summary>
        /// Write inventory to file
        /// </summary>
        /// <param name="gameSaveID">the id designated for the game save</param>
        /// <param name="inventory">the inventory being saved</param>
        void SaveInventory(Inventory inventory)
        {
            string path = save_path + "/" + inventory.GetID() + ".inv";
            FileStream stream = new FileStream(path, FileMode.Create);
            InventoryData data = new InventoryData(inventory);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>
        /// Load Inventory from file
        /// </summary>
        /// <param name="inventory"></param>
        public void LoadInventory(Inventory inventory)
        {
            string path = save_path + "/" + inventory.GetID() + ".inv";

            FileStream stream = new FileStream(path, FileMode.Open);

            inventory.DestructiveClearInventory();

            InventoryData data = (InventoryData)formatter.Deserialize(stream);
            stream.Close();

            foreach (int itemID in data.ItemIDs)
            {
                inventory.AddItem(database.FetchItemByID(itemID));
            }
        }

        [Button]
        public bool InventoryHasSave(Inventory inventory)
        {
            string path = save_path + "/" + inventory.GetID() + ".inv";
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete an inventory file. Remove it from tracking
        /// </summary>
        /// <param name="gameSaveID"></param>
        /// <param name="inventory"></param>
        void DeleteInventory(Inventory inventory)
        {
            string path = save_path + "/" + inventory.GetID() + ".inv";
            File.Delete(path);
            garbageStaging.Remove(inventory);
        }
    }
}

namespace SanguineSyntax
{
    [System.Serializable]
    public class InventoryData
    {
        public int[] ItemIDs;

        public InventoryData(Inventory inventory)
        {
            ItemIDs = inventory.ItemArray().Select(item => item.itemID).ToArray();
        }
    }
}
