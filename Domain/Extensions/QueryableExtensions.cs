using System.Linq.Expressions;

namespace api.Domain.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Método para realizar filtro caso Condição seja true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query">A própria query que está realizando esta função</param>
    /// <param name="condition">Condição para realizar o filtro, senão retorna a própria lista</param>
    /// <param name="predicate">Expressão que irá aplicar ao Where</param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        if (condition)
            return query.Where(predicate).Distinct();

        return query;
    }

    /// <summary>
    /// Método para realizar filtro por múltiplos valores separados por virgula caso Condição seja true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query">A própria query que está realizando esta função</param>
    /// <param name="condition">Condição para realizar o filtro, senão retorna a própria lista</param>
    /// <param name="stringedArray">String completa que contém os filtros</param>
    /// <param name="predicate">Função que irá aplicar os valores do filtro</param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        string stringedArray,
        Func<T, string, bool> predicate)
    {
        if (condition)
        {
            var filters = stringedArray.Split(',').Select(s => s.Trim().ToLower());
            var foundDistributors = new List<T>();

            foreach (var filter in filters)
            {
                foundDistributors.AddRange(query.ToList().Where(q => predicate(q, filter)));
            }

            return foundDistributors.AsQueryable().Distinct();
        }
        return query;
    }
}