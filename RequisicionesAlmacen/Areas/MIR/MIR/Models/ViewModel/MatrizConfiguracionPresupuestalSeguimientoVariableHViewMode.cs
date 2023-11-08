﻿using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel
{
    public class MatrizConfiguracionPresupuestalSeguimientoVariableHViewModel
    {
        public IEnumerable<MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariable> ListadoMatrizConfiguracionPresupuestalSeguimientoVariable { get; set; }
        public MItblMatrizConfiguracionPresupuestalSeguimientoVariable MatrizConfiguracionPresupuestalSeguimientoVariableModel { get; set; }
        public MIspConsultaMatrizIndicadorResultado_Result ConsultaMatrizIndicadorResultado { get; set; }
        public IEnumerable<MItblMatrizIndicadorResultadoIndicador> ListMatrizIndicadorResultadoIndicador { get; set; }
        public IEnumerable<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> ListMatrizIndicadorResultadoIndicadorFormulaVariable { get; set; }
        public IEnumerable<MItblControlMaestroUnidadMedida> ListUnidadMedida { get; set; }
        public IEnumerable<MItblControlMaestroFrecuenciaMedicion> ListFrecuenciaMedicion { get; set; }
        public IEnumerable<GRtblControlMaestro> ListSentido { get; set; }
        public IEnumerable<MItblMatrizConfiguracionPresupuestalSeguimientoVariable> ListMatrizConfiguracionPresupuestalSeguimientoVariable { get; set; }
        public IEnumerable<MenuSeguimientoVariableHModel> ListMenuSeguimientoVariable { get; set; }
        public IEnumerable<MItblControlMaestroControlPeriodo> ListaControlMaestroControlPeriodo { get; set; }
    }

    public class MenuSeguimientoVariableHModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public IEnumerable<MenuSeguimientoVariableModel> Items { get; set; }
    }

    public class MenuItemsSeguimientoVariableHModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? Id_Componente { get; set; }
    }
}