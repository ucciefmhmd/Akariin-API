using AutoMapper;
using Application.Utilities.Models;

namespace Application.Utilities.Contractors
{
    internal interface IMapProfile
    {
        IMappingExpression<BasePaginatedList<T>, BasePaginatedList<T>> GetBasePaginatedListConfiguration<T>();
    }
}
