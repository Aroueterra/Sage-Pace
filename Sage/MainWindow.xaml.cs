using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.Win32;

namespace Sage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        ListModel Retrieve = new ListModel();

        public MainWindow()
        {
            InitializeComponent();
            TextBox[] boxes = new TextBox[10]
            {
                txtID, txtISBN, txtTitle, txtEdition, txtAuthor, txtGenre,
                txtPub_Date, txtPublisher, txtQuantity, txtImage
            };
            Retrieve.SetBoxes(Retrieve.Lister(boxes));
            TextElement.FontFamilyProperty.OverrideMetadata(
            typeof(TextElement),
            new FrameworkPropertyMetadata(
            new FontFamily("Segoe UI")));
            TextBlock.FontFamilyProperty.OverrideMetadata(
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                new FontFamily("Segoe UI")));
            Retrieve.SetTable("book_table");
            FillDataGrid(Retrieve.SelectTable());
        }

        public string conString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = tcp)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA = (SID=xe))); User ID = sage; Password=password";

        #region TAB SETUP
        public void tbInventory_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            List<string> Content = Retrieve.Book_Content();
            for(int i=0; i<=9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, Content[i]);
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
            cmbColumns.ItemsSource = Retrieve.ColumnNames("book_table");
            FillDataGrid("book_table");
        }
        public void tbOrders_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();

            for (int i = 0; i <= 5; i++)
            {
                ListedBoxes[i].IsReadOnly = false;
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
            }
            ListedBoxes[0].SetValue(TextBoxHelper.WatermarkProperty, "Book ID");
            ListedBoxes[1].SetValue(TextBoxHelper.WatermarkProperty, "Student ID");
            ListedBoxes[2].SetValue(TextBoxHelper.WatermarkProperty, "Borrowed");
            ListedBoxes[3].SetValue(TextBoxHelper.WatermarkProperty, "Returned");
            ListedBoxes[4].SetValue(TextBoxHelper.WatermarkProperty, "Balance");
            for (int i = 5; i<=9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Hidden;
            }
            cmbColumns.ItemsSource = Retrieve.ColumnNames("order_table");
            FillDataGrid("order_table");
        }
        public void tbExport_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            for (int i = 0; i <= 9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Collapsed;
            }
        }
        #endregion  

        #region UI SETUP
        public void BookSetup()
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            List<string> Content = Retrieve.Book_Content();
            for (int i = 0; i <= 9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, Content[i]);
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
        }
        public void StudentSetup()
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            ListedBoxes[0].SetValue(TextBoxHelper.WatermarkProperty, "Student ID");
            ListedBoxes[1].SetValue(TextBoxHelper.WatermarkProperty, "Student Name");
            ListedBoxes[2].SetValue(TextBoxHelper.WatermarkProperty, "Contact ID");
            for (int i = 0; i <= 2; i++)
            {
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
            for (int i = 3; i <= 9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Hidden;
            }
        }
        public void AuthorSetup()
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            ListedBoxes[0].SetValue(TextBoxHelper.WatermarkProperty, "Author ID");
            ListedBoxes[1].SetValue(TextBoxHelper.WatermarkProperty, "Author Name");
            for (int i = 0; i <= 1; i++)
            {
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
            for (int i = 2; i <= 9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Hidden;
            }
        }
        public void GenreSetup()
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            ListedBoxes[0].SetValue(TextBoxHelper.WatermarkProperty, "Genre ID");
            ListedBoxes[1].SetValue(TextBoxHelper.WatermarkProperty, "Genre Name");
            for (int i = 0; i <= 1; i++)
            {
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
            for (int i = 2; i <= 9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Hidden;
            }
        }
        public void ContactSetup()
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            ListedBoxes[0].SetValue(TextBoxHelper.WatermarkProperty, "Contact ID");
            ListedBoxes[1].SetValue(TextBoxHelper.WatermarkProperty, "Phone Number");
            ListedBoxes[2].SetValue(TextBoxHelper.WatermarkProperty, "Zip Code");
            ListedBoxes[3].SetValue(TextBoxHelper.WatermarkProperty, "Address");
            for (int i = 0; i <= 3; i++)
            {
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
            for (int i = 4; i <= 9; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region ComboBox
        private bool selected = true;
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (selected) HandleSelection();
            selected = true;
        }
        void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            selected = !cmb.IsDropDownOpen;
            HandleSelection();
        }

        void HandleSelection()
        {
            try
            {
                if (cmbTables.SelectedItem == null)
                    return;
                if (cmbTables.SelectedValue.ToString() != null)
                {
                    string item = cmbTables.SelectedValue.ToString();
                    Retrieve.SetTable(item);
                    switch (Retrieve.SelectTable())
                    {
                        case "book_table":
                            BookSetup();
                            cmbColumns.ItemsSource = Retrieve.ColumnNames(Retrieve.SelectTable());
                            break;
                        case "author_master":
                            AuthorSetup();
                            cmbColumns.ItemsSource = Retrieve.ColumnNames(Retrieve.SelectTable());
                            break;
                        case "genre_master":
                            GenreSetup();
                            cmbColumns.ItemsSource = Retrieve.ColumnNames(Retrieve.SelectTable());
                            break;
                        case "student_table":
                            StudentSetup();
                            cmbColumns.ItemsSource = Retrieve.ColumnNames(Retrieve.SelectTable());
                            break;
                        case "contact_table":
                            ContactSetup();
                            cmbColumns.ItemsSource = Retrieve.ColumnNames(Retrieve.SelectTable());
                            break;
                    }
                }
                else
                {
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion
        
        #region CRUD
        public bool CheckAvail(string id, string table)
        {
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            string column = null;
            switch (table)
            {
                case "book_table":
                    column = "book_ID";
                    command = "Select COUNT(*) from " + QueryBuilder(table, "where", column);
                    break;
                case "author_master":
                    column = "author_ID";
                    command = "Select COUNT(*) from " + QueryBuilder(table, "where", column);
                    Console.WriteLine(command);
                    break;
                case "genre_master":
                    column = "genre_ID";
                    command = "Select COUNT(*) from " + QueryBuilder(table, "where", column);          
                    break;
                case "student_table":
                    column = "student_ID";
                    command = "Select COUNT(*) from " + QueryBuilder(table, "where", column);
                    break;
                case "contact_table":
                    column = "contact_ID";
                    command = "Select COUNT(*) from " + QueryBuilder(table, "where", column);
                    break;
                default:
                    column = "order_ID";
                    command = "Select COUNT(*) from " + QueryBuilder("order_table", "where", column);
                    break;
            }

            int count = 0;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                OracleCommand cmd = new OracleCommand(command, con);
                cmd.Parameters.Add(new OracleParameter(column, txtID.Text));
                try
                {
                    object result = cmd.ExecuteScalar();
                    result = (result == DBNull.Value) ? null : result;
                    count = Convert.ToInt32(result);
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("Exception Message: " + ex.Message);
                    Console.WriteLine("Exception Source: " + ex.Source);
                }
            }

            if (count >= 1)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
        public void btnInsert_Click(object sender, RoutedEventArgs e)
        {

            string table = Retrieve.SelectTable();
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            switch (table) {
                case "book_table":
                    Book_INSERT(connection, table);
                    break;
                case "author_master":
                    Author_INSERT(connection, table);
                    break;
                case "genre_master":
                    Genre_INSERT(connection, table);
                    break;
                case "student_table":
                    Student_INSERT(connection, table);
                    break;
                case "contact_table":
                    Contact_INSERT(connection, table);
                    break;
            }
            //MessageBox.Show("Query complete");

        }
        private void btnInsertOrder_Click(object sender, RoutedEventArgs e)
        {
            string table = "order_table";
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            bool type = false;
            string command = string.Empty;
            if (CheckAvail(txtID.Text, table) == true)
            {
                command = "UPDATE order_table SET book_id = :book_id, student_id = :student_id, borrowed =:borrowed, returned =:returned, balance =:balance where order_ID = :order_ID";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO order_table (book_id, student_id, borrowed, returned, balance) VALUES(:book_id, :student_id, :borrowed, :returned, :balance)";
                type = false;
                Console.WriteLine("Inserting a record!");
            }
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                for (int i = 0; i < 2; i++)
                {
                    if (string.IsNullOrEmpty(Retrieve.GetBoxes()[i].Text))
                        throw new ArgumentException("Parameter cannot be null", "Null detected!");
                }
                cmd.Parameters.Add(new OracleParameter("book_id", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("student_id", txtISBN.Text));
                if (string.IsNullOrEmpty(txtTitle.Text))
                    cmd.Parameters.Add(new OracleParameter("borrowed", DBNull.Value));
                else
                {
                    DateTime CreatedDate = DateTime.ParseExact(txtTitle.Text, new String[] {
                "MM/dd/yyyy", 
                },              
                    System.Globalization.CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal);
                    cmd.Parameters.Add(new OracleParameter("borrowed", OracleDbType.Date)).Value = CreatedDate;
                }
                if (string.IsNullOrEmpty(txtEdition.Text))
                    cmd.Parameters.Add(new OracleParameter("returned", DBNull.Value));
                else
                {
                    DateTime CreatedDate;
                    DateTime.TryParseExact(txtEdition.Text, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out CreatedDate);
                    cmd.Parameters.Add(new OracleParameter("returned", OracleDbType.Date)).Value = CreatedDate;
                }
                if (string.IsNullOrEmpty(txtAuthor.Text))
                    cmd.Parameters.Add(new OracleParameter("balance", DBNull.Value));
                else
                    cmd.Parameters.Add(new OracleParameter("balance", OracleDbType.Int32)).Value = Convert.ToInt32(txtAuthor.Text);

                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted";
                    if (rowsUpdated > 1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                Subtract();
                con.Close();
            }
        }

        string QueryBuilder(string table, string query, string column)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(table);
            sb.Append(" " + query + " ");
            sb.Append(column + " = :" + column);
            return sb.ToString();
        }

        void Book_INSERT(string connection, string table)
        {
            bool type = false;
            string command = string.Empty;
            if (CheckAvail(txtID.Text, table) == true)
            {
                command = "UPDATE book_table SET book_ID = :BOOK_ID, ISBN = :ISBN, TITLE =:Title, EDITION =:Edition, AUTHOR_ID =:Author, GENRE_ID =:Genre, PUBLICATION_DATE =:Publication_Date, PUBLISHER =:Publisher, QUANTITY =:Quantity, IMAGE =:IMAGE";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO book_table VALUES(:Book_ID, :ISBN, :Title, :Edition, :Author_ID, :Genre_ID, :Publication_Date, :Publisher, :Quantity, :IMAGE)";
                type = false;
                Console.WriteLine("Inserting a record!");
                Console.WriteLine(command);
            }
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                if (string.IsNullOrEmpty(txtID.Text))
                    cmd.Parameters.Add(new OracleParameter("Book_ID", DBNull.Value));
                else
                    cmd.Parameters.Add(new OracleParameter("Book_ID", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("ISBN", txtISBN.Text));
                cmd.Parameters.Add(new OracleParameter("Title", txtTitle.Text));
                cmd.Parameters.Add(new OracleParameter("Edition", Convert.ToInt32(txtEdition.Text)));
                cmd.Parameters.Add(new OracleParameter("Author_ID", txtAuthor.Text));
                cmd.Parameters.Add(new OracleParameter("Genre_ID", txtGenre.Text));

                if (string.IsNullOrEmpty(txtPub_Date.Text))
                    cmd.Parameters.Add(new OracleParameter("Publication_Date", DBNull.Value));
                else
                {
                    DateTime CreatedDate = DateTime.ParseExact(txtPub_Date.Text, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal);
                    cmd.Parameters.Add(new OracleParameter("Publication_Date", OracleDbType.Date)).Value=CreatedDate;
                    Console.WriteLine(CreatedDate.ToShortDateString());
                }

                cmd.Parameters.Add(new OracleParameter("Publisher", txtPublisher.Text));
                cmd.Parameters.Add(new OracleParameter("balance", OracleDbType.Int32)).Value = Convert.ToInt32(txtQuantity.Text);
                if (string.IsNullOrEmpty(txtPub_Date.Text))
                    cmd.Parameters.Add(new OracleParameter("IMAGE", DBNull.Value));
                else
                    cmd.Parameters.Add(new OracleParameter("IMAGE", txtImage.Text));
                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted";
                    if (rowsUpdated > 1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                con.Close();
            }
        }
        void Contact_INSERT(string connection, string table)
        {
            bool type = false;
            string command = string.Empty;
            if (CheckAvail(txtID.Text, table) == true)
            {
                command = "UPDATE contact_table SET contact_ID = :contact_ID, phone_number = :phone_number, zip_code = :zip_code, address =:address where contact_ID = :contact_ID";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO contact_table (contact_ID, phone_Number, zip_Code, Address) VALUES(:contact_ID, :phone_number, :zip_code, :address)";
                type = false;
                Console.WriteLine("Inserting a record!");
            }
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                for (int i = 0; i < 3; i++)
                {
                    if (string.IsNullOrEmpty(Retrieve.GetBoxes()[i].Text))
                        throw new ArgumentException("Parameter cannot be null", "Null detected!");
                }
                cmd.Parameters.Add(new OracleParameter("contact_ID", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("phone_number", txtISBN.Text));
                cmd.Parameters.Add(new OracleParameter("zip_code", txtTitle.Text));
                cmd.Parameters.Add(new OracleParameter("address", txtEdition.Text));

                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted";
                    if (rowsUpdated > 1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                con.Close();
            }
        }
        void Student_INSERT(string connection, string table)
        {
            bool type = false;
            string command = string.Empty;
            if (CheckAvail(txtID.Text, table) == true)
            {
                command = "UPDATE student_table SET student_ID = :student_ID, student_Name = :student_Name, contact_ID = :contact_ID where student_ID = :student_ID";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO student_table (student_ID, student_Name, contact_ID) VALUES(:student_ID, :student_Name, :contact_ID)";
                type = false;
                Console.WriteLine("Inserting a record!");
            }
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                for(int i = 0; i<2; i++)
                {
                    if (string.IsNullOrEmpty(Retrieve.GetBoxes()[i].Text))
                        throw new ArgumentException("Parameter cannot be null", "Null detected!");
                }
                cmd.Parameters.Add(new OracleParameter("student_ID", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("student_Name", txtISBN.Text));
                cmd.Parameters.Add(new OracleParameter("contact_ID", txtTitle.Text));
                
                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted";
                    if (rowsUpdated > 1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                con.Close();
            }
        }
        void Author_INSERT(string connection, string table)
        {
            bool type = false;
            string command = string.Empty;
            if (CheckAvail(txtID.Text, table) == true)
            {
                command = "UPDATE author_master SET author_ID = :author_ID, author_Name = :author_Name where author_ID = :author_ID";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO author_master (author_ID, author_Name) VALUES(:author_ID, :author_Name)";
                type = false;
                Console.WriteLine("Inserting a record!");
            }
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                for (int i = 0; i < 1; i++)
                {
                    if (string.IsNullOrEmpty(Retrieve.GetBoxes()[i].Text))
                        throw new ArgumentException("Parameter cannot be null", "Null detected!");
                }
                cmd.Parameters.Add(new OracleParameter("author_ID", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("author_Name", txtISBN.Text));

                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted";
                    if (rowsUpdated > 1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                con.Close();
            }
        }
        void Genre_INSERT(string connection, string table)
        {
            bool type = false;
            string command = string.Empty;
            if (CheckAvail(txtID.Text, table) == true)
            {
                command = "UPDATE genre_master SET genre_ID = :genre_ID, genre_Name = :genre_Name where genre_ID = :genre_ID";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO genre_master (genre_ID, genre_Name) VALUES(:genre_ID, :genre_Name)";
                type = false;
                Console.WriteLine("Inserting a record!");
            }
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                for (int i = 0; i < 1; i++)
                {
                    if (string.IsNullOrEmpty(Retrieve.GetBoxes()[i].Text))
                        throw new ArgumentException("Parameter cannot be null", "Null detected!");
                }
                cmd.Parameters.Add(new OracleParameter("genre_ID", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("genre_Name", txtISBN.Text));

                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted";
                    if (rowsUpdated > 1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                con.Close();
            }
        }

        public void SearchFocus(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            foreach (TextBox tb in ListedBoxes)
            {
                tb.Clear();
            }
        }
        public void btnGo_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            Console.WriteLine(cmbColumns.SelectedValue);
            if (cmbColumns.SelectedItem == null) { Console.WriteLine("No Value in cmb"); return; }

            else if (String.IsNullOrEmpty(txtSearch.Text)) { Console.WriteLine("No Value in txtSearch"); return; }
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                string tables = Retrieve.SelectTable();
                command = "Select * FROM " + tables + " where LOWER("+ cmbColumns.SelectedItem.ToString() + ") = :value";
                Console.WriteLine(command);
                OracleCommand cmd = new OracleCommand(command, con);
                cmd.Parameters.Add(new OracleParameter(":value", txtSearch.Text));
                try
                {
                    Console.WriteLine("Searching TABLE: " + tables);
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable(tables);
                    try
                    {
                        oda.Fill(dt);
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Exception Message: " + ex.Message);
                        MessageBox.Show("Exception Source: " + ex.Source);
                    }
                    DGV.ItemsSource = null;
                    DGV.ItemsSource = dt.DefaultView;
                    DGV.Items.Refresh();
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
            }

        }
        public void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
    
        }
        public void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                string tables = Retrieve.SelectTable();
                if (ListedBoxes[0].Text.ToLower() == "all")
                {
                    command = "Delete FROM "+tables;
                    Console.WriteLine(command);
                    OracleCommand cmd = new OracleCommand(command, con);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Rows deleted");
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Exception Message: " + ex.Message);
                        MessageBox.Show("Exception Source: " + ex.Source);
                    }
                }
                else
                {
                    command = "Delete FROM " + QueryBuilder(tables, "where", tables.Split('_')[0] + "_ID");
                    Console.WriteLine(command);
                    OracleCommand cmd = new OracleCommand(command, con);
                    cmd.Parameters.Add(new OracleParameter(":" + tables.Split('_')[0] + "_ID", ListedBoxes[0].Text));
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Exception Message: " + ex.Message);
                        MessageBox.Show("Exception Source: " + ex.Source);
                    }
                }
            }
        }
        public void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                string tables = Retrieve.SelectTable();
                if (ListedBoxes[0].Text.ToLower() == "all")
                {
                    command = "Delete FROM order_table";
                    Console.WriteLine(command);
                    OracleCommand cmd = new OracleCommand(command, con);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Rows deleted");
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Exception Message: " + ex.Message);
                        MessageBox.Show("Exception Source: " + ex.Source);
                    }
                }
                else
                {
                    command = "Delete FROM order_table where order_ID = :order_ID";
                    Console.WriteLine(command);
                    OracleCommand cmd = new OracleCommand(command, con);
                    cmd.Parameters.Add(new OracleParameter(":order_ID", ListedBoxes[0].Text));
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Exception Message: " + ex.Message);
                        MessageBox.Show("Exception Source: " + ex.Source);
                    }
                }
            }
        }
        #endregion

        #region  PASSIVE
        private void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.StartsWith("BOOK_ID"))
                e.Column.Header = "BOOK ID";
            if (e.PropertyName.StartsWith("ORDER_ID"))
                e.Column.Header = "ORDER ID";
            if (e.PropertyName.StartsWith("AUTHOR_ID"))
                e.Column.Header = "AUTHOR ID";
            if (e.PropertyName.StartsWith("GENRE_ID"))
                e.Column.Header = "GENRE ID";
            if (e.PropertyName.StartsWith("AUTO_NO"))
                e.Column.Header = "AUTO";
            if (e.PropertyName.StartsWith("PRICE_AMT"))
                e.Column.Header = "PRICE";
            if (e.PropertyName.StartsWith("PAID_AMt"))
                e.Column.Header = "PAID";
            if (e.PropertyName.StartsWith("IMAGE"))
                e.Column.Header = "IMAGE";
            if (e.PropertyName.StartsWith("PUBLICATION_DATE"))
                e.Column.Header = "DATE";
            if (e.PropertyName.StartsWith("STUDENT_ID"))
                e.Column.Header = "STUDENT ID";
            if (e.PropertyName.StartsWith("STUDENT_NAME"))
                e.Column.Header = "STUDENT NAME";
            if (e.PropertyName.StartsWith("CONTACT_ID"))
                e.Column.Header = "CONTACT ID";
            if (e.PropertyName.StartsWith("PHONE_NUMBER"))
                e.Column.Header = "PHONE NUMBER";
            if (e.PropertyName.StartsWith("ZIP_CODE"))
                e.Column.Header = "ZIP CODE";
        }
        private void FillDataGrid(string tables)
        {
            string ConString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string CmdString = string.Empty;
            using (OracleConnection con = new OracleConnection(ConString))
            {
                CmdString = string.Format("SELECT * FROM {0}", tables);
                Console.WriteLine("Using TABLE: " + tables);
                OracleCommand cmd = new OracleCommand(CmdString, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable(tables);
                try
                {
                    oda.Fill(dt);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                DGV.ItemsSource = null;
                DGV.ItemsSource = dt.DefaultView;
                DGV.Items.Refresh();
            }
        }
        #endregion

        #region Throwaway
        public void Conn()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                con.Open();
                try
                {

                    Console.WriteLine("Connection Opened");
                    OracleCommand cmd = new OracleCommand();
                    string sqlquery = ("SELECT * FROM sage_schema.sage_table");
                    cmd.CommandText = sqlquery;
                    OracleDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    DGV.DataContext = dataTable;
                    //OracleDataReader dr = cmd.ExecuteReader();
                    //txtFName.Text = dr.ToString();
                }
                catch (OracleException ex)
                {
                    //Console.WriteLine("Record is not inserted into the database table.");
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
                finally
                {
                    Console.WriteLine("Connection Closed");
                }
            }
        }
        public class OracleDBManager
        {
            //private OracleConnection _con;
            private const string connectionString = "User Id={0};Password={1};Data Source=MyDataSource;";
            private const string OracleDBUser = "sage";
            private const string OracleDBPassword = "password";

        }
        private void DGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result.HasValue && result.Value)
            {
                // Open document 
                string filename = dlg.FileName;
                txtImage.Text = filename;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid(Retrieve.SelectTable());
        }

        private void Subtract()
        {
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                command = "UPDATE book_table set Quantity = Quantity-1 where book_id = :book_id";
                OracleCommand cmd = new OracleCommand(command, con);
                //cmd.Parameters.Add(new OracleParameter("Quantity", txtID.Text));
                cmd.Parameters.Add(new OracleParameter("book_id", txtID.Text));
                cmd.ExecuteNonQuery();
            }
        }
        private void ADD()
        {
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                command = "UPDATE book_table set Quantity = Quantity-1 where book_id = :book_id";
                OracleCommand cmd = new OracleCommand(command, con);
                cmd.Parameters.Add(new OracleParameter("book_id", txtID.Text));
                cmd.ExecuteNonQuery();
            }
        }

        public bool HasPassed(DateTime fromDate, DateTime expireDate)
        {
            return expireDate - fromDate > TimeSpan.FromDays(7);
        }
        public TimeSpan HasFactor(DateTime fromDate, DateTime expireDate)
        {
            return expireDate - fromDate;
        }
        void ApplyExpire(string orderid, double factor, bool passed)
        {
            string command = string.Empty;

            Console.WriteLine(orderid);
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                command = "UPDATE order_table SET balance =:balance where order_ID = :order_ID";
                OracleCommand cmd = new OracleCommand(command, con);
                if (passed == true)
                {
                    if (factor != 0)
                    {
                        cmd.Parameters.Add(new OracleParameter("balance", OracleDbType.Int32)).Value = 80 * factor;
                    }
                    else
                    {
                        cmd.Parameters.Add(new OracleParameter("balance", OracleDbType.Int32)).Value = 80;
                    }
                }
                else
                {
                    cmd.Parameters.Add(new OracleParameter("balance", OracleDbType.Int32)).Value = 0;
                }
                cmd.Parameters.Add(new OracleParameter("order_id", orderid));
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
            }
        }
        private void btnOverdue_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            Console.WriteLine(cmbColumns.SelectedValue);
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                string tables = Retrieve.SelectTable();
                command = "SELECT*FROM order_table WHERE returned IN(SELECT returned FROM order_table GROUP BY returned HAVING COUNT(*) >= 1)";
                Console.WriteLine(command);
                OracleCommand cmd = new OracleCommand(command, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable("order_table");
                oda.Fill(dt);
                Console.WriteLine(dt.Rows.Count.ToString());
                foreach (DataRow row in dt.Rows)
                {
                    DateTime borrow = row.Field<DateTime>("borrowed");
                    DateTime returned = row.Field<DateTime>("returned");
                    Console.WriteLine("Date: " +borrow);
                    if (HasPassed(borrow, returned) == true)
                    {
                        Console.WriteLine("Passed");
                        string id = Convert.ToString(row.Field<Decimal>("order_id"));
                        double factor=0;
                        factor = (HasFactor(borrow, returned).Days)/7;
                        Console.WriteLine("Factor: " +factor);
                        try
                        {
                            ApplyExpire(id, factor, true);
                        }
                        catch (OracleException ex)
                        {
                            MessageBox.Show("Exception Message: " + ex.Message);
                            MessageBox.Show("Exception Source: " + ex.Source);
                        }
                    }
                    else
                    {
                        string id = Convert.ToString(row.Field<Decimal>("order_id"));
                        Console.WriteLine("Failed");
                        try
                        {
                            ApplyExpire(id, 0, false);
                        }
                        catch (OracleException ex)
                        {
                            MessageBox.Show("Exception Message: " + ex.Message);
                            MessageBox.Show("Exception Source: " + ex.Source);
                        }
                    }
                }

            }
            MessageBox.Show("Succeeded in penalizing overdue accounts!");            
        }

        void ReturnBooks(string id)
        {
            string command = string.Empty;

            Console.WriteLine(id);
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                command = "UPDATE book_table SET quantity=quantity+1 where book_ID = :book_ID";
                OracleCommand cmd = new OracleCommand(command, con);
                cmd.Parameters.Add(new OracleParameter("book_id", id));
                cmd.ExecuteNonQuery();

            }
        }
        void PurgeAccounts(string id, string Command)
        {
            string command = Command;
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                OracleCommand cmd = new OracleCommand(command, con);
                //cmd.Parameters.Add(new OracleParameter("book_id", id));
                cmd.ExecuteNonQuery();

            }
        }
        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string selectCommand = string.Empty;
            string deleteCommand = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                selectCommand = "SELECT * FROM book_table where book_id IN(SELECT book_id FROM order_table WHERE balance IN(SELECT balance FROM order_table GROUP BY balance HAVING COUNT(*) >= 1))";
                deleteCommand = "DELETE FROM order_table WHERE balance IN(SELECT balance FROM order_table GROUP BY balance HAVING COUNT(*) >= 1)";
                //deleteCommand = "DELETE FROM order_table WHERE balance IN(SELECT balance FROM order_table GROUP BY balance HAVING COUNT(*) >= 1)";
                Console.WriteLine(selectCommand);
                OracleCommand cmd = new OracleCommand(selectCommand, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable("book_table");
                oda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string id = Convert.ToString(row.Field<Decimal>("book_id"));
                    //int newQuantity = Convert.ToInt32(row.Field<Decimal>("quantity"));
                    try
                    {
                        ReturnBooks(id);
                        PurgeAccounts(id, deleteCommand);
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Exception Message: " + ex.Message);
                        MessageBox.Show("Exception Source: " + ex.Source);
                    }
                }
            }
            MessageBox.Show("Succeeded in purging overdue accounts!");
        }


        #region EXCEL
        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                Retrieve.SetIO(openFileDialog.FileName);
                int sheets = Convert.ToInt32(txtSheets.Value);
                readXLS(Retrieve.GetIO(), true, sheets);
                //GetDataTableFromExcel(Retrieve.GetIO());

            }
        }
        public void importExcel(string query)
        {
            query += " SELECT 1 FROM DUAL";
            List<TextBox> ListedBoxes = Retrieve.GetBoxes();
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                string tables = Retrieve.SelectTable();
                command = query;
                Console.WriteLine(command);
                OracleCommand cmd = new OracleCommand(command, con);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Rows imported!");
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Exception Message: " + ex.Message);
                    MessageBox.Show("Exception Source: " + ex.Source);
                }
            }
        }
        public void readXLS(string FilePath, bool hasHeader, int sheets)
        {
            FileInfo existingFile = new FileInfo(FilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                string tables = Retrieve.SelectTable();
                if (tables == "order_table") return;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheets];
                int colCount = worksheet.Dimension.End.Column;  //get Column Count
                int rowCount = worksheet.Dimension.End.Row;     //get row count
                if (worksheet.Dimension == null) return;

                //string queryString = "INSERT INTO " + tables + " VALUES";
                string queryString = "INSERT ALL ";
                string intoString = "";
                switch (tables)
                {
                    case "student_table":
                        intoString += "INTO student_table (student_ID, student_Name, contact_ID) VALUES";
                        break;
                    case "book_table": //AutoPK
                        intoString += "INTO book_table (ISBN, Title, Edition, Author_ID, Genre_ID, Publication_Date, Publisher, Quantity, IMAGE) VALUES";
                        break;
                    case "author_master": //AutoPK
                        intoString += "INTO author_master (author_ID, author_Name) VALUES";
                        break;
                    case "genre_master": //AutoPK
                        intoString += "INTO genre_master (genre_ID, genre_Name) VALUES";
                        break;
                    case "contact_table":
                        intoString += "INTO contact_table (contact_ID, phone_Number, zip_Code, address) VALUES";
                        break;
                }
                string eachVal = "";
                Console.WriteLine("Row: " + rowCount + " Col: " + colCount);
                int row;
                int counter = 0;
                //Start row at 2 to avoid header
                for (row = (hasHeader == true) ? 2 : 1; row <= rowCount; row++)
                {
                    Console.WriteLine("Current row: " + row);

                    if (counter >= 1) queryString += intoString + "(";
                    else queryString += intoString + "(";

                    for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                    {
                        Console.WriteLine("Current col: " + col);
                        //Awesome NULL propagation operator
                        eachVal = worksheet?.Cells[row, col]?.Value?.ToString().Trim();
                        queryString += "'" + eachVal + "',";
                    }
                    //removing last comma (,) from the string
                    queryString = queryString.Remove(queryString.Length - 1, 1);
                    //On every 1000 query will execute, as maximum of 1000 will be executed at a time. 
                    if (row % 1000 == 0)
                    {
                        queryString += "),";
                        importExcel(queryString);    //executing query
                    }
                    else
                    {
                        queryString += ") ";
                    }
                    counter++;
                }
                queryString = queryString.Remove(queryString.Length - 1, 1);    //removing last comma (,) from the string
                Console.WriteLine("Query String: " + queryString);
                importExcel(queryString);    //executing query
            }
        } 
        #endregion
    }
    

}
