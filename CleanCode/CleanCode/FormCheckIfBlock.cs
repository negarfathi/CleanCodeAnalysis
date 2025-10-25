using System.Windows.Forms;

namespace CleanCode
{
    public partial class FormCheckIfBlock : Form
    {
        public FormCheckIfBlock()
        {
            InitializeComponent();
        }
        public void Show(string x)
        {
            dataGridView.ColumnCount = 1;
            dataGridView.Columns[0].Name = "Line";
            dataGridView.Columns[0].MinimumWidth = 220;
            string[] row = new string[] { x };
            dataGridView.Rows.Add(row);
        }
    }
}