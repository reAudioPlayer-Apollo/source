using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class GameAdder : Form
    {
        public GameAdder(string file)
        {
            InitializeComponent();
            txtPath.Text = file;
        }

        private void GameAdder_Load(object sender, EventArgs e)
        {

        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(txtPath.Text))
            {
                txtTitle.Text = Path.GetFileNameWithoutExtension(txtPath.Text);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GameChecker.addGameToJson(txtTitle.Text, Path.GetFileName(txtPath.Text));
            this.Close();
        }
    }
}
