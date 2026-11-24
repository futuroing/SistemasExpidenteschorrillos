using System;
using System.Globalization;
using System.IO;

namespace SistemaExpedienteschorrillos
{
    public static class DatosExpedientes
    {
        public const int MAX = 500;

        // Arreglos paralelos
        public static string[] Codigos = new string[MAX];
        public static string[] Dnis = new string[MAX];
        public static string[] NombresCompletos = new string[MAX];
        public static string[] TiposDocumento = new string[MAX];
        public static string[] Asuntos = new string[MAX];
        public static string[] Fechas = new string[MAX];
        public static int Cantidad = 0;

        private static bool cargado = false;
        private static string Ruta
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "expedientes.txt"); }
        }

        // Carga el archivo una sola vez
        public static void Inicializar()
        {
            if (cargado) return;
            cargado = true;
            Cantidad = 0;

            try
            {
                if (!File.Exists(Ruta)) return;

                string[] lineas = File.ReadAllLines(Ruta);
                for (int i = 0; i < lineas.Length && Cantidad < MAX; i++)
                {
                    if (string.IsNullOrWhiteSpace(lineas[i])) continue;
                    string[] p = lineas[i].Split('|');
                    if (p.Length < 6) continue;

                    Codigos[Cantidad] = p[0];
                    Dnis[Cantidad] = p[1];
                    NombresCompletos[Cantidad] = p[2];
                    TiposDocumento[Cantidad] = p[3];
                    Asuntos[Cantidad] = p[4];
                    Fechas[Cantidad] = p[5];
                    Cantidad++;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al leer expedientes.txt: " + ex.Message);
            }
        }

        public static void GuardarEnArchivo()
        {
            try
            {
                string[] lineas = new string[Cantidad];
                for (int i = 0; i < Cantidad; i++)
                {
                    lineas[i] = Codigos[i] + "|" + Dnis[i] + "|" + NombresCompletos[i] + "|" +
                                TiposDocumento[i] + "|" + Asuntos[i] + "|" + Fechas[i];
                }
                File.WriteAllLines(Ruta, lineas);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar expedientes.txt: " + ex.Message);
            }
        }

        // Código autogenerado: EXP-2026-001
        public static string GenerarCodigo()
        {
            int anio = DateTime.Now.Year;
            int max = 0;
            for (int i = 0; i < Cantidad; i++)
            {
                string[] partes = Codigos[i].Split('-');
                int n;
                if (partes.Length == 3 && partes[1] == anio.ToString() && int.TryParse(partes[2], out n))
                {
                    if (n > max) max = n;
                }
            }
            return "EXP-" + anio + "-" + (max + 1).ToString("D3");
        }

        // INSERCIÓN
        public static bool Agregar(string dni, string nombre, string tipo, string asunto)
        {
            if (Cantidad >= MAX) return false;

            Codigos[Cantidad] = GenerarCodigo();
            Dnis[Cantidad] = dni;
            NombresCompletos[Cantidad] = nombre;
            TiposDocumento[Cantidad] = tipo;
            Asuntos[Cantidad] = asunto;
            Fechas[Cantidad] = DateTime.Now.ToString("dd/MM/yyyy");
            Cantidad++;
            GuardarEnArchivo();
            return true;
        }

        // MODIFICACIÓN
        public static void Modificar(int pos, string dni, string nombre, string tipo, string asunto)
        {
            Dnis[pos] = dni;
            NombresCompletos[pos] = nombre;
            TiposDocumento[pos] = tipo;
            Asuntos[pos] = asunto;
            GuardarEnArchivo();
        }

        // ELIMINACIÓN (desplaza los elementos)
        public static void Eliminar(int pos)
        {
            for (int i = pos; i < Cantidad - 1; i++)
            {
                Codigos[i] = Codigos[i + 1];
                Dnis[i] = Dnis[i + 1];
                NombresCompletos[i] = NombresCompletos[i + 1];
                TiposDocumento[i] = TiposDocumento[i + 1];
                Asuntos[i] = Asuntos[i + 1];
                Fechas[i] = Fechas[i + 1];
            }
            Cantidad--;
            Codigos[Cantidad] = Dnis[Cantidad] = NombresCompletos[Cantidad] = null;
            TiposDocumento[Cantidad] = Asuntos[Cantidad] = Fechas[Cantidad] = null;
            GuardarEnArchivo();
        }

        // RECURSIVIDAD: búsqueda por código
        public static int BuscarCodigoRecursivo(string codigo, int indice)
        {
            if (indice >= Cantidad) return -1;                       // caso base: no existe
            if (string.Equals(Codigos[indice], codigo, StringComparison.OrdinalIgnoreCase))
                return indice;                                       // caso base: encontrado
            return BuscarCodigoRecursivo(codigo, indice + 1);        // caso recursivo
        }

        // RECURSIVIDAD: contar expedientes
        public static int ContarRecursivo(int indice)
        {
            if (indice >= Cantidad) return 0;
            return 1 + ContarRecursivo(indice + 1);
        }

        // Comparación según criterio
        public static int Comparar(string criterio, int a, int b)
        {
            if (criterio == "Administrado")
                return string.Compare(NombresCompletos[a], NombresCompletos[b], StringComparison.OrdinalIgnoreCase);

            if (criterio == "Fecha")
            {
                DateTime d1, d2;
                if (DateTime.TryParseExact(Fechas[a], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d1) &&
                    DateTime.TryParseExact(Fechas[b], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d2))
                    return d1.CompareTo(d2);
                return string.Compare(Fechas[a], Fechas[b], StringComparison.Ordinal);
            }

            return string.Compare(Codigos[a], Codigos[b], StringComparison.OrdinalIgnoreCase);
        }

        public static void Intercambiar(int a, int b)
        {
            string t;
            t = Codigos[a]; Codigos[a] = Codigos[b]; Codigos[b] = t;
            t = Dnis[a]; Dnis[a] = Dnis[b]; Dnis[b] = t;
            t = NombresCompletos[a]; NombresCompletos[a] = NombresCompletos[b]; NombresCompletos[b] = t;
            t = TiposDocumento[a]; TiposDocumento[a] = TiposDocumento[b]; TiposDocumento[b] = t;
            t = Asuntos[a]; Asuntos[a] = Asuntos[b]; Asuntos[b] = t;
            t = Fechas[a]; Fechas[a] = Fechas[b]; Fechas[b] = t;
        }

        // ORDENAMIENTO BURBUJA
        public static void OrdenarBurbuja(string criterio)
        {
            for (int i = 0; i < Cantidad - 1; i++)
            {
                for (int j = 0; j < Cantidad - 1 - i; j++)
                {
                    if (Comparar(criterio, j, j + 1) > 0)
                        Intercambiar(j, j + 1);
                }
            }
        }

        // ORDENAMIENTO SELECCIÓN
        public static void OrdenarSeleccion(string criterio)
        {
            for (int i = 0; i < Cantidad - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < Cantidad; j++)
                {
                    if (Comparar(criterio, j, min) < 0)
                        min = j;
                }
                if (min != i) Intercambiar(i, min);
            }
        }
    }
}