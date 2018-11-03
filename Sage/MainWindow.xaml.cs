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

namespace Sage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            TextElement.FontFamilyProperty.OverrideMetadata(
            typeof(TextElement),
            new FrameworkPropertyMetadata(
            new FontFamily("Segoe UI")));
            TextBlock.FontFamilyProperty.OverrideMetadata(
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                new FontFamily("Segoe UI")));
            FillDataGrid();
        }
        //string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        //string conString = "Data Source=oracle;User Id=nexus;Password=password;";
        string conString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = tcp)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA = (SID=xe))); User ID = sage; Password=password";
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(conString);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "INSERT INTO nexus_table(ID, First_Name) Values(@IDNum, @FNameTxt)";
            cmd.Parameters.AddWithValue("@IDnum", txtID.Text);
            cmd.Parameters.AddWithValue("@FNameTxt", txtFName.Text);
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
                CmdString = "SELECT ID, FName as , LName FROM sage_table";
                OracleCommand cmd = new OracleCommand(CmdString, con);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable("sage_table");
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

            public OracleDBManager()
            {
                InitializeDBConnection();
            }

            ~OracleDBManager()
            {
                if (_con != null)
                {
                    _con.Close();
                    _con.Dispose();
                    _con = null;
                }
            }

            private void InitializeDBConnection()
            {
                _con = new OracleConnection();
                _con.ConnectionString = string.Format(connectionString, OracleDBUser, OracleDBPassword);
                _con.Open();
            }
        }
        public Boolean TableExists(OracleConnection connection, String tableName)
        {
            return TableExists(connection, tableName, null);
        }

        public Boolean TableExists(OracleConnection connection, String tableName, String schema)
        {
            String sql;
            if (schema == null)
                sql = "SELECT FName FROM sage_table";
            //sql = "SELECT sage_table FROM USER_TABLES WHERE TABLE_NAME=:table";
            //"SELECT fullname FROM sup_sys.user_profile WHERE domain_user_name = :userName", db);
            else
                sql = "SELECT TABLE_NAME FROM ALL_TABLES WHERE TABLE_NAME=:table AND OWNER=:schema";
            OracleCommand command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("table", tableName));
                if (schema != null)
                    command.Parameters.Add(new OracleParameter("schema", schema));
            bool get;
            try
            {
                using (OracleDataReader reader = command.ExecuteReader())
                get=reader.HasRows;
            }
            catch (OracleException ex)
            {
                //Console.WriteLine("Record is not inserted into the database table.");
                Console.WriteLine("Exception Message: " + ex.Message);
                Console.WriteLine("Exception Source: " + ex.Source);
                get = false;
            }
            return get;
        }
    }
}
