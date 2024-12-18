using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using EpihinVar12.images;
using Npgsql;

namespace EpihinVar12
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public int checkkForLogin()
        {
            string username = loginTextBox.Text;
            string password = passwordBox.Password;

            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                string query = "SELECT id FROM Users WHERE Login = @login AND Password = @password;";
                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("login", username);
                    command.Parameters.AddWithValue("password", password);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Извлекаем значение id
                            TempUserData.Id = reader.GetInt32(0);
                            return reader.GetInt32(0); // Предполагаем, что id — это BIGINT (long)
                        }
                        else
                        {
                            // Если пользователь не найден
                            return 0;
                        }
                    }
                }

            }
            
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                checkkForLogin();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Error: {ex.Message}");
            }



            if (checkkForLogin() > 0) 
            {
                Window1 rewardsWindow = new Window1();
                rewardsWindow.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Неверные учетные данные. Попробуйте еще раз.");
            }
        }
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;UserId=postgres;Password=12345;Database=Var12DB");
        }
        //public void Connection()
        //{
        //    using (NpgsqlConnection con = GetConnection())
        //    {

        //        con.Open();
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            MessageBox.Show("Все гуд");
        //        }

        //        else
        //        {
        //            MessageBox.Show("sfjoafjuioawsfjnlko");
        //        }
        //    }
        //}
    }
}
