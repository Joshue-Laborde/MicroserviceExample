using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Interfaces
{
    public interface IOperationResult
    {
        public string Error { get; set; }

        public string Message { get; set; }

        HttpStatusCode StatusCode { get; set; }

        public bool Success { get; }
    }

    public interface IOperationResult<T> : IOperationResult
    {
        public T? Result { get; }
    }

    public interface IOperationResultList<T> : IOperationResult
    {
        public IEnumerable<T?>? Result { get; }
    }
}
