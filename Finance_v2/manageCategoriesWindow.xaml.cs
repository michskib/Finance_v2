using System;
using System.Collections.Generic;
using System.Data;
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

namespace Finance_v2
{
    /// <summary>
    /// Interaction logic for manageCategoriesWindow.xaml
    /// </summary>
    public partial class manageCategoriesWindow : Window
    {
        public manageCategoriesWindow()
        {
            InitializeComponent();
        }

        private void manageCategoriesWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            loadCategoryDataGrid();
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            addNewCategoryWindow window = new addNewCategoryWindow();
            window.ShowDialog();
        }

        private void loadCategoryDataGrid()
        {
            balanceDatabaseOperationsClass balanceDatabaseOperations = new balanceDatabaseOperationsClass();
            DataTable table = balanceDatabaseOperations.getCategoryTable();

            categoryDataGrid.ItemsSource = table.DefaultView;
        }

        public void refresh()
        {
            loadCategoryDataGrid();
        }

        private void manageCategoriesWindow1_Activated(object sender, EventArgs e)
        {
            refresh();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                balanceDatabaseOperationsClass balanceDatabaseOperations = new balanceDatabaseOperationsClass();
                DataRowView row = categoryDataGrid.SelectedItem as DataRowView;
                string categoryName = row["CategoryName"].ToString();
                balanceDatabaseOperations.deleteCategory(categoryName);
            }
            catch(ArgumentException exception)
            {
                MessageBox.Show("You cannot delete 'Other' category");
            }
            catch(NullReferenceException exception)
            {
                MessageBox.Show("You have not checked anything to delete");
            }

            refresh();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
