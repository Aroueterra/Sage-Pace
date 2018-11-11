using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sage
{
    public class ListModel
    {
        private string IO_NAME;
        private List<TextBox> ListedBox;
        private List<String> Author_Master = new List<string>
            (new string[] { "Author_ID", "Author_Name"});
        private List<String> Genre_Master = new List<string>
            (new string[] { "Genre_ID", "Genre_Name" });
        private List<String> Contact_Table = new List<string>
            (new string[] { "Contact_ID", "Phone_Number", "Zip_Code", "Address" });
        private List<String> Student_Table = new List<string>
            (new string[] { "Student_ID", "Student_Name", "Contact_ID" });
        private List<String> Book_Table = new List<string>
            (new string[] { "Book_ID", "ISBN", "Title", "Edition", "Author_ID", "Genre_ID", "Publication_Date", "Publisher", "Quantity", "Image" });
        private List<String> Order_Table = new List<string>
            (new string[] { "Order_ID", "Book_ID", "Student_ID", "Borrowed", "Returned" });

        private string SelectedTable;
        public List<TextBox> Lister(TextBox[] boxes)
        {
            TextBox[] box = new TextBox[10];
            box = boxes;
            List<TextBox> ListedBoxes = new List<TextBox>()
            {
                box[0], box[1], box[2], box[3], box[4], box[5],
                box[6], box[7], box[8], box[9] 
            };
            return ListedBoxes;
        }
        public void SetBoxes(List<TextBox> texts)
        {
            ListedBox = texts;
        }
        public void SetIO(string texts)
        {
            IO_NAME = texts;
        }
        public string GetIO()
        {
            return IO_NAME;
        }
        public List<TextBox> GetBoxes()
        {
            return ListedBox;
        }
        public List<string> Book_Content()
        {
            List<string> Content = new List<string>()
            {
                "Book ID", "ISBN", "Title", "Edition", "Author ID", "Genre ID",
                "Publication Date", "Publisher", "Quantity", "Image"
            };
            return Content;
        }
        public List<string> ColumnNames(string str)
        {
            switch (str)
            {
                case "author_master":
                    return Author_Master;
                case "genre_master":
                    return Genre_Master;
                case "contact_table":
                    return Contact_Table;
                case "book_table":
                    return Book_Table;
                case "student_table":
                    return Student_Table;
                case "order_table":
                    return Order_Table;
                default:
                    return Book_Table;
            }
            
        }
        public void SetTable(string table)
        {
            SelectedTable = table;
        }
        public string SelectTable()
        {
            return SelectedTable;
        }
    }
}
