using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Finance_v2
{
    class balanceDatabaseOperationsClass
    {
        #region Public Methods

        public DataTable getTransactionTable()
        {

            DataTable transactionTable = new DataTable();
            SqlConnection connection = getSqlConnection();

            string sqlText = "SELECT * FROM transactionTable;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection);
            adapter.Fill(transactionTable);

            transactionTable = handleCategoryRow(transactionTable);
            connection.Close();

            return transactionTable;
        }

        public DataTable getCategoryTable()
        {

            DataTable categoryTable = new DataTable();
            SqlConnection connection = getSqlConnection();

            string sqlText = "SELECT * FROM categoryTable;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection);
            adapter.Fill(categoryTable);
            connection.Close();

            return categoryTable;
        }

        public void addCategory(string categoryName)
        {
            chceckIfCategoryNameValid(categoryName);

            string sqlText = "INSERT INTO categoryTable (CategoryName, IncomeValue, ExpenseValue) " +
                                                           "VALUES ('" + categoryName +"', 0, 0);";

            executeSqlText(sqlText);
        }

        public void deleteCategory(string categoryName)
        {
            if (categoryName == "Other") throw new ArgumentException();
            handleDeletingCategory(categoryName);
            string sqlText = "DELETE FROM categoryTable WHERE CategoryName = '" + categoryName + "';";
            executeSqlText(sqlText);
        }

        #endregion

        #region Private Methods

        private void handleDeletingCategory(string categoryName)
        {
            handleIncomeAndExpenseValue(categoryName);
            handleRecordsCategory(categoryName);
        }

        private void handleIncomeAndExpenseValue(string categoryName)
        {
            DataTable categoryTable = getCategoryTable();
            int incomeValue = -1;
            int expenseValue = -1;
            int otherIincomeValue = -1;
            int otherExpenseValue = -1;
            foreach (DataRow currentRow in categoryTable.Rows)
            {
                if (currentRow["CategoryName"].ToString() == categoryName)
                {
                    incomeValue = Convert.ToInt32(currentRow["IncomeValue"]);
                    expenseValue = Convert.ToInt32(currentRow["ExpenseValue"]);
                }
                else if (currentRow["CategoryName"].ToString() == "Other")
                {
                    otherIincomeValue = Convert.ToInt32(currentRow["IncomeValue"]);
                    otherExpenseValue = Convert.ToInt32(currentRow["ExpenseValue"]);
                }
            }

            otherIincomeValue += incomeValue;
            otherExpenseValue += expenseValue;

            string sqlText = "UPDATE categoryTable SET IncomeValue = " + otherIincomeValue.ToString()
                + ", ExpenseValue = " + otherExpenseValue.ToString() + " WHERE CategoryName = '" + categoryName + "';";

            executeSqlText(sqlText);
        }

        private void handleRecordsCategory(string categoryName)
        {
            List<string> categoryNameList = getCategoryNameList();
            int categoryId = categoryNameList.IndexOf(categoryName) + 1;

            string sqlText = "UPDATE transactionTable SET CategoryId = 1 WHERE CategoryId = " + categoryId.ToString() + " ;";
            executeSqlText(sqlText);
        }

        private void chceckIfCategoryNameValid(string categoryNameToCheck)
        {
            List<string> categoryNameList = getCategoryNameList();

            foreach (string currentCategoryName in categoryNameList)
            {
                if (currentCategoryName.ToUpper() == categoryNameToCheck.ToUpper()) throw new ArgumentException();
            }
        }

        private DataTable handleCategoryRow(DataTable transactionTable)
        {
            List<string> categoryNameList = getCategoryNameList();
            transactionTable.ConvertColumnType("CategoryId", typeof(string));
            int categoryId;

            foreach (DataRow currentRow in transactionTable.Rows)
            {
                categoryId = Convert.ToInt32(currentRow["CategoryId"]);
                currentRow["CategoryId"] = categoryNameList[categoryId - 1];
            }

            return transactionTable;
        }

        private List<string> getCategoryNameList()
        {
            DataTable categoryTable = getCategoryTable();

            List<string> categoryNameList = new List<string>();

            string categoryName;

            foreach(DataRow currentRow in categoryTable.Rows)
            {
                categoryName = currentRow["CategoryName"].ToString();
                categoryNameList.Add(categoryName);
            }

            return categoryNameList;
        }

        #endregion

        #region Base Private Methods

        private void executeSqlText(string sqlText)
        {
            SqlConnection connection = getSqlConnection();
            SqlCommand command = new SqlCommand(sqlText, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private SqlConnection getSqlConnection()
        {
            string connectionString = Properties.Settings.Default.BalanceDatabaseConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open) connection.Open();
            return connection;
        }

        #endregion
    }
}