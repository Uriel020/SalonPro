using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caja
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Contraseña().Show();
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string UsuarioAdmin = "admin";
            string ClaveAdmin = "admin";

            if(txtUser.Text != UsuarioAdmin && txtClave.Text != ClaveAdmin)
            {
                new PagUsuario().Show();
            }
            else
            {
                new PagAdmin().Show();
            }
        }
    }
}
