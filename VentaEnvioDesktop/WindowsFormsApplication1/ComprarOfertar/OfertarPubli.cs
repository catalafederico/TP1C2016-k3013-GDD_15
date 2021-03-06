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

namespace WindowsFormsApplication1.ComprarOfertar
{
    public partial class OfertarPubli : Form
    {
        String nombreUsuario;
        Int64 idPubli;
        Form form;
        float oferta;
        public OfertarPubli(Int64 idPubliPasado, String nombreUsuarioPasado, Form formPasado)
        {
            InitializeComponent();
            nombreUsuario = nombreUsuarioPasado;
            idPubli = idPubliPasado;
            form = formPasado;
            inicializar();

            calcularReputacion();
        }

        private void calcularReputacion()
        {
            string query5 = "SELECT N_ID_USUARIO FROM GDD_15.PUBLICACIONES WHERE N_ID_PUBLICACION = '" + idPubli + "'";
            DataTable dt5 = (new ConexionSQL()).cargarTablaSQL(query5);
            string usuarioID = dt5.Rows[0][0].ToString();

            string query = "SELECT COUNT(*), SUM(C_CALIFICACION) FROM GDD_15.PUBLICACIONES P JOIN GDD_15.OFERTAS O ON (O.N_ID_PUBLICACION = P.N_ID_PUBLICACION) JOIN GDD_15.CALIFICACIONES C ON (C.N_ID_OFERTA = O.N_ID_OFERTA) WHERE P.N_ID_USUARIO = '" + usuarioID + "'";
            DataTable dt = (new ConexionSQL()).cargarTablaSQL(query);
            Int64 cantOfertasCalif = Convert.ToInt64(dt.Rows[0][0].ToString());
            Int64 estrellasOfertas;
            if (cantOfertasCalif != 0)
            {
                estrellasOfertas = Convert.ToInt64(dt.Rows[0][1].ToString());
            }
            else
            {
                estrellasOfertas = 0;
            }

            string query2 = "SELECT COUNT(*), SUM(C_CALIFICACION) FROM GDD_15.PUBLICACIONES P JOIN GDD_15.COMPRAS CO ON (CO.N_ID_PUBLICACION = P.N_ID_PUBLICACION) JOIN GDD_15.CALIFICACIONES CA ON (CA.N_ID_COMPRA = CO.N_ID_COMPRA) WHERE P.N_ID_USUARIO = '" + usuarioID + "'";
            DataTable dt2 = (new ConexionSQL()).cargarTablaSQL(query2);
            Int64 cantComprasCalif = Convert.ToInt64(dt2.Rows[0][0].ToString());
            Int64 estrellasCompras;
            if (cantComprasCalif != 0)
            {
                estrellasCompras = Convert.ToInt64(dt2.Rows[0][1].ToString());
            }
            else
            {
                estrellasCompras = 0;
            }

            if (cantComprasCalif + cantOfertasCalif == 0)
            {
                txtReputacion.Text = "No tiene calificaciones";
                labelSobre100.Hide();
                return;
            }

            float promedio = (estrellasOfertas + estrellasCompras) / (cantComprasCalif + cantOfertasCalif);

            float factor = factorDeAjuste(cantComprasCalif + cantOfertasCalif);

            float reputacion = (promedio - 1) * 25 * factor;

            if (reputacion > 100)
            {
                reputacion = 100;
            }

            txtReputacion.Text = reputacion.ToString();
        }

        private float factorDeAjuste(Int64 cantidadCalif)
        {
            Int64 i = 0;
            float factor = 0.5F;

            while (i < cantidadCalif - 1)
            {
                factor = factor + ((float)1 / (float)Convert.ToInt64(Math.Pow(2, i + 2)));
                i++;
            }

            return factor + 0.1F;
        }

        public void inicializar()
        {
            txtCodPub.Text = idPubli.ToString();
            String idPubliText = idPubli.ToString();

            string query2 = "SELECT C_USUARIO_NOMBRE, P.D_DESCRED, R.D_DESCRED Rubro, N_PRECIO, F_VENCIMIENTO, C_PERMITE_ENVIO FROM GDD_15.PUBLICACIONES P JOIN GDD_15.USUARIOS U ON (P.N_ID_USUARIO = U.N_ID_USUARIO) JOIN GDD_15.RUBROS R ON (P.N_ID_RUBRO = R.N_ID_RUBRO) WHERE N_ID_PUBLICACION = '" + idPubliText + "'";
            DataTable dt2 = (new ConexionSQL()).cargarTablaSQL(query2);
            string nombreUsuarioPubli = dt2.Rows[0][0].ToString();
            txtNombreUsuario.Text = nombreUsuarioPubli;
            string descripPubli = dt2.Rows[0][1].ToString();
            txtDescrip.Text = descripPubli;
            string rubro = dt2.Rows[0][2].ToString();
            txtRubro.Text = rubro;
            string precio = dt2.Rows[0][3].ToString();
            txtPrecioInicial.Text = precio;
            string fechaVen = dt2.Rows[0][4].ToString();
            dateFechaVen.Text = fechaVen;
            string envio = dt2.Rows[0][5].ToString();
            if (envio == "SI")
            {

            }
            else if (envio == "NO")
            {
                chkEnvio.Enabled = false;
            }

            string  idClienteOfertaAnerior = "";

            string query = "SELECT MAX(N_MONTO) FROM GDD_15.PUBLICACIONES P JOIN GDD_15.OFERTAS O ON (P.N_ID_PUBLICACION = O.N_ID_PUBLICACION) WHERE P.N_ID_PUBLICACION = '" + idPubliText + "'";
            DataTable dt = (new ConexionSQL()).cargarTablaSQL(query);
            string ofertaAnterior = dt.Rows[0][0].ToString();

            string query3 = "SELECT N_ID_CLIENTE FROM GDD_15.PUBLICACIONES P JOIN GDD_15.OFERTAS O ON (P.N_ID_PUBLICACION = O.N_ID_PUBLICACION) WHERE P.N_ID_PUBLICACION = '" + idPubliText + "' AND N_MONTO = '" + ofertaAnterior + "'";
            DataTable dt3 = (new ConexionSQL()).cargarTablaSQL(query3);
            if (dt3.Rows.Count != 0)
            {
                idClienteOfertaAnerior = dt3.Rows[0][0].ToString();
            }
            string query5 = "SELECT N_ID_USUARIO FROM GDD_15.USUARIOS WHERE C_USUARIO_NOMBRE = '" + nombreUsuario + "'";
            DataTable dt5 = (new ConexionSQL()).cargarTablaSQL(query5);
            string usuarioID = dt5.Rows[0][0].ToString();

            if (idClienteOfertaAnerior == usuarioID)
            {
                MessageBox.Show("La última oferta fue realizada por usted entonces no puede volver a ofertar", this.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
                buttonGuardar.Enabled = false;
                chkEnvio.Enabled = false;
                txtOferta.Enabled = false;
            }

            if (ofertaAnterior == "")
            {
                txtOfertaAnterior.Text = "0";
            }
            else
            {
                txtOfertaAnterior.Text = ofertaAnterior;
            }
            txtOferta.Text = "0";
        }

        private void OfertarPubli_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!validaciones())
            {
                return;
            }

            //Validar que no esta vencida

            if ((MessageBox.Show("¿Desea realizar la oferta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                ofertar();
                MessageBox.Show("Oferta realizada", this.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
                form.Close();
                this.Close();
            }
            
        }

        private void ofertar()
        {
            string query5 = "SELECT N_ID_USUARIO FROM GDD_15.USUARIOS WHERE C_USUARIO_NOMBRE = '" + nombreUsuario + "'";
            DataTable dt5 = (new ConexionSQL()).cargarTablaSQL(query5);
            string usuarioID = dt5.Rows[0][0].ToString();

            String envio;

            if (chkEnvio.Checked == true)
            {
                envio = "SI";
            }
            else
            {
                envio = "NO";
            }

            string agregarOferta = "INSERT INTO GDD_15.OFERTAS (N_ID_PUBLICACION, N_ID_CLIENTE, F_ALTA, N_MONTO, C_ENVIO) VALUES ('" + idPubli + "', '" + usuarioID + "', '" + DateTime.Parse(Program.nuevaFechaSistema()).ToString() + "', '" + txtOferta.Text + "', '" + envio + "') ";
            (new ConexionSQL()).ejecutarComandoSQL(agregarOferta);
        }

        private bool validaciones()
        {
            int oferta;

            try
            {
                oferta = Convert.ToInt32(txtOferta.Text);
            }
            catch
            {
                MessageBox.Show("La oferta debe ser un entero menor a 2147483647", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            float ofertaAnterior = float.Parse(txtOfertaAnterior.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);

            if (oferta <= ofertaAnterior)
            {
                MessageBox.Show("La nueva oferta debe ser mayor a la anterior", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
