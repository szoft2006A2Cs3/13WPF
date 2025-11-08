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
using MySqlX.XDevAPI.Common;

namespace BrckettAdminApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBaseAccessor db;
        private GetMetaData gmd;
        private Table ? CurrentTable;


        //
        // A "brckett without constraints" adatbázisra lett tervezve a program
        //
        public MainWindow()
        {
            InitializeComponent();

            db = new DataBaseAccessor();
            //db = new DataBaseAccessor();
            gmd = new GetMetaData(db);
            UIGeneration.GenerateMUI(gmd,this, TablesMenu);
            
            // Need an initialize method, and the rest in GetMetaData.cs
        }

        private void Createbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTable == null)
            {
                MessageBox.Show("Select a Table first!");
            }
            else 
            {
                UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
                DetailsListBox.IsEnabled = false;
                TablesMenu.IsEnabled = false;
                Createbtn.Visibility = Visibility.Hidden;
                Deletebtn.Visibility = Visibility.Hidden;
                Updatebtn.Visibility = Visibility.Hidden;
                CreateOkbtn.Visibility = Visibility.Visible;
                CreateCancelbtn.Visibility = Visibility.Visible;

                foreach (var item in DetailedViewStuff.Children)
                {
                    if (item.GetType() == typeof(TextBox))
                    {
                        TextBox textBox = (TextBox)item;
                        textBox.IsEnabled = true;
                    }
                }
            }
        }
        private void CCancelbtn_Click(object sender, RoutedEventArgs e)
        {
            UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
            DefaultView();
        }
        private void COKbtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> insertData = new List<string>();
            foreach (var item in DetailedViewStuff.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox)item;
                    insertData.Add(textBox.Text);
                }
            }

            string result = "";
            try
            {
                 result = db.InsertInto(CurrentTable, insertData);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            gmd.GetAllMetaData();
            UIGeneration.GenerateLBUI(CurrentTable, db, DetailsListBox);
            UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
            //MessageBox.Show(result);
            DefaultView();
        }
        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)DetailsListBox.SelectedItem;
            if (selectedItem != null)
            {
                DetailsListBox.IsEnabled = false;
                TablesMenu.IsEnabled = false;
                Createbtn.Visibility = Visibility.Hidden;
                Deletebtn.Visibility = Visibility.Hidden;
                Updatebtn.Visibility = Visibility.Hidden;
                UpdateOkbtn.Visibility = Visibility.Visible;
                UpdateCancelbtn.Visibility = Visibility.Visible;

                foreach (var item in DetailedViewStuff.Children)
                {
                    if (item.GetType() == typeof(TextBox))
                    {
                        TextBox textBox = (TextBox)item;
                        textBox.IsEnabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select An Element Before Updating");
            }
            
        }
        private void UCancelbtn_Click(object sender, RoutedEventArgs e)
        {
            UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
            DefaultView();
        }
        private void UOKbtn_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)DetailsListBox.SelectedItem;
            Dictionary<string,string> tempInput = new Dictionary<string,string>();
            int i = 0;
            foreach (var item in DetailedViewStuff.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox)item;
                    tempInput.Add(CurrentTable.FieldNames.Keys.ToList()[i], textBox.Text);
                    i++;
                }
                
            }

            db.Update(CurrentTable,$"{selectedItem.Content}",tempInput);
            gmd.GetAllMetaData();
            UIGeneration.GenerateLBUI(CurrentTable, db, DetailsListBox);
            UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
            DefaultView();
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
                ListBoxItem selectedItem = (ListBoxItem)DetailsListBox.SelectedItem;
            if (selectedItem != null)
            {
                string result = db.Delete(CurrentTable, $"{selectedItem.Content}");
                MessageBox.Show(result);
            }
            else 
            {
                MessageBox.Show("Please Select An Element You Would Like To Delete!");
            }
            UIGeneration.GenerateLBUI(CurrentTable, db, DetailsListBox);
            UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
        }
        private void DefaultView() 
        {
            DetailsListBox.IsEnabled = true;
            TablesMenu.IsEnabled = true;
            Createbtn.Visibility = Visibility.Visible;
            Deletebtn.Visibility = Visibility.Visible;
            Updatebtn.Visibility = Visibility.Visible;
            CreateCancelbtn.Visibility = Visibility.Hidden;
            CreateOkbtn.Visibility = Visibility.Hidden;
            UpdateOkbtn.Visibility = Visibility.Hidden;
            UpdateCancelbtn.Visibility = Visibility.Hidden;
            foreach (var item in DetailedViewStuff.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox)item;
                    textBox.IsEnabled = false;
                }
            }
        }

        private void LogOutbtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DetailsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox _tempCheck = (ListBox)sender;
            if (_tempCheck.Items.Count != 0)
            { 
                ListBoxItem selected = (ListBoxItem)DetailsListBox.SelectedItem;

                //                    CHANGE THIS
                Table currentTable = CurrentTable;
            


                var CurrentTableData = db.Read(currentTable.TableName, currentTable.pkFieldName);

                foreach (var _fieldName in currentTable.FieldNames.Keys) 
                {
                    TextBox temp = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, _fieldName);
                    temp.Text = CurrentTableData[$"{selected.Content}"][_fieldName];
                }
            }
        }
        public void OnMenuElementClick(MenuItem _sender, RoutedEventArgs _event) 
        {
            CurrentTable = gmd.Tables.Where(t => t.TableName == _sender.Header.ToString()).First();
            //MessageBox.Show(CurrentTable.TableName);
            UIGeneration.GenerateLBUI(CurrentTable, db, DetailsListBox);
            UIGeneration.GenerateDWUI(CurrentTable, db, DetailedViewStuff);
        }
    }
}