﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Calificar
{
    public partial class VerCalificaciones : Form
    {
        Int64 idCli;
        public VerCalificaciones(Int64 idCliPasado)
        {
            InitializeComponent();
            idCli = idCliPasado;
        }
    }
}
