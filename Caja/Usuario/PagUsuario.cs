using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caja
{
    public partial class PagUsuario : Form
    {
        public PagUsuario()
        {
            InitializeComponent();
            lblDateTime.Text = DateTime.Now.ToString();
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if(cbListaProductos.SelectedItem != null)
            {
                string product = cbListaProductos.SelectedItem.ToString();
                lbFactura.Items.Add(product);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString();
        }
    }
}
