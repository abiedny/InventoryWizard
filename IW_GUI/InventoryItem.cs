using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW_GUI
{
    public class InventoryItem
    {
        public string purchaseOrder { get; set; }
        public string type { get; set; }
        public string serviceTag { get; set; }
        public string price { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string purchaseDate { get; set; }
        public string status { get; set; }
        public string replacement { get; set; }
        public string staff { get; set; }
        public string role { get; set; }
        public string room { get; set; }
        public string warranty { get; set; }
        public string ip { get; set; }

        public static InventoryItem FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            int i = 0;
            foreach (string element in values)
            {
                values[i] = element.Trim(new[] { '"', '\\' });
                i++;
            }

            InventoryItem item = new InventoryItem
            {
                purchaseOrder = values[1],
                type = values[2],
                serviceTag = values[3],
                price = values[4],
                make = values[5],
                model = values[6],
                purchaseDate = values[7],
                status = values[8],
                replacement = values[9],
                staff = values[10],
                role = values[11],
                room = values[12],
                warranty = values[13],
                ip = values[14]
            };
            return item;
        }
    }
}
