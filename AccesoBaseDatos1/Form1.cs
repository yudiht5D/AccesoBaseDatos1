using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 

namespace AccesoBaseDatos1
{
    public partial class Form1 : Form
    {
        private ClaseMSQL ejecutaMySql = new ClaseMSQL();
        private ClaseSQLServer ejecutaSqlServer = new ClaseSQLServer();

        private string Servidor = "YG\\SQLEXPRESS";
        private string Basedatos = "master";
        private string UsuarioId = "sa";
        private string Password = "tortuga";

        private void EjecutaComando(string ConsultaSQL)
        {
            try
            {
                if (chkSQLServer.Checked)
                    ejecutaSqlServer.EjecutarComando(ConsultaSQL);
                else if (chkMySQL.Checked)
                    ejecutaMySql.EjecutarComando(ConsultaSQL);


                llenarGrid();

            }
            catch (SqlException Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error en el sistema");
            }
        }
        private void llenarGrid()
        {
            try
            {
                string strConn = $"Server={Servidor};" +
                    $"Database={Basedatos};" +
                    $"User Id={UsuarioId};" +
                    $"Password={Password}";

                if (chkSQLServer.Checked)
                {
                    SqlConnection conn = new SqlConnection(strConn);
                    conn.Open();

                    string sqlQuery = "select * from Alumnos";
                    SqlDataAdapter adp = new SqlDataAdapter(sqlQuery, conn);

                    DataSet ds = new DataSet();
                    adp.Fill(ds, "Alumnos");
                    dgvAlumnos.DataSource = ds.Tables[0];
                }

                dgvAlumnos.Refresh();
            }
            catch (SqlException Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error en el sistema");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCrearBD_Click(object sender, EventArgs e)
        {
            try
            {              
                string strConn = $"Server={Servidor};" +
                    $"Database=master;" +
                    $"User Id={UsuarioId};" +
                    $"Password={Password}";

                if (chkSQLServer.Checked)
                {
                    SqlConnection conn = new SqlConnection(strConn);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CREATE DATABASE ESCOLAR";
                    cmd.ExecuteNonQuery();

                }


            }
            catch (SqlException  Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            catch (Exception Ex )
            {
                MessageBox.Show("Error en el sistema");
            }
        }

        private void btnCreaTabla_Click(object sender, EventArgs e)
        {
            EjecutaComando("CREATE TABLE " +
                    "Alumnos (NoControl varchar(10), nombre varchar(50), carrera int)");
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoControl.Text) ||
               string.IsNullOrWhiteSpace(txtNombre.Text) ||
               string.IsNullOrWhiteSpace(txtCarrera.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            string consulta = $"INSERT INTO Alumnos (NoControl, Nombre, Carrera) VALUES ('{txtNoControl.Text}', '{txtNombre.Text}', {txtCarrera.Text})";
            EjecutaComando(consulta);
        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoControl.Text))
            {
                MessageBox.Show("Debe ingresar un número de control válido");
                return;
            }

            string consulta = $"DELETE FROM Alumnos WHERE NoControl = '{txtNoControl.Text}'";
            EjecutaComando(consulta);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chkSQLServer.Checked = true;
            llenarGrid();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void btnCrearBD_Click_1(object sender, EventArgs e)
        {
            try
            {
                string strConn = $"Server={Servidor};" +
                    $"Database=master;" +
                    $"User Id={UsuarioId};" +
                    $"Password={Password}";

                if (chkSQLServer.Checked)
                {
                    SqlConnection conn = new SqlConnection(strConn);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CREATE DATABASE ESCOLAR";
                    cmd.ExecuteNonQuery();

                }


            }
            catch (SqlException Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error en el sistema");
            }
        }

        private void btnCreaTabla_Click_1(object sender, EventArgs e)
        {
            EjecutaComando("CREATE TABLE " +
                  "Alumnos (NoControl varchar(10), nombre varchar(50), carrera int)");
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoControl.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtCarrera.Text))
            {
                MessageBox.Show("Es necesario llenar todos los campos");
                return;
            }

            string consulta = $"UPDATE Alumnos SET Nombre = '{txtNombre.Text}', Carrera = {txtCarrera.Text} WHERE NoControl = '{txtNoControl.Text}'";
            EjecutaComando(consulta);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoControl.Text))
            {
                MessageBox.Show("Debe ingresar un número de control válido");
                return;
            }

            string consulta = $"DELETE FROM Alumnos WHERE NoControl = '{txtNoControl.Text}'";
            EjecutaComando(consulta);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoControl.Text))
            {
                MessageBox.Show("Ingrese un número de control válido para buscar");
                return;
            }

            DataTable datos = chkSQLServer.Checked
                ? ejecutaSqlServer.ObtenerDatos($"SELECT * FROM Alumnos WHERE NoControl = '{txtNoControl.Text}'")
                : ejecutaMySql.ObtenerDatos($"SELECT * FROM Alumnos WHERE NoControl = '{txtNoControl.Text}'");

            if (datos.Rows.Count > 0)
            {
                dgvAlumnos.DataSource = datos;
                dgvAlumnos.Refresh();
            }
            else
            {
                MessageBox.Show("No se encontraron registros.");
            }
        }
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }
    }
}
    

