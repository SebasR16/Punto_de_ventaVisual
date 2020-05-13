using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_venta
{
    public partial class Form1 : Form
    {
        Conexion c = null;
        List<Productos> productos = null;
        List<Productos> vendidos = null;
        //variable de clase 
        /*private String[,] productos =
        {
            {"128468579216","Burro percheron","100"},
            {"127813549721","Taco de asada","25"},
            {"178149325487","Caramelo","35"},
            {"151873204809","Dogo","20"},
            {"094156349284","Pizza","50"},
            {"594684123498","Refresco","20"},
            {"846124863084","Elote","10"},
            {"248765108846","Sopa de Macaco","17"},
            {"948175230843","Tochos","32"},
            {"348147895032","Cloro","5"},
        };*/
        private void buscarProductos()
        {
            if (textBox1.Text.IndexOf('*') != -1)
            {
                String[] arre = textBox1.Text.Split('*');
                for (int i = 0; i < productos.Count; i++)
                {
                    try
                    {
                        if (int.Parse(arre[1]) == productos[i].Id)
                        {
                            if (int.Parse(arre[0]) > productos[i].Existencia)
                            {
                                MessageBox.Show("Solamente hay " + productos[i].Existencia + " en existencia de ese producto.");
                            }
                            else
                            {
                                
                                Productos p = productos[i];
                                Console.Write("Aqui esta el subtotal:   "+ p.Precio * decimal.Parse(arre[0]));
                                dataGridView1.Rows.Add(p.Nombre, p.Marca, p.Descripcion, p.Precio, arre[0], p.Precio*decimal.Parse(arre[0]));
                                total();
                                c.Reduce(p, int.Parse(arre[0]));
                                vendidos.Add(p);
                            }

                        }
                        

                    }
                    catch
                    {
                        MessageBox.Show("Ingrese una cantidad válida");
                        textBox1.Clear();
                    }
                }
            }
            else
            {
                int code;
                try
                {
                    code = int.Parse(textBox1.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un código válido");
                    code = 0;
                    textBox1.Clear();
                }
                for (int i = 0; i < productos.Count; i++)
                {

                    if (code == productos[i].Id)
                    {
                        if (productos[i].Existencia == 0)
                        {
                            MessageBox.Show("No hay ese producto en existencia");
                        }
                        else
                        {
                            Productos p = productos[i];
                            dataGridView1.Rows.Add(p.Nombre, p.Marca, p.Descripcion, p.Precio, "1",p.Precio);
                            total();
                            c.Reduce(p, 1);
                            vendidos.Add(p);
                        }

                    }
                    
                }


            }
            this.ActiveControl = textBox1;
            productos = c.getProductos();
        }

        private float total()
        {
            float total = 0.00f;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("Rows = " + dataGridView1.Rows.Count);
                total += float.Parse(dataGridView1[5, i].Value.ToString());
                Console.WriteLine("Total = " + total);
            }
            label3.Text = "Total = " + total;
            textBox1.Clear();
            textBox1.Focus();
            return total;
        }
        public Form1()
        {
            InitializeComponent();
            c = new Conexion();
            productos = c.getProductos();
            vendidos = new List<Productos>();
            for (int i = 0; i < productos.Count; i++)
            {
                Console.WriteLine(productos[i].Nombre);
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            label1.Location = new Point((this.Width/2)-(label1.Width/2),0);
            label2.Text = DateTime.Now.ToLongDateString()+" "+ DateTime.Now.ToLongTimeString();
            
            label2.Location = new Point((this.Width / 2) - (label2.Width / 2), label1.Height + 1);
            dataGridView1.Width = this.Width-10;
            dataGridView1.Height = this.Height * 3/4;
            dataGridView1.Location = new Point(5, label1.Height+label2.Height + 1);
            textBox1.Width = this.Width;
            textBox1.Location = new Point(0, this.Height - (textBox1.Height-3));
            label3.Location = new Point(this.Width - dataGridView1.Columns[3].Width, label1.Height + dataGridView1.Height + 30);
            button1.Location = new Point(this.Width - dataGridView1.Columns[3].Width - label3.Width -10, label1.Height + dataGridView1.Height + 30);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter)
            {

                productos = c.getProductos();
                buscarProductos();
                textBox1.Text = String.Empty;

            }
            if (e.KeyCode == Keys.Escape)
            {
                if (dataGridView1.Rows.Count >= 1)
                {
                    Productos p = vendidos[vendidos.Count - 1];
                    c.Increment(p.Id, p.Existencia);
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    vendidos.Remove(p);
                    total();
                }
            }
            
            if (e.KeyCode == Keys.S)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    MessageBox.Show("Es necesario terminar la venta antes de salir del sistema");
                    textBox1.Clear();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir?", "Salir", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        textBox1.Clear();
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (vendidos.Count >= 1)
            {
                DialogResult dialogResult = MessageBox.Show("¿Vas a pagar?", "Pagar", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    dataGridView1.Rows.Clear();
                    vendidos.RemoveRange(0, vendidos.Count);
                    label3.Text = "Total = ";


                }
                else if (dialogResult == DialogResult.No)
                {
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("No puede pagar sin ninguna venta");
                textBox1.Focus();
                textBox1.Clear();
            }
        }
    }
}
