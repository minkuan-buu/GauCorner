﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Data.Repositories.GenericRepositories
{
    public interface IGenericRepositories<T> where T : class
    {
        Task<IEnumerable<T>> GetList(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);

        Task<ListDataResultModel<T>> GetPagedList(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int pageIndex = 1,
            int pageSize = 10
        );

        Task<T> GetSingle(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task Insert(T entity);

        Task InsertRange(List<T> entity);

        Task Update(T entity);

        Task UpdateRange(List<T> entities);

        Task Delete(T entity);

        Task DeleteRange(IEnumerable<T> entities);
    }
}
