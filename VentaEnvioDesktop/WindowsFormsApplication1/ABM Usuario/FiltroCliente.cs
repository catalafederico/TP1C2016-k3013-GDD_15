﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MercadoEnvio.Utils;

namespace WindowsFormsApplication1.ABM_Usuario
{
    public partial class FiltroCliente : Form
    {
        String wheres;
        public FiltroCliente()
        {
            InitializeComponent();
            inicializar();
            wheres = "";
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonFiltrar_Click(object sender, EventArgs e)
        {
            if(txtApellido.Text == "" && txtDNI.Text == "" && txtEmail.Text == "" && txtNombre.Text == "")
            {
                MessageBox.Show("Debe ingresar al menos algún filtro", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else 
            {
                wheres = "";
                armarWheres();
                CompletadorDeTablas.hacerQuery("SELECT C_USUARIO_NOMBRE Username, D_NOMBRES Nombres, D_APELLIDOS Apellidos, N_DOCUMENTO DNI, C_CORREO Mail FROM GDD_15.USUARIOS U JOIN GDD_15.CLIENTES C ON (U.N_ID_USUARIO = C.N_ID_USUARIO) WHERE " + wheres, ref dataGridView1);
            }
        }

        public void armarWheres()
        {
            if(txtNombre.Text != "")
            {
                wheres = wheres + "D_NOMBRES LIKE '%" + txtNombre.Text + "%' AND ";
            }
            if (txtApellido.Text != "")
            {
                wheres = wheres + "D_APELLIDOS LIKE '%" + txtApellido.Text + "%' AND ";
            }
            if (txtEmail.Text != "")
            {
                wheres = wheres + "C_CORREO LIKE '%" + txtEmail.Text + "%' AND ";
            }
            if (txtDNI.Text != "")
            {
                wheres = wheres + "N_DOCUMENTO = '" + txtDNI.Text + "' AND ";
            }

            wheres = wheres.Substring(0, wheres.Length - 5);
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            inicializar();
        }

        private void inicializar()
        {
            CompletadorDeTablas.hacerQuery("SELECT C_USUARIO_NOMBRE Username, D_NOMBRES Nombres, D_APELLIDOS Apellidos, N_DOCUMENTO DNI, C_CORREO Mail FROM GDD_15.USUARIOS U JOIN GDD_15.CLIENTES C ON (U.N_ID_USUARIO = C.N_ID_USUARIO)", ref dataGridView1);
            txtApellido.Text = "";
            txtDNI.Text = "";
            txtEmail.Text = "";
            txtNombre.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
