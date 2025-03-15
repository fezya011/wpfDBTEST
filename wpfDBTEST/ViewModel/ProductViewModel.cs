using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Input;
using wpfDBTEST.Model;
using wpfDBTEST.VMTools;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wpfDBTEST.ViewModel
{
    public class ProductViewModel : BaseVM
    {
        private string title;
        private string description;
        private decimal price;
        private DateOnly createDate;
        private short expireDate;
        private List<Product> products;
        private Product selectedProduct;

        public string Title 
        { 
            get => title; 
            set
            {
                title = value;
                Signal();
            }
        }
        public string Description 
        { 
            get => description; 
            set 
            { 
                description = value; 
                Signal(); 
            } 
        }

        public decimal Price 
        { 
            get => price; 
            set 
            { 
                price = value; 
                Signal();
            } 
        }
        public DateOnly CreateDate 
        { 
            get => createDate; 
            set 
            { 
                createDate = value; 
                Signal(); 
            } 
        }
        public short ExpireDate 
        { 
            get => expireDate;       
            set 
            {  
               expireDate = value; 
                Signal(); 
            }
        }
        public List<Product> Products 
        { 
            get => products; 
            set 
            { 
                products = value;
                Signal(); 
            } 
        }
        public Product SelectedProduct 
        { 
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                Signal();
            }
        }

        public ICommand Save { get; set; }

        ConnectionDB db;
        public ProductViewModel()
        {
            db = new ConnectionDB();
            db.SetConnection(ProductDB.Instance);
            Products = new List<Product>(ProductDB.Instance.SelectAll());
            Save = new CommandVM(() =>
            {
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description) || Price == 0 || ExpireDate == 0 )
                {
                    MessageBox.Show("Поля заполнены неверно");
                }
                else
                {
                    Product product = new Product
                    {
                        Title = Title,
                        Description = Description,
                        Price = Price,
                        CreateDate = CreateDate,
                        ExpireDate = ExpireDate
                    };
                    ProductDB.Instance.Update(product);
                }
                Products = new List<Product>(ProductDB.Instance.SelectAll());
            });      
        }
    }
}
