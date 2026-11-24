using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaExpedienteschorrillos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConstruirMenuPrincipal();
        }

        private void ConstruirMenuPrincipal()
        {
            this.Text = "Sistema de Mesa de Partes - Municipalidad de Chorrillos";
            this.Size = new Size(850, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(242, 244, 248);

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(26, 115, 232)
            };

            Label lblHeader = new Label
            {
                Text = "MESA DE PARTES - MUNICIPALIDAD DE CHORRILLOS",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 22),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblHeader);

            Label lblSubtitulo = new Label
            {
                Text = "Seleccione un módulo para continuar:",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(40, 105),
                AutoSize = true
            };

            Button btnRegistrar = new Button
            {
                Text = "Registrar Expediente",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(26, 115, 232),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(40, 160),
                Size = new Size(240, 110),
                Cursor = Cursors.Hand
            };
            btnRegistrar.FlatAppearance.BorderColor = Color.FromArgb(26, 115, 232);
            btnRegistrar.FlatAppearance.BorderSize = 2;
            btnRegistrar.Click += btnRegistrar_Click;

            Button btnGestionar = new Button
            {
                Text = "Gestionar Expedientes\n(Buscar, Ordenar y Ver)",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 30, 30),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(305, 160),
                Size = new Size(240, 110),
                Cursor = Cursors.Hand
            };
            btnGestionar.FlatAppearance.BorderColor = Color.FromArgb(180, 185, 195);
            btnGestionar.FlatAppearance.BorderSize = 2;
            btnGestionar.Click += btnGestionar_Click;

            Button btnCerrarSesion = new Button
            {
                Text = "Cerrar sesión",
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(40, 320),
                Size = new Size(150, 38),
                Cursor = Cursors.Hand
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += btnCerrarSesion_Click;

            this.Controls.AddRange(new Control[] {
                pnlHeader, lblSubtitulo, btnRegistrar, btnGestionar, btnCerrarSesion
            });
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            FrmGestionExpedientes frm = new FrmGestionExpedientes();
            frm.ShowDialog();
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            FrmGestionExpedientes frm = new FrmGestionExpedientes();
            frm.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Cierra el menú; FrmLogin detecta el cierre y se cierra con él.
            // Para volver al login, se abre uno nuevo antes de cerrar.
            FrmLogin login = new FrmLogin();
            login.Show();
            this.Hide();
        }
    }
}