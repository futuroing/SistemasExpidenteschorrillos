using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SistemaExpedienteschorrillos
{
    public partial class FrmLogin : Form
    {
        private Panel pnlCard;
        private PictureBox picLogo;
        private Label lblSubtitulo, lblUsuario, lblPassword;
        private TextBox txtUsuario, txtPassword;
        private Button btnIngresar, btnSalir;

        // Arreglos paralelos
        private string[] usuarios = new string[50];
        private string[] claves = new string[50];
        private string[] roles = new string[50];
        private int cantidadUsuarios = 0;

        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;
        private const string ARCHIVO_USUARIOS = "usuarios.txt";

        public FrmLogin()
        {
            InitializeComponent();
            ConstruirInterfaz();
            CargarUsuariosDesdeArchivo();
        }

        private void ConstruirInterfaz()
        {
            this.Text = "Acceso al Sistema - Mesa de Partes Chorrillos";
            this.Size = new Size(860, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(242, 244, 248);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            pnlCard = new Panel
            {
                Size = new Size(390, 490),
                BackColor = Color.White
            };
            pnlCard.Left = (this.ClientSize.Width - pnlCard.Width) / 2;
            pnlCard.Top = (this.ClientSize.Height - pnlCard.Height) / 2;

            picLogo = new PictureBox
            {
                Size = new Size(210, 120),
                Location = new Point((pnlCard.Width - 210) / 2, 22),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            CargarLogo();

            lblSubtitulo = new Label
            {
                Text = "Mesa de Partes • Acceder a tu cuenta",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 45, 75),
                AutoSize = false,
                Size = new Size(330, 30),
                Location = new Point(30, 152),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblUsuario = new Label
            {
                Text = "Usuario",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(40, 198),
                AutoSize = true
            };
            txtUsuario = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(40, 222),
                Size = new Size(310, 32),
                BorderStyle = BorderStyle.FixedSingle,
                MaxLength = 30
            };

            lblPassword = new Label
            {
                Text = "Contraseña",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(40, 268),
                AutoSize = true
            };
            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(40, 292),
                Size = new Size(310, 32),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true,
                MaxLength = 30
            };

            btnIngresar = new Button
            {
                Text = "Iniciar sesión",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 102, 168),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(40, 350),
                Size = new Size(310, 42),
                Cursor = Cursors.Hand
            };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Click += btnIngresar_Click;

            btnSalir = new Button
            {
                Text = "Salir",
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.FromArgb(245, 247, 250),
                ForeColor = Color.FromArgb(80, 90, 100),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(40, 405),
                Size = new Size(310, 36),
                Cursor = Cursors.Hand
            };
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => Application.Exit();

            this.AcceptButton = btnIngresar;

            pnlCard.Controls.AddRange(new Control[] {
                picLogo, lblSubtitulo, lblUsuario, txtUsuario,
                lblPassword, txtPassword, btnIngresar, btnSalir
            });
            this.Controls.Add(pnlCard);
        }

        private void CargarLogo()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] posiblesRutas =
            {
                Path.Combine(baseDir, "logo_chorrillos.png"),
                Path.Combine(baseDir, "Assets", "logo_chorrillos.png"),
                Path.Combine(baseDir, "Resources", "logo_chorrillos.png")
            };

            foreach (string ruta in posiblesRutas)
            {
                if (File.Exists(ruta))
                {
                    using (var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
                    {
                        picLogo.Image = Image.FromStream(fs);
                    }
                    return;
                }
            }
        }

        // Lee usuarios.txt (formato: usuario;clave;rol)
        private void CargarUsuariosDesdeArchivo()
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ARCHIVO_USUARIOS);

            try
            {
                if (!File.Exists(ruta))
                {
                    File.WriteAllLines(ruta, new string[]
                    {
                        "admin;1234;Administrador",
                        "mesa;5678;Mesa de Partes",
                        "jefe;abcd;Supervisor"
                    });
                }

                string[] lineas = File.ReadAllLines(ruta);
                cantidadUsuarios = 0;

                for (int i = 0; i < lineas.Length && cantidadUsuarios < usuarios.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lineas[i])) continue;

                    string[] partes = lineas[i].Split(';');
                    if (partes.Length < 3) continue;

                    usuarios[cantidadUsuarios] = partes[0].Trim();
                    claves[cantidadUsuarios] = partes[1].Trim();
                    roles[cantidadUsuarios] = partes[2].Trim();
                    cantidadUsuarios++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo leer el archivo de usuarios: " + ex.Message,
                    "Error de archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Devuelve la posición del usuario o -1
        private int ValidarCredenciales(string usuario, string clave)
        {
            for (int i = 0; i < cantidadUsuarios; i++)
            {
                if (string.Equals(usuarios[i], usuario, StringComparison.OrdinalIgnoreCase)
                    && claves[i] == clave)
                {
                    return i;
                }
            }
            return -1;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Complete usuario y contraseña.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pos = ValidarCredenciales(usuario, clave);

            if (pos >= 0)
            {
                intentosFallidos = 0;
                MessageBox.Show("Bienvenido, " + usuarios[pos] + " (" + roles[pos] + ")",
                    "Acceso correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form1 principal = new Form1();
                principal.FormClosed += (s, ev) => this.Close();
                principal.Show();
                this.Hide();
            }
            else
            {
                intentosFallidos++;
                int restantes = MAX_INTENTOS - intentosFallidos;

                if (restantes <= 0)
                {
                    MessageBox.Show("Superó el máximo de 3 intentos. El sistema se cerrará.",
                        "Acceso bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                    return;
                }

                MessageBox.Show("Usuario o contraseña incorrectos. Intentos restantes: " + restantes,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}