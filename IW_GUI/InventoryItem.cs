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
                status = values[7],
                replacement = values[8],
                staff = values[9],
                role = values[10],
                room = values[11],
                warranty = values[12],
                ip = values[13]
            };
            return item;
        }
    }
}
