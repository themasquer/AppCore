using AppCore.Business.Concretes.Models.DataTables;
using AppCore.Business.Concretes.Models.Results;
using AppCore.Business.Configs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AppCore.Business.Abstracts.Utils.DataTables
{
    // Reference: https://github.com/DavidSuescunPelegay/jQuery-datatable-server-side-net-core

    public abstract class DataTablesUtilBase
    {
        public virtual async Task<Result<DtResult<T>>> BindDataTable<T>(DtParameters dtParameters, IQueryable<T> query,
            Expression<Func<T, bool>> predicate = null, string orderExpressionValueSuffix = "Value")
        where T : class, new()
        {
            try
            {
                string orderExpression = null;
                bool orderDirectionAscending = true;
                if (dtParameters.Order != null)
                {
                    orderExpression = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderDirectionAscending = dtParameters.Order[0].Dir == DtOrderDir.Asc;
                }
                int recordsCount = await query.CountAsync();
                IQueryable<T> orderedQuery = OrderQuery(query, orderDirectionAscending, orderExpression, orderExpressionValueSuffix);
                IQueryable<T> filteredQuery;
                if (predicate == null)
                    filteredQuery = orderedQuery;
                else
                    filteredQuery = orderedQuery.Where(predicate);
                int filteredRecordsCount = await filteredQuery.CountAsync();
                var dataTable = new DtResult<T>()
                {
                    Draw = dtParameters.Draw,
                    RecordsTotal = recordsCount,
                    RecordsFiltered = filteredRecordsCount,
                    Data = await filteredQuery.Skip(dtParameters.Start).Take(dtParameters.Length).ToListAsync()
                };
                return new SuccessResult<DtResult<T>>(dataTable);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<DtResult<T>>(exc);
            }
        }

        public virtual Result<DtResult<T>> BindDataTable<T>(DtParameters dtParameters, List<T> list,
            Expression<Func<T, bool>> predicate = null, string orderExpressionValueSuffix = "Value")
        where T : class, new()
        {
            try
            {
                string orderExpression = null;
                bool orderDirectionAscending = true;
                if (dtParameters.Order != null)
                {
                    orderExpression = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderDirectionAscending = dtParameters.Order[0].Dir == DtOrderDir.Asc;
                }
                int recordsCount = list.Count;
                IQueryable<T> orderedQuery = OrderQuery(list.AsQueryable(), orderDirectionAscending, orderExpression, orderExpressionValueSuffix);
                IQueryable<T> filteredQuery;
                if (predicate == null)
                    filteredQuery = orderedQuery;
                else
                    filteredQuery = orderedQuery.Where(predicate);
                int filteredRecordsCount = filteredQuery.Count();
                var dataTable = new DtResult<T>()
                {
                    Draw = dtParameters.Draw,
                    RecordsTotal = recordsCount,
                    RecordsFiltered = filteredRecordsCount,
                    Data = filteredQuery.Skip(dtParameters.Start).Take(dtParameters.Length).ToList()
                };
                return new SuccessResult<DtResult<T>>(dataTable);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<DtResult<T>>(exc);
            }
        }

        public virtual Result AddDataTableOperations<T>(DtResult<T> dataTable, DataTableOperations operations)
        where T : class, new()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(operations.DetailsUrl) && String.IsNullOrWhiteSpace(operations.EditUrl) && String.IsNullOrWhiteSpace(operations.DeleteUrl))
                    return new ErrorResult(DataTablesUtilConfig.NoUrlsMessage);
                string linksValue;
                PropertyInfo linksProperty;
                PropertyInfo keyProperty;
                foreach (T data in dataTable.Data)
                {
                    linksProperty = data.GetType().GetProperty(operations.OperationLinksProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    keyProperty = data.GetType().GetProperty(operations.OperationKeyProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (linksProperty == null || keyProperty == null)
                        break;
                    linksValue = "";
                    if (!String.IsNullOrWhiteSpace(operations.DetailsUrl))
                        linksValue += "<a class='" + operations.DetailsCss + "' " +
                                      "href='" + operations.DetailsUrl + (operations.DetailsUrl.Contains("?") ? "&" : "?") +
                                      operations.OperationKeyProperty + "=" + keyProperty.GetValue(data) + "' " +
                                      "title='" + operations.DetailsTitle + "'>" +
                                      operations.DetailsText + "</a>";
                    if (!String.IsNullOrWhiteSpace(operations.EditUrl))
                        linksValue += "<a class='" + operations.EditCss + "' " +
                                      "href='" + operations.EditUrl + (operations.EditUrl.Contains("?") ? "&" : "?") +
                                      operations.OperationKeyProperty + "=" + keyProperty.GetValue(data) + "' " +
                                      "title='" + operations.EditTitle + "'>" +
                                      operations.EditText + "</a>";
                    if (!String.IsNullOrWhiteSpace(operations.DeleteUrl))
                        linksValue += "<a class='" + operations.DeleteCss + "' " +
                                      "href='" + operations.DeleteUrl + (operations.DeleteUrl.Contains("?") ? "&" : "?") +
                                      operations.OperationKeyProperty + "=" + keyProperty.GetValue(data) + "' " +
                                      "title='" + operations.DeleteTitle + "'>" +
                                      operations.DeleteText + "</a>";
                    linksProperty.SetValue(data, linksValue);
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        private IQueryable<T> OrderQuery<T>(IQueryable<T> query, bool orderDirectionAscending, string orderExpression, string orderExpressionValueSuffix)
            where T : class, new()
        {
            if (orderExpression == null)
                return query;
            PropertyInfo property = typeof(T).GetProperty(orderExpression, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                return query;
            PropertyInfo valueProperty = typeof(T).GetProperty(orderExpression + orderExpressionValueSuffix, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (valueProperty != null)
                orderExpression = orderExpression + orderExpressionValueSuffix;
            ParameterExpression parameter = Expression.Parameter(typeof(T), "c");
            Expression body = orderExpression.Split('.').Aggregate<string, Expression>(parameter, Expression.PropertyOrField);
            return orderDirectionAscending
                ? (IOrderedQueryable<T>)Queryable.OrderBy(query, (dynamic)Expression.Lambda(body, parameter))
                : (IOrderedQueryable<T>)Queryable.OrderByDescending(query, (dynamic)Expression.Lambda(body, parameter));
        }
    }
}
