﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

using MercadoEnvio.Utils;

namespace WindowsFormsApplication1
{
    public partial class Inicio : Form
    {

        private string username, password;
        Elegir_Rol.EleccionRol eleccion;
        Elegir_Funcionalidad.EleccionFuncionalidad funcionalidades;

        public Inicio()
        {
            InitializeComponent();

            DateTime fecha = DateTime.Parse(Program.nuevaFechaSistema());
            labelDia.Text = fecha.Day.ToString() + "/" + fecha.Month + "/" + fecha.Year;

            string comando = "execute GDD_15.FINALIZAR_PUBLIS '" + fecha + "'";
            DataTable dt6 = (new ConexionSQL()).cargarTablaSQL(comando);
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
           // CompletadorDeTablas.hacerQuery("select top 6 * from gd_esquema.Maestra", ref dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
         
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }  

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }
            

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void BotonLogin_Click(object sender, EventArgs e)
        {
            username = txtNombre.Text;
            password = txtPass.Text;

            if (username == null || password == null || password == "" || username == "")
            {
                MessageBox.Show("Debe ingresar su nombre de usuario y contraseña",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                string query = "SELECT C_PASSWORD FROM GDD_15.USUARIOS WHERE C_USUARIO_NOMBRE = '" + username + "'";
                DataTable dt = (new ConexionSQL()).cargarTablaSQL(query);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Nombre de usuario incorrecto",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Warning);
                } 
                else 
                {
                    String passwordSistema = dt.Rows[0][0].ToString();            
                    if (getSha256(password) == passwordSistema)
                    {
                        string resetearIntentos = "UPDATE GDD_15.USUARIOS SET N_CANT_INTENTOS = 0 WHERE C_USUARIO_NOMBRE = '" + username + "'";
                        (new ConexionSQL()).ejecutarComandoSQL(resetearIntentos);
                        string query5 = "SELECT N_HABILITADO FROM GDD_15.USUARIOS WHERE C_USUARIO_NOMBRE = '" + username + "'";
                        DataTable dt5 = (new ConexionSQL()).cargarTablaSQL(query5);
                        string habilitado = dt5.Rows[0][0].ToString();
                        if (habilitado == "1")
                        {
                            string query2 = "SELECT COUNT(*) FROM GDD_15.ROLES_USUARIOS ROLES JOIN GDD_15.USUARIOS USUARIOS ON (USUARIOS.N_ID_USUARIO = ROLES.N_ID_USUARIO) WHERE USUARIOS.C_USUARIO_NOMBRE = '" + username + "' AND ROLES.N_HABILITADO = 1";
                            DataTable dt2 = (new ConexionSQL()).cargarTablaSQL(query2);
                            string cantidadRoles = dt2.Rows[0][0].ToString();
                            if (cantidadRoles == "1")
                            {
                                DataTable dt3 = (new ConexionSQL()).cargarTablaSQL("SELECT R.C_ROL FROM GDD_15.ROLES_USUARIOS RU JOIN GDD_15.USUARIOS U ON (U.N_ID_USUARIO = RU.N_ID_USUARIO) JOIN GDD_15.ROLES R ON (R.N_ID_ROL = RU.N_ID_ROL) WHERE U.C_USUARIO_NOMBRE = '" + username + "' AND R.N_HABILITADO = 1 AND RU.N_HABILITADO = 1");
                                string rol = dt3.Rows[0][0].ToString();
                                funcionalidades = new Elegir_Funcionalidad.EleccionFuncionalidad(rol, username);
                                funcionalidades.ShowDialog();
                            }
                            else if (cantidadRoles == "0")
                            {
                                MessageBox.Show("El usuario no tiene roles habilitados", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                eleccion = new Elegir_Rol.EleccionRol(username);
                                eleccion.ShowDialog();
                            }
                        }
                        else
                        {
                            MessageBox.Show("El usuario no esta habilitado", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string query4 = "SELECT N_CANT_INTENTOS FROM GDD_15.USUARIOS WHERE C_USUARIO_NOMBRE = '" + username + "'";
                        DataTable dt4 = (new ConexionSQL()).cargarTablaSQL(query4);
                        string cantidadIntentos = dt4.Rows[0][0].ToString();
                        string texto = "";
                        if (cantidadIntentos == "0")
                        {
                            cantidadIntentos = "1";
                            texto = "";
                        }
                        else if (cantidadIntentos == "1")
                        {
                            cantidadIntentos = "2";
                            texto = "";
                        }
                        else if (cantidadIntentos == "2")
                        {
                            cantidadIntentos = "3";
                            texto = ": Usuario inhabilitado";
                            string inhabilitarUsuario = "UPDATE GDD_15.USUARIOS SET N_HABILITADO = 0 WHERE C_USUARIO_NOMBRE = '" + username + "'";
                            (new ConexionSQL()).ejecutarComandoSQL(inhabilitarUsuario);

                        }
                        string sumarIntento = "UPDATE GDD_15.USUARIOS SET N_CANT_INTENTOS = '" + cantidadIntentos + "' WHERE C_USUARIO_NOMBRE = '" + username + "'";
                        (new ConexionSQL()).ejecutarComandoSQL(sumarIntento);
                        MessageBox.Show("Contraseña incorrecta" + texto,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
        }
            public String getSha256(String input)
            {
            SHA256Managed encriptador = new SHA256Managed();
            byte[] inputEnBytes = Encoding.UTF8.GetBytes(input);
            byte[] inputHashBytes = encriptador.ComputeHash(inputEnBytes);
            return BitConverter.ToString(inputHashBytes).Replace("-", String.Empty).ToLower();
            }

        }
}

