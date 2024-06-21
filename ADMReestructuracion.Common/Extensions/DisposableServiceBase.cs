using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Extensions
{
    public abstract class DisposableServiceBase<T> : IDisposable where T : IDisposable
    {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing) 
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Liberar recursos administrados
                    DisposeManagedResources();
                }

                //Liberar recursos no administrados
                disposed = true;
            }
        }

        protected virtual void DisposeManagedResources() 
        {
            //Implementa la logica para liberar los recursos administrados
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableServiceBase()
        {
            Dispose(false);
        }
    }
}
