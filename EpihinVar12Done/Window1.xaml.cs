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
using EpihinVar12.images;
using Npgsql;
namespace EpihinVar12
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    
    public partial class Window1 : Window
    {
        public decimal bonusnum ;
        public Window1()
        {
            InitializeComponent();
            //LoadOffers();
            GetAllProducts();
            GetUserBonuses();
            Bonuses.Content = "Количество бонусов: " + bonusnum.ToString();
        }
        
        private static NpgsqlConnection Getcon()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;UserId=postgres;Password=12345;Database=Var12DB");
        }
        private List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var con = Getcon())
            {
                con.Open();

                string query = "SELECT ProductID, ProductName, OldPrice, NewPrice, Description, ImageUri FROM Products;";
                using (var command = new NpgsqlCommand(query, con))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            ProductID = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            OldPrice = reader.GetDecimal(2),
                            NewPrice = reader.GetDecimal(3),
                            Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ImageUri = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                        products.Add(product);
                    }
                }
            }
            OffersItemsControl.ItemsSource = products;
            return products;
        }

        public void GetUserBonuses()
        {
            using (var con = Getcon())
            {
                con.Open();

                string query = "SELECT Bonuses FROM users WHERE id = @id;";
                using (var command = new NpgsqlCommand(query, con))
                {
                    // Устанавливаем параметр перед выполнением команды
                    command.Parameters.AddWithValue("id", TempUserData.Id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Если данные найдены
                        {
                            bonusnum = reader.GetInt32(0); // Читаем значение бонусов
                        }
                        
                    }
                }
            }
        }

        public void UpdateUserBonuses()
        {
            using (var con = Getcon())
            {
                con.Open();

                string query = "UPDATE Users SET Bonuses = @bonusnum WHERE id = @id;";

                using (var command = new NpgsqlCommand(query, con))
                {
                    // Добавляем параметры
                    command.Parameters.AddWithValue("id", TempUserData.Id);
                    command.Parameters.AddWithValue("bonusnum", bonusnum);

                    // Выполняем команду
                    command.ExecuteNonQuery();
                }
            }
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            // Получаем контекст данных из кнопки
            GetUserBonuses();
            Button buyButton = sender as Button;
            if (buyButton?.DataContext is Product product)
            {
                // Вычисляем бонусы: допустим, 10% от новой цены
                bonusnum += product.NewPrice * 0.10m;
                Bonuses.Content = "Количество бонусов: " + string.Format("{0:0.00}", bonusnum.ToString()); 
                // Формируем сообщение
                string message = $"Покупка успешна!\nТовар: {product.ProductName}\n" +
                                 $"Новая цена: {product.NewPrice} руб.\n" +
                                 $"Бонусы начислены: {bonusnum:F2} руб.";

                // Выводим сообщение пользователю
                MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateUserBonuses();
            }
        }

        //private void LoadOffers()
        //    {
        //        var products = new List<Product>
        //        {
        //            new Product { ProductName = "Товар 1", OldPrice = 1200, NewPrice = 800, Description = "Описание товара 1" },
        //            new Product { ProductName = "Товар 2", OldPrice = 1500, NewPrice = 900, Description = "Описание товара 2" },
        //            new Product { ProductName = "Товар 3", OldPrice = 2000, NewPrice = 1500, Description = "Описание товара 3" },
        //        };

        //        
        //        



        //    }

        private void CouponButton_Click(object sender, RoutedEventArgs e)
        {
            Coupon CouponWindow = new Coupon();
            CouponWindow.Show();
            this.Hide();
        }
    }
}
