using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        List<FoundsFlow> lstFoundsFlow = new List<FoundsFlow>();
        List<Position> lstPosition = new List<Position>();
        decimal curVal = 0m;
        decimal totalProfit = 0m;
        decimal totalCost = 0m;
        decimal total = 0m;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            LoadDataFromFile();
            CalculateTotalCost();

        }

        Dictionary<string, decimal> dicStockProfit;
        Dictionary<string, decimal> dicPosition;
        private void SumaryByStock()
        {
            dicStockProfit = new Dictionary<string, decimal>();
            foreach (var ff in this.lstFoundsFlow)
            {
                if (ff.Description.Contains("证券买入") || ff.Description.Contains("证券卖出"))
                {
                    string stockName = ff.Description.Substring(4, ff.Description.Length - 4);
                    stockName = Utilitly.RemoveXDR(stockName);

                    stockName = this.SearchStock(stockName);
                    dicStockProfit[stockName] += ff.Total;
                }
            }

            this.ListPositions();
            foreach (var p in this.dicPosition)
            {
                string stockName = p.Key;
                stockName = SearchStock(stockName);
                dicStockProfit[stockName] += p.Value;
            }

            this.txtResult.Text += SEPARATOR;
            this.txtResult.Text += SEPARATOR;
            totalProfit = 0m;
            foreach (var t in dicStockProfit.OrderBy(kvp=>kvp.Value))
            {
                totalProfit += t.Value;
                this.txtResult.Text += GetLineForTwoFields(t.Key, t.Value);// string.Format(FORMAT_TEMPLATE_TWO_FIELDS, t.Key.PadRight(16, SPACE_HAN), t.Value);
            }
            this.txtResult.Text += SEPARATOR;
            this.txtResult.Text += GetLineForTwoFields("总利润", totalProfit);//string.Format(FORMAT_TEMPLATE_TWO_FIELDS, "总利润".PadRight(16, SPACE_HAN), (totalProfit + this.curVal));
            this.txtResult.Text += SEPARATOR;
            this.total = totalCost + this.totalProfit;
            this.txtResult.Text += GetLineForTwoFields("共计", this.total);
        }

        private string SearchStock(string stockName)
        {
            if (!dicStockProfit.ContainsKey(stockName))
            {
                bool found = false;
                foreach (var key in dicStockProfit.Keys)
                {
                    if (key.Contains(stockName.Substring(0, 2)))
                    {
                        stockName = key;
                        found = true;
                    }
                }
                if (!found)
                    dicStockProfit.Add(stockName, 0m);
            }
            return stockName;
        }

        private string GetLineForTwoFields(string fieldName, decimal fieldValue)
        {
            return string.Format(FORMAT_TEMPLATE_TWO_FIELDS, fieldName.PadRight(16, SPACE_HAN), fieldValue);
        }

        private void LoadDataFromFile()
        {
            if (!Directory.Exists(this.txtFileBox.Text))
                MessageBox.Show("路径不存在");
            lstFoundsFlow = new List<FoundsFlow>();
            this.txtResult.Text = "";
            foreach (string file in Directory.GetFiles(this.txtFileBox.Text, "*.txt", SearchOption.TopDirectoryOnly))
            {
                // Encoding.UTF8.GetBytes("对账单")
                //  if (string.Compare(System.IO.Path.GetFileName(file).Substring(0,3), "对账单", StringComparison.CurrentCulture) != 0) continue;
                foreach (string line in File.ReadLines(file, Encoding.Default))
                {
                    //foundation flow record
                    if (line.Length > 4 && line.Substring(0, 3) == "201")
                    {
                        var ff = FoundsFlow.TryParse(line);
                        if (ff != null)
                        {
                            lstFoundsFlow.Add(ff);
                        }
                    }
                    int pos = line.IndexOf("市值:");
                    if (pos > 0)
                    {
                        string strCurVal = line.Substring(pos + 3).Trim();
                        decimal.TryParse(strCurVal, out this.curVal);
                    }
                    //position record
                    if (line.Length > 10 && line.Substring(0, 10).Trim() == SH_ACCOUNT_NO)
                    {
                        Position p = Position.TryParse(line);
                        if (p != null)
                        {
                            lstPosition.Add(p);
                        }
                    }
                }
            }
        }

        const string SEPARATOR = "---------------------------------------------------------\n";
        const char SPACE_HAN = '\u3000';
        const string FORMAT_TEMPLATE_TWO_FIELDS = "         {0} {1,15:N2} \n";
        const string FORMAT_TEMPLATE_THREE_FIELDS = "{0:yyyyMMdd} {1} {2,15:N2} \n";
        const string SH_ACCOUNT_NO = "A403409969";
        const string SZ_ACCOUNT_NO = "0152854725";
        private void CalculateTotalCost()
        {
            totalCost = 0;
            foreach (var ff in this.lstFoundsFlow)
            {
                if (ff.Description.Contains("银行转存") || ff.Description.Contains("银行转取")
                    || ff.Description.Contains("资金调拨调入") || ff.Description.Contains("资金调拨调出"))
                {
                    totalCost += ff.Total;
                    this.txtResult.Text += string.Format(FORMAT_TEMPLATE_THREE_FIELDS, ff.Date, ff.Description.PadRight(16, SPACE_HAN), ff.Total);
                }
            }

            this.txtResult.Text += SEPARATOR;
            this.txtResult.Text += GetLineForTwoFields("总成本", totalCost);// string.Format(FORMAT_TEMPLATE_TWO_FIELDS, "总成本".PadRight(16, SPACE_HAN), totalCost.ToString());
        }

        private void btnByStock_Click(object sender, RoutedEventArgs e)
        {
            this.SumaryByStock();
        }

        private void btnPositions_Click(object sender, RoutedEventArgs e)
        {
            ListPositions();
        }

        private void ListPositions()
        {
            this.txtResult.Text += SEPARATOR;
            curVal = 0m;
            dicPosition = new Dictionary<string, decimal>();
            foreach (var p in this.lstPosition)
            {
                if (!dicPosition.ContainsKey(p.ProductName))
                {
                    dicPosition.Add(p.ProductName, p.Total);
                    curVal += p.Total;
                    this.txtResult.Text += GetLineForTwoFields(p.ProductName, p.Total);
                }
            }
            this.txtResult.Text += SEPARATOR;
            this.txtResult.Text += GetLineForTwoFields("总市值", curVal);
        }

        private void btnTimeSplit_Click(object sender, RoutedEventArgs e)
        {
            var query = from q in this.lstFoundsFlow
                        where q.Description.Contains("证券买入") || q.Description.Contains("证券卖出")
                        group q by new { StockName = q.Description.Substring(4, q.Description.Length - 4), TransactionDate =new DateTime(q.Date.Year,q.Date.Month,1) } into g
                        select new { StockName = g.Key.StockName, TransactionDate = g.Key.TransactionDate, Amount =g.Sum(q=>q.Total)};
             //var gq=from st in query
             //       group st by new {Month=st.TransactionDate,st.StockName
        }

    }
}
