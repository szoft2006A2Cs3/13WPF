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

namespace BrckettAdminApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBaseAccessor db;
        private GetMetaData gmd;
        private Table CurrentTable;
        public MainWindow()
        {
            InitializeComponent();
            db = new DataBaseAccessor();
            gmd = new GetMetaData(db);
            CurrentTable = gmd.Tables.First();
            UIGeneration.GenerateLBUI(gmd.Tables[0], db, DetailsListBox);
            UIGeneration.GenerateDWUI(gmd.Tables[0], db, DetailedViewStuff);
            // Need an initialize method, and the rest in GetMetaData.cs
        }

        private void Createbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DetailsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selected = (ListBoxItem)DetailsListBox.SelectedItem;

            //                    CHANGE THIS
            Table currentTable = CurrentTable;
            


            var CurrentTableData = db.Read(currentTable.TableName, currentTable.pkFieldName);

            foreach (var _fieldName in currentTable.FieldNames) 
            {
                TextBox temp = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, _fieldName);
                temp.Text = CurrentTableData[$"{selected.Content}"][_fieldName];
            }
        }
    }








    //HELP FROM THE INTERNET
    //https://stackoverflow.com/questions/636383/how-can-i-find-wpf-controls-by-name-or-type
    public class UIHelper 
    {
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter or null if not found</returns> 
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}