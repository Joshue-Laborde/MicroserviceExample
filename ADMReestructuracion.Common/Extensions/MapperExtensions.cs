using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Extensions
{
    public static class MapperExtensions
    {
        public static TDestination Map<TSource1, TSource2, TDestination>(this IMapper mapper, TSource1 source1, TSource2 source2)
        {
            var destination = mapper.Map<TSource1, TDestination>(source1);
            return mapper.Map(source2, destination);
        }

        public static TDestination Map<TDestination>(this IMapper mapper, params object[] sources) where TDestination : new()
        {
            return Map(mapper, new TDestination(), sources);
        }

        public static TDestination Map<TDestination>(this IMapper mapper, TDestination destination, params object[] sources) where TDestination : new()
        {
            if (!sources.Any())
                return destination;

            foreach (var src in sources)
            {
                try
                {

                    destination = mapper.Map(src, destination);

                }
                catch (Exception)
                { }
            }

            return destination;
        }

        public static IMappingExpression<TSource, TDestination> ForAllMembersIfNotEmpty<TSource, TDestination>(
          this IMappingExpression<TSource, TDestination> expression)
        {
            ForAllMembers(expression);
            var newExpresion = expression.ReverseMap();
            ForAllMembers(newExpresion);
            return expression;
        }

        private static void ForAllMembers<TSource, TDestination>(IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (srcMember == null) return false;

                if (srcMember is int || srcMember is decimal)
                {
                    return Convert.ToDecimal(srcMember) != 0;
                }

                return true;
            }));
        }
    }
}
