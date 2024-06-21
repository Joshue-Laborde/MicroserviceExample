using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Interfaces
{
    public interface IOperationRequest
    {
        string Ip { get; }

        DateTime Fecha { get; }

        int IdEmpresa { get; }

        string InicioSesion { get; }

        IUserEntity Usuario { get; }

        ICompanyEntity Empresa { get; }
    }

    public interface IOperationRequest<T> : IOperationRequest
    {
        public T Data { get; set; }
    }
}
