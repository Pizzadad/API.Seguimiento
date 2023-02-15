using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeformedFilesCleaned
{
    public partial class Form1 : Form
    {
        public ConsultaBL BL { get; private set; }

        private List<ConsultaDocumentosDto> _model;

        public List<ConsultaDocumentosDto> Model
        {
            get { return _model; }
            set
            {
                _model = value;
                bindingSource1.DataSource = value;
                bindingSource1.ResetBindings(true);
            }
        }

        public Form1()
        {
            BL = new ConsultaBL();
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;
                var ruc = txtruc.Text;
                var anio = int.Parse(txtanio.Text);
                var mes = int.Parse(txtmes.Text);
                Model = await BL.ObtenerDocumentosCorruptosPorRUC(ruc, anio, mes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> idstodelete = new List<int>();
                foreach (var item in Model)
                {
                    try
                    {
                        var stream = new StreamReader(new MemoryStream(item.xmlbinary));
                        var r = stream.ReadLine();
                        var ext = r.StartsWith("PK") ? "zip" : "xml";
                        if (ext.Equals("zip"))
                            idstodelete.Add(item.id);
                    }
                    catch (Exception ex)
                    {
                        item.error = ex.Message;
                    }
                }

                Debug.WriteLine(string.Join(", ", idstodelete));

                bindingSource1.ResetBindings(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
