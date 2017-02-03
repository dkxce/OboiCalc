using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace OboiCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public double right_real(string input)
        {
            double d = 0;
            if (!double.TryParse(input, out d))
                if (!double.TryParse(input.Replace(",", "."), out d))
                    double.TryParse(input.Replace(".", ","), out d);
            return d;
        }

        private void mywidth_TextChanged(object sender, EventArgs e)
        {
            double ml = right_real(mylength.Text);
	        double mw = right_real(mywidth.Text);
	        perimetr.Text = ((ml + mw) * 2).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadOBC();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double cp = 0;
            double pp = right_real(perimetr.Text)*100;
	        double ww = right_real(wwidth.Text);
            double ll = right_real(lleight.Text);
	        if (ww!=0) cp = Math.Ceiling(pp / ww);            
	
	        double rap = right_real(rapport.Text);
	
            double sdv = 0;
	        if (sdvig.Checked) sdv = rap / 2; 
	
	        double zap = 10;
            if(zapas.SelectedIndex == 1) zap = 15;
	
	        double hp = right_real(pheight.Text)*100 + zap;
            double hp_real = 0;
	        if (rap!=0) 
            {
                double povtor = Math.Ceiling(hp / rap);
                hp_real = povtor * rap + sdv;
            } else {
                hp_real = hp;
            };

            double result = Math.Ceiling(hp_real * cp / 100);
	        res1.Text = result.ToString();
            res2.Text = Math.Ceiling(result / ll).ToString();	        
        }

        public OboiConfig obc = new OboiConfig();
        private void новыйПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            obc = new OboiConfig();
            LoadOBC();
        }

        public void LoadOBC()
        {
            LoadOBC("");
        }

        public void LoadOBC(string fileName)
        {
            if (fileName == "") Text = "Калькулятор обоев [Безымянный]";

            mywidth.Text = obc.FloorWidth.ToString();
            mylength.Text = obc.FloorHeight.ToString();
            pheight.Text = obc.WallHeight.ToString();
            mywidth_TextChanged(this, null);

            lleight.Text = obc.OboiLength.ToString();
            wwidth.Text = obc.OboiWidth.ToString();
            rapport.Text = obc.OboiRapport.ToString();
            sdvig.Checked = obc.Sdvig;
            if (obc.Zapas == 10)
                zapas.SelectedIndex = 0;
            else
                zapas.SelectedIndex = 1;

            button1_Click(this, null);
        }

        public void SaveOBC()
        {
            obc.FloorWidth = right_real(mywidth.Text);
            obc.FloorHeight = right_real(mylength.Text);
            obc.WallHeight = right_real(pheight.Text);

            obc.OboiLength = right_real(lleight.Text);
            obc.OboiWidth = right_real(wwidth.Text);
            obc.OboiRapport = right_real(rapport.Text);
            obc.Sdvig = sdvig.Checked;
            if (zapas.SelectedIndex == 0)
                obc.Zapas = 10;
            else
                obc.Zapas = 15;
        }

        private void сохранитьПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveOBC();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".obcfg";
            sfd.Filter = "Файл конфигурации обоев (*.obcfg)|*.obcfg|ВСе типы файлов (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XMLSaved<OboiConfig>.Save(sfd.FileName, obc);
                Text = "Калькулятор обоев ["+ System.IO.Path.GetFileName(sfd.FileName) +"]";
                LoadOBC(sfd.FileName);
            };
            sfd.Dispose();
        }

        private void открытьПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".obcfg";
            ofd.Filter = "Файл конфигурации обоев (*.obcfg)|*.obcfg|ВСе типы файлов (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                obc = XMLSaved<OboiConfig>.Load(ofd.FileName);
                Text = "Калькулятор обоев [" + System.IO.Path.GetFileName(ofd.FileName) + "]";
                LoadOBC(ofd.FileName);
            };
            ofd.Dispose();
        }
    }

    public class OboiConfig : XMLSaved<OboiConfig>
    {
        public double FloorWidth = 0;
        public double FloorHeight = 0;
        public double WallHeight = 2.66;

        public double OboiLength = 10;
        public double OboiWidth = 106;
        public double OboiRapport = 0;
        public bool Sdvig = false;
        public double Zapas = 10;
    }
}