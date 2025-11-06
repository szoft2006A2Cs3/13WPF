using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BrckettAdminApp
{
    class UIGeneration
    {
        public static void GenerateLBUI(Table currentTable, DataBaseAccessor _dba, ListBox parentElement)
        {
            // Implementation for ListBox UI generation
            parentElement.Items.Clear();

            var currentTableData = _dba.Read(currentTable.TableName, currentTable.pkFieldName);
            foreach (var key in currentTableData.Keys) 
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = key;
                parentElement.Items.Add(lbi);
            }
        }
        public static void GenerateDWUI(Table currentTable, DataBaseAccessor _dba, Grid parentElement)
        {
            // Implementation for Detailed View UI generation
            parentElement.Children.Clear();
            parentElement.RowDefinitions.Clear();
            
            for (int i = 0; i< currentTable.FieldNames.Count(); i++)
            {
                RowDefinition rowDef = new RowDefinition();
                parentElement.RowDefinitions.Add(rowDef);
                Label fieldLabel = new Label();
                fieldLabel.Content = currentTable.FieldNames.Keys.ToList()[i];
                parentElement.Children.Add(fieldLabel);
                Grid.SetRow(fieldLabel, i);
                Grid.SetColumn(fieldLabel, 0);
                TextBox fieldTextBox = new TextBox();
                fieldTextBox.IsEnabled = false;
                fieldTextBox.Name = currentTable.FieldNames.Keys.ToList()[i];
                fieldTextBox.Text = "...";
                parentElement.Children.Add(fieldTextBox);
                Grid.SetRow(fieldTextBox, i);
                Grid.SetColumn(fieldTextBox, 1);
            }
        }
        public static void GenerateMUI(GetMetaData _gmd, MainWindow MW, MenuItem parentElement)
        {

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FF7F0086");
            brush.Freeze();
            // Implementation for Menu UI generation
            foreach (var _tableNames in _gmd.TableNames)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = _tableNames;
                menuItem.Background = brush;
                menuItem.FontWeight = FontWeights.Bold;
                menuItem.Click += (s, e) => 
                {
                    MW.OnMenuElementClick((MenuItem)s,e);
                };
                parentElement.Items.Add(menuItem);
            }
        }
    }
}
