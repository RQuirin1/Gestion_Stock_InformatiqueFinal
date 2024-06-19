using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Stock_Informatique
{
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if ((textBoxLog.Text == "jgontard") && (textBoxMdp.Text == "CCBD+05"))
            {
                GestionStock gest = new GestionStock();
                gest.ShowDialog();
                
                textBoxLog.Text = "";
                textBoxMdp.Text = "";
            }
            else
            {
                MessageBox.Show("Erreur : Login ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxLog_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLog.Text) &&
               !String.IsNullOrEmpty(textBoxMdp.Text))
            {
                buttonSubmit.Enabled = true;
            }
            else
            {
                buttonSubmit.Enabled = false;
            }
        }

        private void textBoxMdp_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLog.Text) &&
               !String.IsNullOrEmpty(textBoxMdp.Text))
            {
                buttonSubmit.Enabled = true;
            }
            else
            {
                buttonSubmit.Enabled = false;
            }
        }

        private void textBoxMdp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSubmit_Click(sender, e);
            }
        }

        private void Log_Load(object sender, EventArgs e)
        {
            textBoxLog.Select();
        }
    }
}
