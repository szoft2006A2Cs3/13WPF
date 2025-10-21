using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            
            for (int i = 0; i< currentTable.FieldNames.Count(); i++)
            {
                RowDefinition rowDef = new RowDefinition();
                parentElement.RowDefinitions.Add(rowDef);
                Label fieldLabel = new Label();
                fieldLabel.Content = currentTable.FieldNames[i];
                parentElement.Children.Add(fieldLabel);
                Grid.SetRow(fieldLabel, i);
                Grid.SetColumn(fieldLabel, 0);
                TextBox fieldTextBox = new TextBox();
                fieldTextBox.IsEnabled = false;
                fieldTextBox.Name = currentTable.FieldNames[i];
                fieldTextBox.Text = "...";
                parentElement.Children.Add(fieldTextBox);
                Grid.SetRow(fieldTextBox, i);
                Grid.SetColumn(fieldTextBox, 1);
            }
        }
        public static void GenerateMUI(GetMetaData _gmd)
        {
            // Implementation for Menu UI generation
            foreach(var _tableNames in _gmd.TableNames)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = _tableNames;
                menuItem.Click += (s, e) => 
                {
                    
                };
            }
        }
    }
}
