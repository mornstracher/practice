using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
   public static class Utilitly
    {
      
       public static string RemoveXDR(string stockName)
       {
           return stockName.Replace("X", "").Replace("D", "").Replace("R", "");
       }
    }
}
