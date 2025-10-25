using System.Windows.Forms;

namespace CleanCode
{
    public partial class FormCheckVariableName : Form
    {
        public FormCheckVariableName()
        {
            InitializeComponent();
        }
        public void Show(string x, string y, string z)
        {
            dataGridView.ColumnCount = 3;
            dataGridView.Columns[0].Name = "Variable Name";
            dataGridView.Columns[0].MinimumWidth = 200;
            dataGridView.Columns[1].Name = "Line";
            dataGridView.Columns[1].MinimumWidth = 130;
            dataGridView.Columns[2].Name = "Message";
            dataGridView.Columns[2].MinimumWidth = 240;
            string[] row = new string[] { x, y, z };
            dataGridView.Rows.Add(row);
        }
    }
}