using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfDBTEST.Model
{
    public class SupplierDB
    {
        ConnectionDB connection;

        private SupplierDB(ConnectionDB db)
        {
            this.connection = db;
        }


        public bool Insert(Supplier Supplier)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `suppliers` Values (0, @title, @address, @phone);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("title", Supplier.Title));
                cmd.Parameters.Add(new MySqlParameter("address", Supplier.Address));
                cmd.Parameters.Add(new MySqlParameter("phone", Supplier.Phone));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        Supplier.Id = id;
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
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Supplier> SelectAll()
        {
            List<Supplier> suppliers = new List<Supplier>();
            if (connection == null)
                return suppliers;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `title`, `phone`, `address` from `suppliers` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("title");
                        string address = string.Empty;
                        if (!dr.IsDBNull(2))
                            address = dr.GetString("address");
                        string phone = string.Empty;
                        if (!dr.IsDBNull(3))
                            phone = dr.GetString("phone");
    
                        suppliers.Add(new Supplier
                        {
                            Id = id,
                            Title = title,
                            Address = address,
                            Phone = phone,
                            

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return suppliers;
        }

        internal bool Update(Supplier edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `suppliers` set `title`=@title, `address`=@address , `phone`=@phone where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("address", edit.Address));
                mc.Parameters.Add(new MySqlParameter("phone", edit.Phone));
                
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Supplier remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `supppliers` where `id` = {remove.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static SupplierDB db;
        public static SupplierDB GetDb()
        {
            if (db == null)
                db = new SupplierDB(ConnectionDB.GetDbConnection());
            return db;
        }
    }

}
