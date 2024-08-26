using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using DataAccessLayer;
using static IT481M2.MainWindow;
namespace IT481M2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initialize the business layer with a connection string.
            CustomerService _customerService = new CustomerService("Server=DESKTOP-3M0TI8O\\SQLEXPRESS;Database=Northwind;Integrated Security=True;TrustServerCertificate=true");

            // Display customer count in a label.
            int customerCount = _customerService.GetCustomerCount();
            CustomerCountTextBlock.Text = $"Total Customers: {customerCount}";

            // Display customer names in a ListBox.
            var customerNames = _customerService.GetCustomerNames();
            foreach (var name in customerNames)
            {
                CompanyNamesListBox.Items.Add(name);
            }
        }

        public class CustomerService
        {
            private readonly CustomerRepository _repository;

            // Constructor that initializes the CustomerRepository with a connection string.
            public CustomerService(string connectionString)
            {
                _repository = new CustomerRepository(connectionString);
            }

            // Method to get the total count of customers.
            public int GetCustomerCount()
            {
                return _repository.GetCustomerCount();
            }

            // Method to get a list of customer names.
            public List<string> GetCustomerNames()
            {
                return _repository.GetCustomerNames();
            }
        }
    }
}

namespace DataAccessLayer
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        // Constructor that accepts a connection string.
        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to get the total count of customers from the database.
        public int GetCustomerCount()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Customers";
                SqlCommand command = new SqlCommand(query, connection);

                count = (int)command.ExecuteScalar();
            }

            return count;
        }

        // Method to get a list of customer names from the database.
        public List<string> GetCustomerNames()
        {
            List<string> customerNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT CompanyName FROM Customers";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customerNames.Add(reader["CompanyName"].ToString());
                }
            }

            return customerNames;
        }
    }
}