using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;
using System.Data.Entity.Core.Objects;

namespace RequisicionesAlmacenBL.Services.Sistema
{
    public class AlertaService : BaseService<GRtblAlerta>
    {
        public override GRtblAlerta Inserta(GRtblAlerta entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override bool Actualiza(GRtblAlerta entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblAlerta BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.GRtblAlerta.Where(m => m.AlertaId == id).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<GRspGetListadoAlertasPorUsuario_Result> GetListadoAlertas(int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.GRspGetListadoAlertasPorUsuario(usuarioId).ToList();
            }
        }

        public int IniciarAlerta(int alertaDefinicionId, int referenciaProcesoId, string codigoTramite, string textoRepresentativo, int alertaCreadaPorId, SAACGContext context)
        {
            return context.GRspIniciarAlerta(AlertaAccion.INICIAR, 
                                             alertaDefinicionId,
                                             referenciaProcesoId,
                                             codigoTramite,
                                             textoRepresentativo,
                                             alertaCreadaPorId,
                                             new ObjectParameter("valorSalida", ""));
        }

        public int AutorizarAlerta(int alertaId, int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Ejecutamos la consulta
                    alertaId = Context.GRspAutorizarAlerta(AlertaAccion.AUTORIZAR, usuarioId, alertaId, new ObjectParameter("valorSalida", ""));

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    return alertaId;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public int RevisionAlerta(int alertaId, string motivo, int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Ejecutamos la consulta
                    alertaId = Context.GRspRevisionRechazarAlerta(AlertaAccion.REVISION, usuarioId, alertaId, motivo, new ObjectParameter("valorSalida", ""));

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    return alertaId;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public int RechazarAlerta(int alertaId, string motivo, int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Ejecutamos la consulta
                    alertaId = Context.GRspRevisionRechazarAlerta(AlertaAccion.RECHAZAR, usuarioId, alertaId, motivo, new ObjectParameter("valorSalida", ""));

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    return alertaId;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public int OcultarAlertas(string alertasId, int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Ejecutamos la consulta
                    int alertaId = Context.GRspOcultarAlertas(AlertaAccion.OCULTAR, usuarioId, alertasId, new ObjectParameter("valorSalida", ""));

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    return alertaId;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }
    }
}