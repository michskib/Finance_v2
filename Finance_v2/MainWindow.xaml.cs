using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace Finance_v2
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

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fill_transactionDataGrid();
        }

        private void manageCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            manageCategoriesWindow window = new manageCategoriesWindow();
            window.ShowDialog();
        }

        private void addTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            addTransactionWindow window = new addTransactionWindow();
            window.ShowDialog();
        }

        private void fill_transactionDataGrid()
        {
            balanceDatabaseOperationsClass balanceDatabaseOperations = new balanceDatabaseOperationsClass();
            DataTable table = balanceDatabaseOperations.getTransactionTable();

            transactionDataGrid.ItemsSource = table.DefaultView;
        }

        private void mainWindow_Activated(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            fill_transactionDataGrid();
        }
    }
}
