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

namespace Sage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        private ViewModeller VM;
        public MainWindow()
        {
            InitializeComponent();
            VM = new ViewModeller();

            TextElement.FontFamilyProperty.OverrideMetadata(
            typeof(TextElement),
            new FrameworkPropertyMetadata(
            new FontFamily("Segoe UI")));
            TextBlock.FontFamilyProperty.OverrideMetadata(
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                new FontFamily("Segoe UI")));

            FillDataGrid();
            Receipt.IsEnabled = false;
            Export.IsEnabled = false;

        }
        public
        string conString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = tcp)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA = (SID=xe))); User ID = sage; Password=password";
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(conString);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "INSERT INTO nexus_table(ID, First_Name) Values(@IDNum, @FNameTxt)";
            cmd.Parameters.AddWithValue("@IDnum", txtID.Text);
            cmd.Parameters.AddWithValue("@FNameTxt", txtISBN.Text);
            Connection.Open();
            OleDbDataReader Reader = cmd.ExecuteReader();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Reader.Close();
                Connection.Close();
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {

            OLE();
        }

        public void tbInventory_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = new List<TextBox>()
            {
                txtID, txtISBN, txtTitle, txtEdition, txtAuthor, txtGenre,
                txtPub_Date, txtPublisher, txtQuantity, txtPrice, txtThumb
            };
            List<string> Content = new List<string>()
            {
                "Book ID", "ISBN ID", "Title", "Edition", "Author", "Genre",
                "Publication Date", "Publisher", "Quantity", "Price", "Thumbnail"
            };
            for(int i=0; i<=10; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, Content[i]);
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
                ListedBoxes[i].IsReadOnly = false;
            }
            Insert.IsEnabled = true;
            Update.IsEnabled = true;
            Delete.IsEnabled = true;
            Receipt.IsEnabled = false;
            Export.IsEnabled = false;
        }
        public void tbOrders_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = new List<TextBox>()
            {
                txtID, txtISBN, txtTitle, txtEdition, txtAuthor, txtGenre,
                txtPub_Date, txtPublisher, txtQuantity, txtPrice, txtThumb
            };
            for (int i = 0; i <= 5; i++)
            {
                ListedBoxes[i].IsReadOnly = false;
                ListedBoxes[i].IsEnabled = true;
                ListedBoxes[i].Visibility = Visibility.Visible;
            }
            ListedBoxes[0].SetValue(TextBoxHelper.WatermarkProperty, "Order ID");
            ListedBoxes[1].SetValue(TextBoxHelper.WatermarkProperty, "Book ID");
            ListedBoxes[2].SetValue(TextBoxHelper.WatermarkProperty, "Price");
            ListedBoxes[3].SetValue(TextBoxHelper.WatermarkProperty, "Paid");
            ListedBoxes[4].SetValue(TextBoxHelper.WatermarkProperty, "Date");
            for(int i = 5; i<=10; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Hidden;
            }
            Insert.IsEnabled = false;
            Update.IsEnabled = false;
            Delete.IsEnabled = false;
            Receipt.IsEnabled = true;
            Export.IsEnabled = false;
        }
        public void tbExport_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> ListedBoxes = new List<TextBox>()
            {
                txtID, txtISBN, txtTitle, txtEdition, txtAuthor, txtGenre,
                txtPub_Date, txtPublisher, txtQuantity, txtPrice, txtThumb
            };
            for (int i = 0; i <= 10; i++)
            {
                ListedBoxes[i].SetValue(TextBoxHelper.WatermarkProperty, "");
                ListedBoxes[i].IsReadOnly = true;
                ListedBoxes[i].IsEnabled = false;
                ListedBoxes[i].Visibility = Visibility.Collapsed;
            }
            Insert.IsEnabled = false;
            Update.IsEnabled = false;
            Delete.IsEnabled = false;
            Receipt.IsEnabled = false;
            Export.IsEnabled = true;
        }
        public Boolean CheckAvail(string id)
        {
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            int count = 0;
            using (OracleConnection con = new OracleConnection(connection))
            {
                con.Open();
                command = "Select COUNT(*) from book_table where Book_ID = :book_ID";
                OracleCommand cmd = new OracleCommand(command, con);
                cmd.Parameters.Add(new OracleParameter(":book_ID", txtID.Text));
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                count = Convert.ToInt32(result);
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
            string command = string.Empty;
            bool type = false;
            if (CheckAvail(txtID.Text) == true)
            {
                command = "UPDATE book_table SET book_ID = :BOOK_ID, ISBN = :ISBN, TITLE =:Title, EDITION =:Edition, AUTHOR =:Author, GENRE =:Genre, PUBLICATION_DATE =:Publication_Date, PUBLISHER =:Publisher, QUANTITY =:Quantity, PRICE =:Price, THUMB =:Thumbnail";
                type = true;
                Console.WriteLine("Updating a record!");
            }
            else
            {
                command = "INSERT INTO book_table VALUES(:Book_ID, :ISBN, :Title, :Edition, :Author, :Genre, :Publication_Date, :Publisher, :Quantity, :Price, :Thumbnail)";
                type = false;
                Console.WriteLine("Inserting a record!");
            }
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

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
                cmd.Parameters.Add(new OracleParameter("Author", txtAuthor.Text));
                cmd.Parameters.Add(new OracleParameter("Genre", txtGenre.Text));

                if (string.IsNullOrEmpty(txtPub_Date.Text))
                    cmd.Parameters.Add(new OracleParameter("Publication_Date", DBNull.Value));
                else
                {
                    DateTime CreatedDate = DateTime.ParseExact(txtPub_Date.Text, new String[] {
                "MM/dd/yyyy hh:mm:ss tt", // your initial pattern, recommended way
                "d-M-yyyy"},              // actual input, tolerated way
                    System.Globalization.CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal);
                    cmd.Parameters.Add(new OracleParameter("Publication_Date", CreatedDate));
                }

                cmd.Parameters.Add(new OracleParameter("Publisher", txtPublisher.Text));
                cmd.Parameters.Add(new OracleParameter("Quantity", Convert.ToInt32(txtQuantity.Text)));
                cmd.Parameters.Add(new OracleParameter("Price", Convert.ToDouble(txtPrice.Text)));
                if (string.IsNullOrEmpty(txtPub_Date.Text))
                    cmd.Parameters.Add(new OracleParameter("Thumbnail", DBNull.Value));
                else
                    cmd.Parameters.Add(new OracleParameter("Thumbnail", txtThumb.Text));
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
                    Console.WriteLine("Exception Message: " + ex.Message);
                    Console.WriteLine("Exception Source: " + ex.Source);
                }
                con.Close();
            }

        }

        public void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string command = string.Empty;
            bool type = false;
            if (CheckAvail(txtID.Text) == true){
                command = "UPDATE book_table SET book_ID = :BOOK_ID, ISBN = :ISBN, TITLE =:Title, EDITION =:Edition, AUTHOR =:Author, GENRE =:Genre, PUBLICATION_DATE =:Publication_Date, PUBLISHER =:Publisher, QUANTITY =:Quantity, PRICE =:Price, THUMB =:Thumbnail";
                type = true;
            }
            else
            {
                command = "INSERT INTO book_table VALUES(:Book_ID, :ISBN, :Title, :Edition, :Author, :Genre, :Publication_Date, :Publisher, :Quantity, :Price, :Thumbnail";
                type = false;
            }
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            
            using (OracleConnection con = new OracleConnection(connection))
            {
                OracleCommand cmd = new OracleCommand(command, con);
                if (string.IsNullOrEmpty(txtID.Text))
                    cmd.Parameters.Add(new OracleParameter(":Book_ID", DBNull.Value));
                else
                    cmd.Parameters.Add(new OracleParameter(":Book_ID", txtID.Text));
                cmd.Parameters.Add(new OracleParameter(":ISBN", txtISBN.Text));
                cmd.Parameters.Add(new OracleParameter(":Title", txtTitle.Text));
                cmd.Parameters.Add(new OracleParameter(":Edition", Convert.ToInt32(txtEdition.Text)));
                cmd.Parameters.Add(new OracleParameter(":Author", txtAuthor.Text));
                cmd.Parameters.Add(new OracleParameter(":Genre", txtGenre.Text));

                if (string.IsNullOrEmpty(txtPub_Date.Text))
                    cmd.Parameters.Add(new OracleParameter(":Publication_Date", DBNull.Value));
                else
                {
                    DateTime CreatedDate = DateTime.ParseExact(txtPub_Date.Text, new String[] {
                "MM/dd/yyyy hh:mm:ss tt", // your initial pattern, recommended way
                "d-M-yyyy"},              // actual input, tolerated way
                    System.Globalization.CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal);
                    cmd.Parameters.Add(new OracleParameter(":Publication_Date", CreatedDate));
                }

                cmd.Parameters.Add(new OracleParameter(":Publisher", txtPublisher.Text));
                cmd.Parameters.Add(new OracleParameter(":Quantity", Convert.ToInt32(txtQuantity.Text)));
                cmd.Parameters.Add(new OracleParameter(":Price", Convert.ToDouble(txtPrice.Text)));
                if (string.IsNullOrEmpty(txtPub_Date.Text))
                    cmd.Parameters.Add(new OracleParameter(":Thumbnail", DBNull.Value));
                else
                    cmd.Parameters.Add(new OracleParameter(":Thumbnail", txtThumb.Text));
                con.Open();
                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    string text = type == true ? "updated" : "inserted" ;
                    if (rowsUpdated >1)
                        MessageBox.Show(rowsUpdated + " row(s) " + text);
                    else
                        MessageBox.Show(rowsUpdated + " row " + text);
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("Exception Message: " + ex.Message);
                    Console.WriteLine("Exception Source: " + ex.Source);
                }
                con.Close();
            }
            

        }
        public void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string connection = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string command = string.Empty;
            using (OracleConnection con = new OracleConnection(connection))
            {
                command = "Delete * FROM book_table where BOOK_ID =:id ";
                OracleCommand cmd = new OracleCommand(command, con);
                cmd.Parameters.Add(new OracleParameter(":id", txtID.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtISBN.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtTitle.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtEdition.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtAuthor.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtPub_Date.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtPublisher.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtQuantity.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtPrice.Text));
                cmd.Parameters.Add(new OracleParameter(":id", txtThumb.Text));
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                
                DataTable dt = new DataTable("book_table");
                oda.Fill(dt);
                DGV.ItemsSource = dt.DefaultView;
            }
        }
        private void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.StartsWith("BOOK_ID"))
                e.Column.Header = "BOOK ID";
            if (e.PropertyName.StartsWith("ORDER_ID"))
                e.Column.Header = "ORDER ID";
            if (e.PropertyName.StartsWith("PRICE_AMT"))
                e.Column.Header = "PRICE";
            if (e.PropertyName.StartsWith("PAID_AMt"))
                e.Column.Header = "PAID";
            if (e.PropertyName.StartsWith("THUMB"))
                e.Column.Header = "IMAGE";
            if (e.PropertyName.StartsWith("PUBLICATION_DATE"))
                e.Column.Header = "DATE";
        }

        public void OLE()
        {
            String sConnectionString = "User ID=SAGE;password=password;Data Source = localhost:1521/xe; Persist Security Info = False";
            String mySelectQuery = "SELECT FName FROM sage_table";

            OracleConnection myConnection = new OracleConnection(sConnectionString);
            OracleCommand myCommand = new OracleCommand(mySelectQuery, myConnection);

            myConnection.Open();
            OracleDataReader myReader = myCommand.ExecuteReader();
            int RecordCount = 0;
            try
            {
                while (myReader.Read())
                {
                    RecordCount = RecordCount + 1;
                    MessageBox.Show(myReader.GetString(0).ToString());
                }
                if (RecordCount == 0)
                {
                    MessageBox.Show("No data returned");
                }
                else
                {
                    MessageBox.Show("Number of records returned: " + RecordCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myReader.Close();
                myConnection.Close();
            }
        }



        private void FillDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string CmdString = string.Empty;
            using (OracleConnection con = new OracleConnection(ConString))
            {
                CmdString = "SELECT * FROM book_table";
                OracleCommand cmd = new OracleCommand(CmdString, con);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable("book_table");
                sda.Fill(dt);
                DGV.ItemsSource = dt.DefaultView;
            }
        }

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
                    Console.WriteLine("Exception Message: " + ex.Message);
                    Console.WriteLine("Exception Source: " + ex.Source);
                }
                finally
                {
                    Console.WriteLine("Connection Closed");
                }
            }
        }

        public void ReadData(string connectionString)
        {
            string queryString = "SELECT ID, FName FROM sage_table";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand command = new OracleCommand(queryString, connection);
                connection.Open();
                Console.WriteLine("open");
                try
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));
                            Console.WriteLine("done");
                        }
                    }
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("Exception Message: " + ex.Message);
                    Console.WriteLine("Exception Source: " + ex.Source);
                }
            }
        }
        public class OracleDBManager
        {
            private OracleConnection _con;
            private const string connectionString = "User Id={0};Password={1};Data Source=MyDataSource;";
            private const string OracleDBUser = "sage";
            private const string OracleDBPassword = "password";

        }

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
                txtThumb.Text = filename;
            }
        }
    }
}
