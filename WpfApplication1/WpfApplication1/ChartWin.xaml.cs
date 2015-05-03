using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for ChartWin.xaml
    /// </summary>
    public partial class ChartWin : Window
    {
        public ChartWin()
        {
            InitializeComponent();
        }

        public void SetDataSource(List<StockProfit> lstStockProfit)
        {
            this._reportViewer.LocalReport.DataSources.Clear();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
                Microsoft.Reporting.WinForms.ReportDataSource();
            this._reportViewer.LocalReport.ReportPath = "ByStock.rdlc";
            reportDataSource1.Name = "lstStockProfit";
            //Name of the report dataset in our .RDLC file
            reportDataSource1.Value = lstStockProfit;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            
          
            _reportViewer.RefreshReport();
        }
    }
}
