using ADMReestructuracion.Common.Interfaces;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Operations
{
    public class OperationRequest<T> : IOperationRequest<T>
    {
        public string Ip { get; set; }

        public DateTime Fecha { get; set; }

        public T Data { get; set; }

        public int IdEmpresa => Empresa.IdEmpresa;

        public string InicioSesion => Usuario.Nombres;

        public ICompanyEntity Empresa { get; set; }

        public IUserEntity Usuario { get; set; }

        public OperationRequest(T entidad, ICompanyEntity empresa, IUserEntity usuario = default, object ipAddress = default, DateTime? fecha = default)
        {
            Empresa = empresa;
            Usuario = usuario;

            Ip = $"{ipAddress}";
            Fecha = fecha ?? DateTime.Now;
            Data = entidad;
        }
    }

    public class OperationRequest : IOperationRequest
    {
        public string Ip { get; set; }

        public DateTime Fecha { get; set; }

        public int IdEmpresa => Empresa.IdEmpresa;

        public string InicioSesion => Usuario.Nombres;

        public ICompanyEntity Empresa { get; set; }

        public IUserEntity Usuario { get; set; }

        public OperationRequest(object ipAddress, DateTime fecha, ICompanyEntity empresa, IUserEntity usuario)
        {
            Ip = $"{ipAddress}";
            Fecha = fecha;
            Empresa = empresa;
            Usuario = usuario;
        }
    }
}
