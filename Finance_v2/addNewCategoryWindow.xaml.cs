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

namespace Finance_v2
{
    /// <summary>
    /// Interaction logic for addNewCategoryWindow.xaml
    /// </summary>
    public partial class addNewCategoryWindow : Window
    {
        public addNewCategoryWindow()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                balanceDatabaseOperationsClass balanceDatabaseOperations = new balanceDatabaseOperationsClass();
                balanceDatabaseOperations.addCategory(categoryNameTextbox.Text);
                this.Close();
            }
            catch(ArgumentException exception)
            {
                MessageBox.Show("Category with this name already exists");
                categoryNameTextbox.Text = "";
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.ToString());
                categoryNameTextbox.Text = "";
            }
        }
    }
}
