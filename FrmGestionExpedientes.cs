using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SistemaExpedienteschorrillos
{
    public partial class FrmGestionExpedientes : Form
    {
        private TextBox txtBuscar;
        private ComboBox cmbCriterioBuscar, cmbCriterioOrden;
        private Button btnBuscar, btnMostrarTodos, btnOrdenar, btnVolver;
        private DataGridView dgvExpedientes;

        public FrmGestionExpedientes()
        {
            InitializeComponent();
            ConstruirFormulario();
            DatosExpedientes.Inicializar();
            CargarTabla();
        }

        private void ConstruirFormulario()
        {
            this.Text = "Gestión de Expedientes - Mesa de Partes";
            this.Size = new Size(980, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(242, 244, 248);

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(26, 115, 232)
            };

            Label lblHeader = new Label
            {
                Text = "GESTIÓN DE EXPEDIENTES",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 18),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblHeader);

            Label lblBuscarPor = new Label
            {
                Text = "Buscar por:",
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                AutoSize = true
            };

            cmbCriterioBuscar = new ComboBox
            {
                Location = new Point(30, 104),
                Size = new Size(140, 28),
                Font = new Font("Segoe UI", 9.5f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCriterioBuscar.Items.AddRange(new object[] { "Código", "DNI", "Administrado" });
            cmbCriterioBuscar.SelectedIndex = 0;

            txtBuscar = new TextBox
            {
                Location = new Point(180, 104),
                Size = new Size(210, 28),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(400, 102),
                Size = new Size(95, 32),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(26, 115, 232),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += BtnBuscar_Click;

            btnMostrarTodos = new Button
            {
                Text = "Ver todos",
                Location = new Point(503, 102),
                Size = new Size(95, 32),
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMostrarTodos.FlatAppearance.BorderColor = Color.Gray;
            btnMostrarTodos.Click += (s, e) =>
            {
                txtBuscar.Clear();
                CargarTabla();
            };

            Label lblOrdenar = new Label
            {
                Text = "Ordenar (Burbuja):",
                Location = new Point(625, 80),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                AutoSize = true
            };

            cmbCriterioOrden = new ComboBox
            {
                Location = new Point(625, 104),
                Size = new Size(145, 28),
                Font = new Font("Segoe UI", 9.5f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCriterioOrden.Items.AddRange(new object[] { "Código", "Administrado", "Fecha" });
            cmbCriterioOrden.SelectedIndex = 0;

            btnOrdenar = new Button
            {
                Text = "Ordenar",
                Location = new Point(778, 102),
                Size = new Size(95, 32),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOrdenar.FlatAppearance.BorderSize = 0;
            btnOrdenar.Click += BtnOrdenar_Click;

            dgvExpedientes = new DataGridView
            {
                Location = new Point(30, 152),
                Size = new Size(905, 375),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvExpedientes.Columns.Add("Codigo", "Código");
            dgvExpedientes.Columns.Add("Dni", "DNI");
            dgvExpedientes.Columns.Add("Nombres", "Nombres y Apellidos");
            dgvExpedientes.Columns.Add("TipoDoc", "Tipo Documento");
            dgvExpedientes.Columns.Add("Asunto", "Asunto");
            dgvExpedientes.Columns.Add("Fecha", "Fecha");

            btnVolver = new Button
            {
                Text = "Volver al Menú",
                Location = new Point(775, 540),
                Size = new Size(160, 36),
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                pnlHeader, lblBuscarPor, cmbCriterioBuscar, txtBuscar, btnBuscar,
                btnMostrarTodos, lblOrdenar, cmbCriterioOrden, btnOrdenar,
                dgvExpedientes, btnVolver
            });
        }

        private void CargarTabla()
        {
            DatosExpedientes.Inicializar();
            dgvExpedientes.Rows.Clear();
            for (int i = 0; i < DatosExpedientes.Cantidad; i++)
            {
                AgregarFilaDesdeIndice(i);
            }
        }

        private void AgregarFilaDesdeIndice(int i)
        {
            dgvExpedientes.Rows.Add(
                DatosExpedientes.Codigos[i],
                DatosExpedientes.Dnis[i],
                DatosExpedientes.NombresCompletos[i],
                DatosExpedientes.TiposDocumento[i],
                DatosExpedientes.Asuntos[i],
                DatosExpedientes.Fechas[i]
            );
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            DatosExpedientes.Inicializar();
            string termino = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(termino))
            {
                MessageBox.Show("Ingrese un valor para buscar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string criterio = cmbCriterioBuscar.SelectedItem?.ToString() ?? "Código";
            dgvExpedientes.Rows.Clear();
            int encontrados = 0;

            for (int i = 0; i < DatosExpedientes.Cantidad; i++)
            {
                bool coincide = false;
                if (criterio == "Código")
                {
                    coincide = string.Equals(DatosExpedientes.Codigos[i], termino, StringComparison.OrdinalIgnoreCase);
                }
                else if (criterio == "DNI")
                {
                    coincide = DatosExpedientes.Dnis[i] == termino;
                }
                else if (criterio == "Administrado")
                {
                    coincide = (DatosExpedientes.NombresCompletos[i] ?? string.Empty)
                        .IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (coincide)
                {
                    AgregarFilaDesdeIndice(i);
                    encontrados++;
                }
            }

            if (encontrados == 0)
            {
                MessageBox.Show("No se encontraron expedientes con ese criterio.", "Búsqueda",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarTabla();
            }
        }

        private void BtnOrdenar_Click(object sender, EventArgs e)
        {
            DatosExpedientes.Inicializar();
            if (DatosExpedientes.Cantidad <= 1) return;

            string criterio = cmbCriterioOrden.SelectedItem?.ToString() ?? "Código";

            for (int i = 0; i < DatosExpedientes.Cantidad - 1; i++)
            {
                for (int j = 0; j < DatosExpedientes.Cantidad - 1 - i; j++)
                {
                    bool debeIntercambiar = false;

                    if (criterio == "Código")
                    {
                        debeIntercambiar = string.Compare(
                            DatosExpedientes.Codigos[j], DatosExpedientes.Codigos[j + 1],
                            StringComparison.OrdinalIgnoreCase) > 0;
                    }
                    else if (criterio == "Administrado")
                    {
                        debeIntercambiar = string.Compare(
                            DatosExpedientes.NombresCompletos[j], DatosExpedientes.NombresCompletos[j + 1],
                            StringComparison.OrdinalIgnoreCase) > 0;
                    }
                    else if (criterio == "Fecha")
                    {
                        debeIntercambiar = CompararFechaStr(
                            DatosExpedientes.Fechas[j], DatosExpedientes.Fechas[j + 1]) > 0;
                    }

                    if (debeIntercambiar)
                    {
                        Intercambiar(j, j + 1);
                    }
                }
            }

            DatosExpedientes.GuardarEnArchivo();
            CargarTabla();
            MessageBox.Show($"Expedientes ordenados por {criterio} (Método Burbuja).", "Ordenamiento",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int CompararFechaStr(string f1, string f2)
        {
            if (DateTime.TryParseExact(f1, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d1) &&
                DateTime.TryParseExact(f2, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d2))
            {
                return d1.CompareTo(d2);
            }
            return string.Compare(f1, f2, StringComparison.Ordinal);
        }

        private void Intercambiar(int a, int b)
        {
            Swap(ref DatosExpedientes.Codigos[a], ref DatosExpedientes.Codigos[b]);
            Swap(ref DatosExpedientes.Dnis[a], ref DatosExpedientes.Dnis[b]);
            Swap(ref DatosExpedientes.NombresCompletos[a], ref DatosExpedientes.NombresCompletos[b]);
            Swap(ref DatosExpedientes.TiposDocumento[a], ref DatosExpedientes.TiposDocumento[b]);
            Swap(ref DatosExpedientes.Asuntos[a], ref DatosExpedientes.Asuntos[b]);
            Swap(ref DatosExpedientes.Fechas[a], ref DatosExpedientes.Fechas[b]);
        }

        private void Swap(ref string x, ref string y)
        {
            string temp = x;
            x = y;
            y = temp;
        }
    }
}