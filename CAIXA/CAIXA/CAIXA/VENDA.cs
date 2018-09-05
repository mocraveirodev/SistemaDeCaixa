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
    public partial class VENDA : Form
    {
        public VENDA()
        {
            InitializeComponent();
            CODIGO();
        }

        SqlConnection sqlCon = null;
        DataTable venda = null;
        SqlDataAdapter da = null;
        private string strCon = @"Password=1234qwer;Persist Security Info=True;User ID=sa;Initial Catalog=CAIXA;Data Source=MONICANOTEBOOK\SQLEXPRESS";
        private string strSql = string.Empty;

        string vendaid;

        private void CODIGO()
        {
            strSql = "SELECT MAX((VENDAID)+1) FROM VENDA";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            sqlCon.Open();
            vendaid = comando.ExecuteScalar().ToString();
            txtCodVend.Text = vendaid.ToString();
            sqlCon.Close();
        }

        private void btnAddProduto_Click(object sender, EventArgs e)
        {
            ESTOQUE();
            txCodigo.Focus();
            txtDescricao.Text = "";
            txtQuantidade.Text = "";
            txtValorU.Text = "";
            txtValorTotal.Text = "";
        }
        string estoque1;
        int estoque;
        private void ESTOQUE()
        {
            strSql = "SELECT QUANTIDADE FROM PRODUTO WHERE CODIGO = "+ txCodigo.Text +"";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            sqlCon.Open();
            estoque1 = comando.ExecuteScalar().ToString();
            estoque = int.Parse(estoque1);
            sqlCon.Close();
            TESTARESTOQUE();
        }
        private void TESTARESTOQUE()
        {
            int quantidade = int.Parse(txtQuantidade.Text);
            if (quantidade > estoque)
            {
                MessageBox.Show("A QUANTIDADE DIGITADA ESTÁ ACIMA DO ESTOQUE DISPONÍVEL!");
            }
            else
            {
                string valorfinal = (estoque - quantidade).ToString();
                strSql = "UPDATE PRODUTO SET QUANTIDADE =@QUANTIDADE WHERE CODIGO=@CODIGOBUSCA";
                sqlCon = new SqlConnection(strCon);
                SqlCommand comando = new SqlCommand(strSql, sqlCon);
                comando.Parameters.Add("@CODIGOBUSCA", SqlDbType.Int).Value = int.Parse(txCodigo.Text);
                comando.Parameters.Add("@QUANTIDADE", SqlDbType.Int).Value = int.Parse(valorfinal);
                try
                {
                    sqlCon.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlCon.Close();
                }
                GRAVARPRODUTO();
            }            
            
        }
        private void CARREGAR(string SQLCONSULTAGERAL)
        {
            try
            {
                sqlCon = new SqlConnection(strCon);
                da = new SqlDataAdapter(SQLCONSULTAGERAL, sqlCon);
                venda = new DataTable();
                da.Fill(venda);
                venda.Columns[0].ColumnName = "PRODUTO";
                venda.Columns[1].ColumnName = "DESCRICAO";
                venda.Columns[2].ColumnName = "QUANTIDADE";
                venda.Columns[3].ColumnName = "VALOR UNITARIO";
                venda.Columns[4].ColumnName = "VALOR TOTAL";
                dataGridView1.DataSource = venda;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SELECT()
        {
            strSql = "SELECT PRODUTOID, DESCRICAO, QUANTIDADE, VALORU, VALORT FROM VENDA WHERE VENDAID=" + vendaid + "";
            CARREGAR(strSql);
        }
        private void GRAVARPRODUTO()
        {
            strSql = "INSERT INTO VENDA(VENDAID, PRODUTOID, DESCRICAO, QUANTIDADE, VALORU, VALORT) VALUES (@VENDAID, @PRODUTOID, @DESCRICAO, @QUANTIDADE, @VALORU, @VALORT)";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            comando.Parameters.Add("@VENDAID", SqlDbType.Int).Value = int.Parse(vendaid);
            comando.Parameters.Add("@PRODUTOID", SqlDbType.Int).Value = int.Parse(txCodigo.Text);
            comando.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = (txtDescricao.Text);
            comando.Parameters.Add("@QUANTIDADE", SqlDbType.Int).Value = int.Parse(txtQuantidade.Text);
            comando.Parameters.Add("@VALORU", SqlDbType.Decimal).Value = decimal.Parse(txtValorU.Text);
            comando.Parameters.Add("@VALORT", SqlDbType.Decimal).Value = decimal.Parse(txtValorTotal.Text);
            try
            {
                if (txtQuantidade.Text == string.Empty)
                {
                    throw new Exception("O CAMPO QUANTIDADE NÃO PODE ESTAR EM BRANCO!");
                }
                sqlCon.Open();
                comando.ExecuteNonQuery();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                SELECT();
                SELECTSUN();
            }
        }
        private void SELECTSUN()
        {

            strSql = "SELECT SUM(VALORT) FROM VENDA WHERE VENDAID = " + vendaid + "";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            sqlCon.Open();
            txtTotal.Text = comando.ExecuteScalar().ToString();
            sqlCon.Close(); 
        }
        string valorunitario;
        private void txCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            strSql = "SELECT PRODUTO, VALOR FROM PRODUTO WHERE CODIGO = " + txCodigo.Text + "";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            try
            {
                if (txCodigo.Text == string.Empty)
                {
                    throw new Exception("O CAMPO CÓDIGO NÃO PODE ESTAR EM BRANCO!");
                }
                sqlCon.Open();
                SqlDataReader dr = comando.ExecuteReader();
                if (dr.HasRows == false)
                {
                    throw new Exception("CÓDIGO NÃO CADASTRADO!");
                }
                dr.Read();

                txtDescricao.Text = Convert.ToString(dr["PRODUTO"]);
                valorunitario = Convert.ToString(dr["VALOR"]);
                txtValorU.Text = valorunitario.ToString();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void txtQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            if(txCodigo.Text == string.Empty)
            {
                MessageBox.Show("O CAMPO CÓDIGO NÃO PODE ESTAR EM BRANCO!");
            }
            if (txtQuantidade.Text == string.Empty)
            {
                MessageBox.Show("O CAMPO CÓDIGO NÃO PODE ESTAR EM BRANCO!");
            }
            else
            {
                decimal quantidade = decimal.Parse(txtQuantidade.Text);
                decimal valortotal = decimal.Parse(valorunitario) * quantidade;
                txtValorTotal.Text = valortotal.ToString();
            }
        }

        private void btnConcVend_Click(object sender, EventArgs e)
        {
            strSql = "INSERT INTO VALORFINAL(VENDAID, VALORFINAL) VALUES (@VENDAID, @VALORFINAL)";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            comando.Parameters.Add("@VENDAID", SqlDbType.Int).Value = int.Parse(vendaid);
            comando.Parameters.Add("@VALORFINAL", SqlDbType.Decimal).Value = decimal.Parse(txtTotal.Text);
            try
            {
                if (txtTotal.Text == string.Empty)
                {
                    MessageBox.Show("O VALOR TOTAL NÃO PODE ESTAR EM BRANCO");
                }
                sqlCon.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("VENDA REALIZADA COM SUCESSO!");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                VENDA_CONCLUIDA vc = new VENDA_CONCLUIDA();
                vc.ShowDialog();
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            INICIAL i = new INICIAL();
            i.ShowDialog();
        }

   

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
           // this.Visible = false;
            INICIAL i = new INICIAL();
            i.ShowDialog();
        }

        private void VENDA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Deseja realizar uma nova venda?", "Atenção", MessageBoxButtons.YesNo,MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                VENDA v = new VENDA();
                v.ShowDialog();
            }
            else
            {
                e.Cancel = false;
            }
           // if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
                
              //  e.Cancel = true;
            //}
        }
    }
}
