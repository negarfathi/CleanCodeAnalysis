using System.Windows.Forms;

namespace CleanCode
{
    public partial class FormCheckMethodParameters : Form
    {
        public FormCheckMethodParameters()
        {
            InitializeComponent();
        }
        public void Show(string x, string y)
        {
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "Method Name";
            dataGridView.Columns[0].MinimumWidth = 310;
            dataGridView.Columns[1].Name = "Line";
            dataGridView.Columns[1].MinimumWidth = 110;
            string[] row = new string[] { x, y };
            dataGridView.Rows.Add(row);
        }
    }
}