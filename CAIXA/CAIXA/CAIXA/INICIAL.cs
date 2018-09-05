using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAIXA
{
    public partial class INICIAL : Form
    {
        public INICIAL()
        {
            InitializeComponent();
        }

        private void btnProduto_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            PRODUTO p = new PRODUTO();
            p.ShowDialog();
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            VENDA v = new VENDA();
            v.ShowDialog();
        }

        private void INICIAL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
