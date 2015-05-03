using System;
using System.Collections.Generic;

namespace WpfApplication1
{
    public class FoundsFlow
    {
        const int FIELDS_COUNT = 9;
        public FoundsFlow()
        {
        }

        public DateTime Date { get; set; }
        public string IdentityCode { get; set; }
        public string DealerCode { get; set; }
        public string Description { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public Decimal Fee { get; set; }
        public Decimal VAT { get; set; }
        public Decimal Total { get; set; }
        public const string FoundsFlow_Pattern = @"(201[4|5]\d{4})\s*(980518597383|A403409969|0152854725|\s{10})\s*(00050[1|3]*)\s*(\w*)\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3})\s*(-?\d*\.\d{2,3})\s*(-?\d*\.\d{2,3})";
        public static FoundsFlow TryParse(string text)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(text, FoundsFlow_Pattern);
            if (m.Groups.Count == 11)
            {
                FoundsFlow ff = new FoundsFlow();
                ff.Date = DateTime.ParseExact(m.Groups[1].Value, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                ff.IdentityCode = m.Groups[2].Value;
                ff.DealerCode = m.Groups[3].Value;
                ff.Description = m.Groups[4].Value;
                ff.Quantity = decimal.Parse(m.Groups[5].Value);
                ff.Price = decimal.Parse(m.Groups[6].Value);
                ff.Fee = decimal.Parse(m.Groups[7].Value);
                ff.VAT = decimal.Parse(m.Groups[8].Value);
                ff.Total = decimal.Parse(m.Groups[9].Value);

                return ff;

            }
          
            return null;
        }
    }
}
