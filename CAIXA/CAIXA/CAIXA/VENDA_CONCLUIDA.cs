using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAIXA
{
    public partial class VENDA_CONCLUIDA : Form
    {
        public VENDA_CONCLUIDA()
        {
            InitializeComponent();
            GRAVAR();
        }
        SqlConnection sqlCon = null;
        DataTable venda = null;
        SqlDataAdapter da = null;
        private string strCon = @"Password=1234qwer;Persist Security Info=True;User ID=sa;Initial Catalog=CAIXA;Data Source=MONICANOTEBOOK\SQLEXPRESS";
        private string strSql = string.Empty;

        private void GRAVAR()
        {
            string strSql = "SELECT v.VENDAID, v.PRODUTOID, v.DESCRICAO, v.QUANTIDADE, v.VALORU, v.VALORT, vf.VALORFINAL FROM VENDA AS v INNER JOIN VALORFINAL AS vf ON v.VENDAID & vf.VENDAID=" + "(SELECT MAX(VENDAID) FROM VENDA)" + "";
            Carregar(strSql);
        }
        private void Carregar(string SQLCONSULTAGERAL)
        {
            try {
                sqlCon = new SqlConnection(strCon);
                da = new SqlDataAdapter(SQLCONSULTAGERAL, sqlCon);
                venda = new DataTable();
                da.Fill(venda);
                venda.Columns[0].ColumnName = "VENDA";
                venda.Columns[1].ColumnName = "PRODUTO";
                venda.Columns[2].ColumnName = "DESCRICAO";
                venda.Columns[3].ColumnName = "QUANTIDADE";
                venda.Columns[4].ColumnName = "VALOR UNITARIO";
                venda.Columns[5].ColumnName = "VALOR TOTAL";
                venda.Columns[6].ColumnName = "VALOR FINAL";
                dataGridView1.DataSource = venda;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VENDA_CONCLUIDA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
