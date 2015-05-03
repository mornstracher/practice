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
        public const string FoundsFlow_Pattern = @"(A403409969|0152854725)\s*(\d{6})-(\w*)\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3}\s*){5}\s*(\d*\.\d{2,3})\s*(\d*\.\d{2,3})";
        public static FoundsFlow TryParse(string text)
        {
            List<string> strArray=  Utilitly.ExtractFieldsValue(text);

            if (strArray.Count == FIELDS_COUNT - 1) strArray.Insert(1, "");
            if (strArray.Count != FIELDS_COUNT) return null;

            FoundsFlow ff = new FoundsFlow();
            ff.Date = DateTime.ParseExact(strArray[0],"yyyyMMdd" ,System.Globalization.CultureInfo.CurrentCulture);
            ff.IdentityCode = strArray[1];
            ff.DealerCode = strArray[2];
            ff.Description = strArray[3];
            ff.Quantity = decimal.Parse(strArray[4]);
            ff.Price = decimal.Parse(strArray[5]);
            ff.Fee = decimal.Parse(strArray[6]);
            ff.VAT = decimal.Parse(strArray[7]);
            ff.Total = decimal.Parse(strArray[8]);

            return ff;
        }
    }
}
