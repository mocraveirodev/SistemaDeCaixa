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
    public partial class PRODUTO : Form
    {
        public PRODUTO()
        {
            InitializeComponent();
            CODIGO();
        }
        SqlConnection sqlCon = null;
        SqlDataAdapter da = null;
        private string strCon = @"Password=1234qwer;Persist Security Info=True;User ID=sa;Initial Catalog=CAIXA;Data Source=MONICANOTEBOOK\SQLEXPRESS";
        private string strSql = string.Empty;
        DataTable produto = null;

        private void CODIGO()
        {
            strSql = "SELECT MAX((CODIGO)+1) FROM PRODUTO";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            sqlCon.Open();
            txtCadCod.Text = comando.ExecuteScalar().ToString();
            sqlCon.Close();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            strSql = "INSERT INTO PRODUTO(CODIGO, PRODUTO, MARCA, QUANTIDADE, CATEGORIA, DATAVALIDADE, VALOR) VALUES (@CODIGO, @PRODUTO, @MARCA, @QUANTIDADE, @CATEGORIA, @DATAVALIDADE, @VALOR)";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);

            comando.Parameters.Add("@CODIGO", SqlDbType.Int).Value = int.Parse(txtCadCod.Text);
            comando.Parameters.Add("@PRODUTO", SqlDbType.VarChar).Value = (txtCadProduto.Text);
            comando.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = (txtCadMarca.Text);
            comando.Parameters.Add("@QUANTIDADE", SqlDbType.Int).Value = int.Parse(ndCadQuantidade.Text);
            comando.Parameters.Add("@CATEGORIA", SqlDbType.VarChar).Value = (cbCadCategoria.Text);
            comando.Parameters.Add("@DATAVALIDADE", SqlDbType.VarChar).Value = (mtCadValidade.Text);
            comando.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = Decimal.Parse(txtCadValor.Text);
            try
            {
                if (txtCadProduto.Text == string.Empty)
                {
                    throw new Exception("O CAMPO PRODUTO NÃO PODE ESTAR EM BRANCO");
                }
                sqlCon.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("DADOS CADASTRADOS COM SUCESSO!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void btnAltPesquisa_Click(object sender, EventArgs e)
        {
            strSql = "SELECT * FROM PRODUTO WHERE CODIGO = " + txtAltPesquisa.Text + "";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);

            try
            {
                if(txtAltPesquisa.Text == String.Empty)
                {
                    throw new Exception("VOCÊ PRECISA DIGITAR UM CÓDIGO!");
                }
                sqlCon.Open();
                SqlDataReader dr = comando.ExecuteReader();

                if(dr.HasRows == false)
                {
                    throw new Exception("CÓDIGO NÃO CADASTRADO!");
                }

                dr.Read();
                txtAltCod.Text = Convert.ToString(dr["CODIGO"]);
                txtAltProduto.Text = Convert.ToString(dr["PRODUTO"]);
                txtAltMarca.Text = Convert.ToString(dr["MARCA"]);
                ndAltQuantidade.Text = Convert.ToString(dr["QUANTIDADE"]);
                cbAltCategoria.Text = Convert.ToString(dr["CATEGORIA"]);
                mbAltValidade.Text = Convert.ToString(dr["DATAVALIDADE"]);
                txtAltValor.Text = Convert.ToString(dr["VALOR"]);
                btnAlterar.Enabled = true;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            strSql = "UPDATE PRODUTO SET PRODUTO = @PRODUTO, MARCA = @MARCA, QUANTIDADE = @QUANTIDADE, CATEGORIA = @CATEGORIA, DATAVALIDADE = @DATAVALIDADE, VALOR = @VALOR WHERE CODIGO = @CODIGOBUSCA";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            comando.Parameters.Add("@CODIGOBUSCA", SqlDbType.Int).Value = int.Parse (txtAltCod.Text);
            comando.Parameters.Add("@PRODUTO", SqlDbType.VarChar).Value = (txtAltProduto.Text);
            comando.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = (txtAltMarca.Text);
            comando.Parameters.Add("@QUANTIDADE", SqlDbType.Int).Value = int.Parse(ndAltQuantidade.Text);
            comando.Parameters.Add("@CATEGORIA", SqlDbType.VarChar).Value = (cbAltCategoria.Text);
            comando.Parameters.Add("@DATAVALIDADE", SqlDbType.VarChar).Value = (mbAltValidade.Text);
            comando.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = Decimal.Parse(txtAltValor.Text);

            try
            {
                sqlCon.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("PRODUTO ATUALIZADO COM SUCESSO!");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void btnExcPesq_Click(object sender, EventArgs e)
        {
            strSql = "SELECT PRODUTO, MARCA, QUANTIDADE FROM PRODUTO WHERE CODIGO = " + txtExcCod.Text + "";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);

            try
            {
                if (txtExcCod.Text == String.Empty)
                {
                    throw new Exception("VOCÊ PRECISA DIGITAR UM CÓDIGO!");
                }
                sqlCon.Open();
                SqlDataReader dr = comando.ExecuteReader();

                if (dr.HasRows == false)
                {
                    throw new Exception("CÓDIGO NÃO CADASTRADO!");
                }

                dr.Read();
                txtExcProd.Text = Convert.ToString(dr["PRODUTO"]);
                txtExcMarca.Text = Convert.ToString(dr["MARCA"]);
                ndExcQuantidade.Text = Convert.ToString(dr["QUANTIDADE"]);
                btnExcluir.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            strSql = "DELETE FROM PRODUTO WHERE CODIGO = " + txtExcCod.Text + "";
            sqlCon = new SqlConnection(strCon);
            SqlCommand comando = new SqlCommand(strSql, sqlCon);
            try
            {
                sqlCon.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("PRODUTO EXLCUÍDO COM SUCESSO!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }
        private void CARREGAR(string SQLConsultaGeral)
        {
            try
            {
                sqlCon = new SqlConnection(strCon);
                da = new SqlDataAdapter(SQLConsultaGeral, sqlCon);
                produto = new DataTable();
                da.Fill(produto);
                produto.Columns[0].ColumnName = "CODIGO";
                produto.Columns[1].ColumnName = "PRODUTO";
                produto.Columns[2].ColumnName = "MARCA";
                produto.Columns[3].ColumnName = "QUANTIDADE";
                produto.Columns[4].ColumnName = "CATEGORIA";
                produto.Columns[5].ColumnName = "DATAVALIDADE";
                produto.Columns[6].ColumnName = "VALOR";
                dataGridView1.DataSource = produto;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtEstoque_KeyUp(object sender, KeyEventArgs e)
        {
            string strSQL = "SELECT * FROM PRODUTO WHERE CODIGO LIKE '" + txtEstoque.Text + "%'";
            CARREGAR(strSQL);
        }

        private void iNÍCIOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            INICIAL i = new INICIAL();
            i.ShowDialog();
        }

        private void pRODUTOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            PRODUTO p = new PRODUTO();
            p.ShowDialog();
        }

        private void vENDAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            VENDA v = new VENDA();
            v.ShowDialog();
        }

        private void sAIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }            
        }

        private void btnCadSair_Click(object sender, EventArgs e)
        {
           // this.Visible = false;
            INICIAL i = new INICIAL();
            i.ShowDialog();
        }

        private void btnAltSair_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            INICIAL i = new INICIAL();
            i.ShowDialog();
        }

        private void btnExcSair_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            INICIAL i = new INICIAL();
            i.ShowDialog();
        }

        private void btnCadLimpar_Click(object sender, EventArgs e)
        {

            this.txtCadProduto.Text = "";
            this.txtCadMarca.Text = "";
            this.ndCadQuantidade.Value = 0;
            this.cbCadCategoria.Text = "";
            this.mtCadValidade.Text = "";
            this.txtCadValor.Text = "";
        }

        private void btnAltLimpar_Click(object sender, EventArgs e)
        {
            txtAltCod.Text = "";
            txtAltProduto.Text = "";
            txtAltMarca.Text = "";
            ndAltQuantidade.Value = 0;
            cbAltCategoria.Text = "";
            mbAltValidade.Text = "";
            txtAltValor.Text = "";
        }

        private void btnExcLimpar_Click(object sender, EventArgs e)
        {
            txtExcProd.Text = "";
            txtExcMarca.Text = "";
            ndExcQuantidade.Value = 0;
        }

        private void PRODUTO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnEstPesq_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT * FROM PRODUTO WHERE CODIGO LIKE '" + txtEstoque.Text + "%'";
            CARREGAR(strSQL);
        }
    }
}
