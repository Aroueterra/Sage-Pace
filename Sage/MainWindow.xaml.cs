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
            OracleConnection con = new OracleConnection(conString);
            //OracleCommand cmd = new OracleCommand();
            //cmd.CommandText = "Select * sage_table";
            //con.Open();
            //Console.WriteLine("Connected to Oracle" + con.ServerVersion);
            //if (CheckConnection() == true)
            //{
            //    MessageBox.Show("Connected");
            //}
            //OracleDataReader Reader = cmd.ExecuteReader();
            //int RecordCount = 0;
            //con.Open();
            //MessageBox.Show((TableExists(con, "sage_table")).ToString());
            //con.Close();
            Conn();
            //RecordCount = checkPort();


            //try
            //{
            //    while (Reader.Read())
            //    {
            //        RecordCount = RecordCount + 1;
            //        MessageBox.Show(Reader.GetString(0).ToString());
            //    }
            //    if (RecordCount == 0)
            //    {
            //        MessageBox.Show("No data returned");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Number of records returned: " + RecordCount);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //finally
            //{
            //    RowCount.Text = "Rows: " + RecordCount;
            //    Reader.Close();
            //    con.Close();
            //    con.Dispose();
            //    Console.WriteLine("Disconnected");
            //}
        }
        public bool CheckConnection()
        {
            using (var conn = new OracleConnection(conString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
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
                    var dataReader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(dataReader);
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
                    con.Close();

                    Console.WriteLine("Connection Closed");
                }
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
