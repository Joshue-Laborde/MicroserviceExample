using ADMReestructuracion.Common.Interfaces;
using ADMReestructuracion.Common.Operations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Extensions
{
    public static class BusinessExtensions
    {
        static IMapper _mapper;

        public static string GenerateJwtToken(this IUserEntity user, string secretKey, bool changePassword = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "",
                Subject = user.ToIdentity(changePassword),
                Expires = DateTime.UtcNow.AddHours(2), // Tiempo de expiracion
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        //*************** Mapping Extensions ***************** //
        public static void Initialize(IServiceProvider services)
        {
            _mapper = (IMapper)services.GetService(typeof(IMapper));
        }

        /// <summary>
        /// Metodo de Mapeo Extension
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static async Task<TDestino> MappingResult<TDestino>(this object formMapp)
        {

            return await formMapp.MappingResult<TDestino>(default);
        }

        public static async Task<TDestino> MappingResult<TDestino>(this object formMapp, TDestino opts)
        {
            TDestino mapeo;
            if (opts == null)
            {
                mapeo = _mapper.Map<TDestino>(formMapp);
            }
            else
            {
                mapeo = _mapper.Map(formMapp, opts);
            }
            return mapeo;
        }

        // ********** Request Data *************** //

        public static IOperationRequest<T> ToRequest<T>(this T entity, IOperationRequest request)
        {
            return new OperationRequest<T>(entity, request.Empresa, request.Usuario, request.Ip, request.Fecha);
        }

        public static IOperationRequest<T> ToRequest<T>(this T entity, ICompanyEntity empresa, IUserEntity usuario, IPAddress ip, DateTime? fecha = default)
        {
            return new OperationRequest<T>(entity, empresa, usuario, ip, fecha ?? DateTime.Now);
        }
        public static IOperationRequest ToRequest(IOperationRequest request)
        {
            return new OperationRequest(request.Ip, request.Fecha, request.Empresa, request.Usuario);
        }

        public static IOperationRequest ToRequest(ICompanyEntity empresa, IUserEntity usuario, IPAddress ip, DateTime? fecha = default)
        {
            return new OperationRequest(ip, fecha ?? DateTime.Now, empresa, usuario);
        }

        public static async Task<IOperationRequest<T>> ToRequestAsync<T>(this T entity, ICompanyEntity empresa, IUserEntity usuario, IPAddress ip, DateTime? fecha = default)
        {
            return await Task.FromResult(entity.ToRequest(empresa, usuario, ip, fecha));
        }

        public static async Task<IOperationRequest<T>> ToRequestAsync<T>(this T entity, IOperationRequest request)
        {
            return await Task.FromResult(entity.ToRequest(request));
        }

        public static async Task<IOperationRequest> ToRequestAsync(this IOperationRequest request)
        {
            return await Task.FromResult(ToRequest(request));
        }


        // *************** Result Data ************** //
        public static IOperationResult<T> ToResult<T>(this T entity, HttpStatusCode status = HttpStatusCode.OK, string message = default)
        {
            return new OperationResult<T>(status, message, entity);
        }

        public static IOperationResult ToResult(this Exception ex)
        {
            return new OperationResult(ex);
        }

        public static IOperationResult<TResult> ToResult<TQuery, TResult>(this TQuery entity, HttpStatusCode status = HttpStatusCode.OK, string message = default, string error = default)
        {
            try
            {

                var result = _mapper.Map<TResult>(entity);
                return new OperationResult<TResult>(status, message, result, error);
            }
            catch (Exception ex)
            {
                return new OperationResult<TResult>(ex);
            }

        }

        public static IOperationResult<T> ToResult<T>(this Exception ex) where T : class
        {
            return new OperationResult<T>(ex);
        }

        public static async Task<IOperationResult> ToResultAsync(this Exception ex)
        {
            return await Task.FromResult(new OperationResult(ex));
        }

        public static async Task<IOperationResult<TResult>> ToResultAsync<TQuery, TResult>(this TQuery entity)
        {
            return await Task.FromResult(entity.ToResult<TQuery, TResult>());
        }

        public static async Task<IOperationResult<TResult>> ToResultAsync<TQuery, TResult>(this TQuery entity, HttpStatusCode status, string message = default, string error = default)
        {
            return await Task.FromResult(entity.ToResult<TQuery, TResult>(status, message, error));
        }

        public static async Task<IOperationResult<T>> ToResultAsync<T>(this Exception ex)
        {
            return await Task.FromResult(new OperationResult<T>(ex));
        }

        public static async Task<IOperationResult<T>> ToResultAsync<T>(this T entity, HttpStatusCode status = HttpStatusCode.OK, string message = default, string error = default)
        {
            return await Task.FromResult(new OperationResult<T>(status, message, entity, error));
        }

        public static IOperationResultList<T> ToResultList<T>(this IEnumerable<T> entity,
                        int pageNumber = 1,
                        int pageSize = 10,
                        int count = 0)
        {
            return new OperationResultList<T>(HttpStatusCode.OK, default, entity, pageNumber, pageSize, count);
        }

        public static IOperationResultList<T> ToResultList<T>(this Exception ex, IEnumerable<T> entity = null)
        {
            return new OperationResultList<T>(ex, entity);
        }

        public static IOperationResultList<T> ToResultList<T>(this IEnumerable<T> entity,
                        HttpStatusCode status,
                        string message = default,
                        int pageNumber = 1,
                        int? pageSize = default,
                        int? count = default
                        )
        {
            return new OperationResultList<T>(status, message, entity, pageNumber, pageSize, count);
        }

        public static async Task<IOperationResultList<TResult>> ToResultList<TQuery, TResult>(this IQueryable<TQuery> entity,
                            int pageNumber = 1,
                            int? pageSize = default,
                Expression<Func<TResult, object>> orderByDesc = null)
        {


            try
            {

                var query = entity;
                var count = query.Count();
                IEnumerable<TResult> result = null;

                if (pageSize > 0)
                {
                    if (pageNumber > 1)
                    {
                        query = query.Skip(pageSize.Value * (pageNumber - 1));
                    }

                    query = query.Take(pageSize.Value);
                }

                var baseresul = await query.ToListAsync();
                result = _mapper.Map<List<TResult>>(baseresul);


                if (orderByDesc != null)
                {
                    var asquerabel = result.AsQueryable();
                    result = asquerabel.OrderByDescending(orderByDesc).ToList();
                }

                return await Task.FromResult(new OperationResultList<TResult>(HttpStatusCode.OK, default, result, pageNumber, pageSize, count));

            }
            catch (Exception ex)
            {

                return await Task.FromResult(new OperationResultList<TResult>(ex));
            }


        }
        public static async Task<IOperationResultList<TResult>> ToResultList<TQuery, TResult>(
                this List<TQuery> entity,
                int pageNumber = 1,
                int? pageSize = default,
                Expression<Func<TResult, object>> orderByDesc = null)
        {
            try
            {
                var query = entity;
                var count = query.Count();
                IEnumerable<TResult> result = _mapper.Map<List<TResult>>(entity);

                // Aplicar paginación si es necesario
                //if (pageSize.HasValue && pageSize > 0)
                //{
                //    result = result.Skip((pageNumber - 1) * pageSize.Value).Take(pageSize.Value);
                //}

                if (pageSize > 0)
                {
                    if (pageNumber > 1)
                    {
                        result = result.Skip(pageSize.Value * (pageNumber - 1));
                    }
                    result = result.Take(pageSize.Value);
                }

                // Aplicar ordenamiento si se especificó
                if (orderByDesc != null)
                {
                    var asQueryable = result.AsQueryable();
                    result = asQueryable.OrderByDescending(orderByDesc).ToList();
                }


                var resultList = result.ToList();


                return await Task.FromResult(
                    new OperationResultList<TResult>(
                        HttpStatusCode.OK,
                        default,
                        result,
                        pageNumber,
                        pageSize,
                        count));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new OperationResultList<TResult>(ex));
            }
        }

        public static IOperationResultList<T> ToResultList<T>(this Exception ex) where T : class
        {
            return new OperationResultList<T>(ex);
        }

        public static async Task<IOperationResultList<TResult>> ToResultListAsync<TQuery, TResult>(this IOrderedQueryable<TQuery> entity,
                                int pageNumber = 1,
                        int? pageSize = default,
                Expression<Func<TResult, object>> orderByDesc = null)
        {
            return await entity.ToResultList<TQuery, TResult>(pageNumber, pageSize, orderByDesc);
        }

        public static async Task<IOperationResultList<TResult>> ToResultListAsync<TQuery, TResult>(this IQueryable<TQuery> entity,
                        int pageNumber = 1,
                        int? pageSize = default)
        {
            return await entity.ToResultList<TQuery, TResult>(pageNumber, pageSize);
        }


        public static async Task<IOperationResultList<TResult>> ToResultListAsync<TQuery, TResult>(this List<TQuery> entity,
                        int pageNumber = 1,
                        int? pageSize = default, Expression<Func<TResult, object>> orderByDesc = null)
        {
            return await entity.ToResultList<TQuery, TResult>(pageNumber, pageSize, orderByDesc);
        }

        public static async Task<IOperationResultList<T>> ToResultListAsync<T>(this IEnumerable<T> entity,
                        int pageNumber = 1,
                        int? pageSize = default,
                        int? count = default)
        {
            return await Task.FromResult(new OperationResultList<T>(HttpStatusCode.OK, default, entity, pageNumber, pageSize, count));
        }

        public static async Task<IOperationResultList<T>> ToResultListAsync<T>(this IEnumerable<T> entity,
                        HttpStatusCode status,
                        string message = default,
                        int pageNumber = 1,
                        int? pageSize = default,
                        int? count = default)
        {
            return await Task.FromResult(new OperationResultList<T>(status, message, entity, pageNumber, pageSize, count));
        }
        public static async Task<IOperationResultList<T>> ToResultListAsync<T>(this Exception ex)
        {
            return await Task.FromResult(new OperationResultList<T>(ex));
        }
    }
}

