using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models.Mapeos
{
    public class MesMapeo
    {
        public string BuscaNombreMes(int mesId)
        {
            switch (mesId)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return "";
            }
        }

        public string BuscaNombreMes3Letras(int mesId)
        {
            switch (mesId)
            {
                case 1:
                    return "ENE";
                case 2:
                    return "FEB";
                case 3:
                    return "MAR";
                case 4:
                    return "ABR";
                case 5:
                    return "MAY";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AGO";
                case 9:
                    return "SEP";
                case 10:
                    return "OCT";
                case 11:
                    return "NOV";
                case 12:
                    return "DIC";
                default:
                    return "";
            }
        }

        public string BuscaNombrePeriodo(int periodoId)
        {
            switch (periodoId)
            {
                case 1:
                    return "I Timestre";
                case 2:
                    return "II Trimestre";
                case 3:
                    return "I Semestre";
                case 4:
                    return "III Trimestre";
                case 5:
                    return "IV Trimestre";
                case 6:
                    return "II Semestre";
                case 7:
                    return "Anual";
                default:
                    return "";
            }
        }

        public string BuscaNombrePeriodo3Letras(int periodoId)
        {
            switch (periodoId)
            {
                case 1:
                    return "I TRIM";
                case 2:
                    return "II TRIM";
                case 3:
                    return "I SEM";
                case 4:
                    return "III TRIM";
                case 5:
                    return "IV TRIM";
                case 6:
                    return "II SEM";
                case 7:
                    return "ANUAL";
                default:
                    return "";
            }
        }


        public string BuscaNombrePeriodoTitulo(int periodoId)
        {
            switch (periodoId)
            {
                case 1:
                    return "I TRIMESTRE";
                case 2:
                    return "I TRIMESTRE, II TIMESTRE";
                case 3:
                    return "I TRIMESTRE, II TIMESTRE, I SEMESTRE";
                case 4:
                    return "I TRIMESTRE, II TIMESTRE, I SEMESTRE, III TRIMESTRE";
                case 5:
                    return "I TRIMESTRE, II TIMESTRE, I SEMESTRE, III TRIMESTRE, IV TRIMESTRE";
                case 6:
                    return "I TRIMESTRE, II TIMESTRE, I SEMESTRE, III TRIMESTRE, IV TRIMESTRE, II SEMESTRE";
                case 7:
                    return "I TRIMESTRE, II TIMESTRE, I SEMESTRE, III TRIMESTRE, IV TRIMESTRE, II SEMESTRE, ANUAL";
                default:
                    return "";
            }
        }
    }

    public class MesModel
    {
        public int MesId { get; set; }
        public string NombreMes { get; set; }
    }

    public class PeriodoModel
    {
        public int PeriodoId { get; set; }
        public string NombrePeriodo { get; set; }
    }
    
}