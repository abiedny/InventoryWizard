using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW_GUI
{
    public static class Inventory //I think I want this masterlist to all be static?
    {
        private static List<InventoryItem> _inventory;

        public static List<InventoryItem> GetInventory()
        {
            return _inventory;
        }

        public static void ImportInventory(string importFile) //this works
        {
            _inventory = File.ReadAllLines(importFile).Skip(1).Select(v => InventoryItem.FromCsv(v)).ToList();
            //TODO: persistant storage of imported inventory
            return;
        }

        public static InventoryItem CheckServiceTag(string serviceTag)
        {
            foreach (InventoryItem item in _inventory)
            {
                if (item.serviceTag == serviceTag)
                {
                    //servicetag exists in hits
                    return item;
                }
            }
            //If we don't find the item after the whole loop. Not sure if I should return null but w/e who cares best practice is for nerds
            return null;
        }

    }
}
