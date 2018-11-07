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
        private List<TextBox> ListedBox;
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
