using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfDBTEST.Model
{
    class ProductDB
    {
        private static ProductDB instance;
        public static ProductDB Instance
        {
            get
            {
                if (instance == null)
                    instance = new ProductDB();
                return instance;
            }
        }

        MySqlConnection connection;

        internal void SetConnection(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public bool Insert(Product Product)
        {
            bool result = false;
            if (connection == null)
                return result;

            connection.Open();

            MySqlCommand cmd = new MySqlCommand("insert into `product` Values (0, @title, @description, @price ,@create_date ,@expire_days);select LAST_INSERT_ID();",
                 connection);
            // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
            cmd.Parameters.Add(new MySqlParameter("title", Product.Title));
            cmd.Parameters.Add(new MySqlParameter("description", Product.Description));
            cmd.Parameters.Add(new MySqlParameter("price", Product.Price));
            cmd.Parameters.Add(new MySqlParameter("create_date", Product.CreateDate));
            cmd.Parameters.Add(new MySqlParameter("expire_days", Product.ExpireDate));
      
            try
            {
                // выполняем запрос через ExecuteScalar, получаем id вставленной записи
                // если нам не нужен id, то в запросе убираем часть select LAST_INSERT_ID(); и выполняем команду через ExecuteNonQuery
                int id = (int)(ulong)cmd.ExecuteScalar();
                if (id > 0)
                {
                    MessageBox.Show(id.ToString());
                    // назначаем полученный id обратно в объект для дальнейшей работы
                    Product.ID = id;
                    result = true;
                }
                else
                {
                    MessageBox.Show("Запись не добавлена");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            return result;
        }

        internal List<Product> SelectAll()
        {
            List<Product> Products = new List<Product>();
            if (connection == null)
                return Products;

            connection.Open();
            var command = new MySqlCommand("select `id`, `title`, `description`, `price`, `create_date` ,`expire_days`  from `product` ", connection);
            try
            {
                // выполнение запроса, который возвращает результат-таблицу
                MySqlDataReader dr = command.ExecuteReader();
                // в цикле читаем построчно всю таблицу
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string title = string.Empty; 
                    if (!dr.IsDBNull(1))
                        title = dr.GetString("title");
                    string description = string.Empty;
                    if (!dr.IsDBNull(2))
                        description = dr.GetString("description");
                    decimal price = 0;
                    if (!dr.IsDBNull(3))
                        price = dr.GetInt32("price");
                    DateOnly createDate = new DateOnly();
                    if (!dr.IsDBNull(4))
                        createDate = dr.GetDateOnly("create_date");
                    short expireDate = 0;
                    if (!dr.IsDBNull(5))
                        expireDate = dr.GetInt16("expire_days");
                    Products.Add(new Product
                    {
                        ID = id,
                        Title = title,
                        Description = description,
                        Price = price,
                        CreateDate = createDate,
                        ExpireDate = expireDate,

                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            return Products;
        }

        internal bool Update(Product edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            connection.Open();
            var mc = new MySqlCommand($"update `product` set `title`=@title, `description`=@description , `price`=@price, `create_date`=@create_date , `expire_days`=@expire_days where `id` = {edit.ID}", connection);
            mc.Parameters.Add(new MySqlParameter("title", edit.Title));
            mc.Parameters.Add(new MySqlParameter("description", edit.Description));
            mc.Parameters.Add(new MySqlParameter("price", edit.Price));
            mc.Parameters.Add(new MySqlParameter("create_date", edit.CreateDate));
            mc.Parameters.Add(new MySqlParameter("expire_days", edit.ExpireDate));
        

            try
            {
                mc.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            return result;
        }


        internal bool Remove(Product remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            connection.Open();
            var mc = new MySqlCommand($"delete from `product` where `id` = {remove.ID}", connection);
            try
            {
                mc.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            return result;
        }
    }

}
