using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApplication1
{
   // 股东代码   证券代码及名称        库存数     当日买入   买入价    当日卖出   卖出价      可用数量            市值  成本价
   public class Position
    {
       public string ProductNo { get; set; }
       public string ProductName { get; set; }
       public decimal Quantity { get; set; }
       public decimal CostPrice { get; set; }
       public decimal Total { get; set; }

       public const string POSITION_PATERN = @"(A403409969|0152854725)\s*(\d{6})-(\w*)\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3}\s*){5}\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3})";
       public static Position TryParse(string text)
       {
          // List<string> strArray = Utilitly.ExtractFieldsValue(text);
           System.Text.RegularExpressions.Match m= System.Text.RegularExpressions.Regex.Match(text, POSITION_PATERN);
           if (m.Groups.Count == 8)
           {
               Position position = new Position();
               position.ProductNo = m.Groups[2].Value;
               position.ProductName = Utility.RemoveXDR(m.Groups[3].Value);
               position.Quantity = decimal.Parse(m.Groups[4].Value);
               position.CostPrice = decimal.Parse(m.Groups[7].Value);
               position.Total = decimal.Parse(m.Groups[6].Value);
               return position;
           }

           return null;
       }

       public static bool IsPositionRecord(string record)
       {
           return Regex.IsMatch(record, Position.POSITION_PATERN);
       }


    }
}
