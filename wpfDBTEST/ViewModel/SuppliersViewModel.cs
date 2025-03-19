using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using wpfDBTEST.VMTools;
using System.Threading.Tasks;
using wpfDBTEST.Model;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using wpfDBTEST.View;

namespace wpfDBTEST.ViewModel
{
    public class SuppliersViewModel : BaseVM
    {
        
        private string title;
        private string address;
        private string phone;
        private Supplier selectedSupplier;
        private List<Supplier> suppliers;

        public string Title 
        { 
            get => title;
            set
            {
                title = value;
                Signal();
            }
        }
        public string Address 
        { 
            get => address;
            set
            {
                address = value; 
                Signal();
            }
        }

        public string Phone 
        { 
            get => phone;
            set
            {
                phone = value; 
                Signal(); 
            }
        }
        public Supplier SelectedSupplier 
        { 
            get => selectedSupplier; 
            set
            {
                selectedSupplier = value;
                Signal();
            }
        }
        public List<Supplier> Suppliers 
        { 
            get => suppliers; 
            set
            {
                suppliers = value;
                Signal();
            }
        }

        public ICommand Save { get; set; }
        public ICommand Remove { get; set; }
        public ICommand Edit { get; set; }

        ConnectionDB db;
        public SuppliersViewModel()
        {
            SelectAll();

            Save = new CommandVM(() =>
            {
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Phone))
                {
                    MessageBox.Show("Поля заполнены неверно");
                }
                else
                {
                    Supplier supplier = new Supplier
                    {
                        Title = Title,
                        Address = Address,
                        Phone = Phone,
                    };
                    SupplierDB.GetDb().Insert(supplier);
                }
                SelectAll();
            }, () => true);

            Remove = new CommandVM(() =>
            {
                SupplierDB.GetDb().Remove(SelectedSupplier);
                SelectAll();
            }, () => SelectedSupplier != null);

            Edit = new CommandVM(() =>
            {
                if (SelectedSupplier != null)
                {
                    SelectedSupplier.Address = Address;
                    SelectedSupplier.Title = Title;
                    SelectedSupplier.Phone = Phone;                 
                }
                else
                {
                    MessageBox.Show("ОШИБКА");
                }
                SupplierDB.GetDb().Update(SelectedSupplier);
                MessageBox.Show("Успешно");
                SelectAll();
            }, () => SelectedSupplier != null);
          
        }

        private void SelectAll()
        {
            Suppliers = new List<Supplier>(SupplierDB.GetDb().SelectAll());
        }
    }
}
