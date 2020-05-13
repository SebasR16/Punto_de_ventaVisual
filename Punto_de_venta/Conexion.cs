using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Punto_de_venta
{
    class Conexion
    {
        private MySqlConnection connection;
        private string Hostname;
        private string database;
        private string user;
        private string password;

        public Conexion()
        {
            iniciar();
        }
        private void iniciar()
        {
            try
            {
                Hostname = "sql3.freesqldatabase.com";
                database = "sql3339480";
                user = "sql3339480";
                password = "rpSgcK6fmM";
                string connectionString;
                connectionString = "SERVER=" + Hostname + ";" + "DATABASE=" +
                database + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(connectionString);

            }
            catch (Exception e)
            {
                Console.WriteLine("No se conecto: " + e.Message);
            }
        }
        private bool OpenConnection()
        {
            try
            {
                Console.WriteLine("IT DOES ENTER TO OPENCONNECTION");
                connection.Open();
                Console.WriteLine("IT DOES OPEN THE CONNECTION");
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("IT DOES OPEN THE CATCH " + ex.Number);
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                /*switch (ex.Number)
                {
                    case 0:
                        try
                        {
                            Console.WriteLine("IT DOES ENTER CASE 0");
                            MessageBox.Show("Cannot connect to server.  Working on local server.");
                            connection = new MySqlConnection("SERVER=192.168.0.12; PORT=3307; DATABASE=tiendaisi; UID=usr217210185; PWD=pw217210185;");

                            connection.Open();

                        }
                        catch (MySqlException e)
                        {
                            switch (e.Number)
                            {
                                case 0:
                                    MessageBox.Show("Can't connect to local server either.");
                                    break;
                            }
                        }
                        return true;
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                    case 1042:
                        try
                        {
                            Console.WriteLine("IT DOES ENTER CASE 0");
                            MessageBox.Show("Cannot connect to server.  Working on local server.");
                            connection = new MySqlConnection("SERVER=192.168.0.12; PORT=3307; DATABASE=tiendaisi; UID=usr217210185; PWD=pw217210185;");

                            connection.Open();

                        }
                        catch (MySqlException e)
                        {
                            switch (e.Number)
                            {
                                case 0:
                                    MessageBox.Show("Can't connect to local server either.");
                                    break;
                            }
                        }
                        return true;
                        break;
                }*/
                return false;
            }
        }


        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public List<Productos> getProductos()
        {
            String query = "SELECT * FROM producto";
            Console.WriteLine("IT DOES ENTER TO GETPRODUCTOS");
            List<Productos> productos = new List<Productos>();


            if (this.OpenConnection() == true)
            {
                Console.WriteLine("IT DOES ENTER TO GP IF");
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Productos p = new Productos();
                    p.Id = (int)dataReader["id"];
                    p.Nombre = (String)dataReader["Nombre"];
                    p.Descripcion = (String)dataReader["descripcion"];
                    p.Marca = (String)dataReader["marca"];
                    //p.Precio = (float)dataReader["precio"];
                    p.Precio = (decimal)dataReader["precio"];
                    p.Existencia = (int)dataReader["existencia"];
                    productos.Add(p);
                }
                dataReader.Close();
                this.CloseConnection();
                return productos;
            }
            else
            {
                return productos;
            }
        }
        public void Reduce(Productos p, int compras)
        {
            int Id = p.Id;
            string nombre = p.Nombre;
            String descripcion = p.Descripcion;
            String marca = p.Descripcion;
            int existencia = p.Existencia - compras;
            decimal precio = p.Precio;
            //int precio = p.Precio;

            string query = "UPDATE producto SET existencia =" + existencia + " WHERE id = " + Id;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }

        }
        public void Increment(int id, int existencia)
        {
            string query = "UPDATE producto SET existencia = " + existencia
                + " WHERE id = " + id;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }

        }
    }
}
