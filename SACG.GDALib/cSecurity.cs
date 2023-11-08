using System;
using System.Collections.Generic;
using System.Text;

namespace SACG.GDALib
{
    public class cSecurity
    {
        #region constantes

        const string pass = "SACG";

        #endregion

        # region metodos

        /// <summary>
        /// devuelve la clave encriptada
        /// </summary>
        /// <param name="Strg"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        /// <remarks>
        /// Fecha: 10.10.2010,  Autor: Israel Medrano
        /// </remarks>
        public static string EncryptINI(string Strg)
        {
            // Encripta una cadena de texto

            string b;
            string s = "";
            string p1 = "";
            long A1, A2, A3;
            int j = 0;
            for (int I = 0; I < pass.Length; I++)
            {
                p1 = p1 + Microsoft.VisualBasic.Strings.Asc(pass.Substring(I, 1)).ToString();
            }

            for (int I = 0; I < Strg.Length; I++)
            {
                A1 = Microsoft.VisualBasic.Strings.Asc(p1.Substring(j, 1));
                j++;
                if (j >= p1.Length) j = 0;
                A2 = Microsoft.VisualBasic.Strings.Asc(Strg.Substring(I, 1));
                A3 = A1 ^ A2;
                b = Microsoft.VisualBasic.Conversion.Hex(A3);
                if (b.Length < 2) b = "0" + b;
                s += b;
            }

            return s;
        }


        /// <summary>
        /// devuelve la clave encriptada
        /// </summary>
        /// <param name="Strg"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        /// <remarks>
        /// Fecha: 10.10.2010,  Autor: Israel Medrano
        /// </remarks>
        public static string EncryptINI(string Strg, string palabraClave)
        {
            // Encripta una cadena de texto

            string b;
            string s = "";
            string p1 = "";
            long A1, A2, A3;
            int j = 0;
            for (int I = 0; I < palabraClave.Length; I++)
            {
                p1 = p1 + Microsoft.VisualBasic.Strings.Asc(palabraClave.Substring(I, 1)).ToString();
            }

            for (int I = 0; I < Strg.Length; I++)
            {
                A1 = Microsoft.VisualBasic.Strings.Asc(p1.Substring(j, 1));
                j++;
                if (j >= p1.Length) j = 0;
                A2 = Microsoft.VisualBasic.Strings.Asc(Strg.Substring(I, 1));
                A3 = A1 ^ A2;
                b = Microsoft.VisualBasic.Conversion.Hex(A3);
                if (b.Length < 2) b = "0" + b;
                s += b;
            }

            return s;
        }


        /// <summary>
        /// devuelve la cadena desencriptada
        /// </summary>
        /// <param name="Strg"></param>
        /// <returns></returns>
        /// <remarks>
        /// Fecha: 10.10.2010,  Autor: Israel Medrano
        /// </remarks>
        public static string DecryptINI(string Strg)
        {
            // Desencripta una cadena de texto
            string b;
            string s = "";
            string p1 = "";
            long A1, A2, A3;
            int j = 0;
            for (int I = 0; I < pass.Length; I++)
            {
                p1 = p1 + Microsoft.VisualBasic.Strings.Asc(pass.Substring(I, 1)).ToString();
            }

            for (int I = 0; I < Strg.Length; I += 2)
            {
                A1 = Microsoft.VisualBasic.Strings.Asc(p1.Substring(j, 1));
                j++;
                if (j >= p1.Length) j = 0;
                b = Strg.Substring(I, 2);
                A3 = long.Parse(b, System.Globalization.NumberStyles.AllowHexSpecifier);
                A2 = A1 ^ A3;
                s = s + (char)A2;
            }
            return s;
        }

        /// <summary>
        /// devuelve la cadena desencriptada
        /// </summary>
        /// <param name="Strg"></param>
        /// <returns></returns>
        /// <remarks>
        /// Fecha: 10.10.2010,  Autor: Israel Medrano
        /// </remarks>
        public static string DecryptINI(string Strg, string palabraClave)
        {
            // Desencripta una cadena de texto
            string b;
            string s = "";
            string p1 = "";
            long A1, A2, A3;
            int j = 0;
            for (int I = 0; I < palabraClave.Length; I++)
            {
                p1 = p1 + Microsoft.VisualBasic.Strings.Asc(palabraClave.Substring(I, 1)).ToString();
            }

            for (int I = 0; I < Strg.Length; I += 2)
            {
                A1 = Microsoft.VisualBasic.Strings.Asc(p1.Substring(j, 1));
                j++;
                if (j >= p1.Length) j = 0;
                b = Strg.Substring(I, 2);
                A3 = long.Parse(b, System.Globalization.NumberStyles.AllowHexSpecifier);
                A2 = A1 ^ A3;
                s = s + (char)A2;
            }
            return s;
        }


        #endregion
    }
}
