using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFSemana3
{
    public partial class MainWindow : Window
    {
        public class Estudiante
        {
            public string StudentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        private string connectionString = "Data Source=DAVILA-FERNANDO\\SQLEXPRESS;Initial Catalog=DBNET;User Id=Davila;Password=Davila12";
        private DataTable dataTable = new DataTable();

        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    connection.Close();
                }
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                DataView dv = dataTable.DefaultView;
                dv.RowFilter = $"FirstName LIKE '%{searchTerm}%' OR LastName LIKE '%{searchTerm}%'";
                dataGrid.ItemsSource = dv;
            }
            else
            {
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void CargarDataTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable newDataTable = new DataTable();
                    adapter.Fill(newDataTable);
                    connection.Close();
                    MessageBox.Show("Tabla cargada usando data table");
                    dataGrid.ItemsSource = newDataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarDataReader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Estudiante> estudiantes = new List<Estudiante>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int studentId = reader.GetInt32(reader.GetOrdinal("StudentId"));
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        estudiantes.Add(new Estudiante { StudentId = studentId.ToString(), FirstName = firstName, LastName = lastName });
                    }
                    connection.Close();
                }
                MessageBox.Show("Tabla cargada usando data Reader");
                dataGrid.ItemsSource = estudiantes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
