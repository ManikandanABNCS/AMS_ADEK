using Kendo.Mvc.Extensions;
using ACS.AMS.DAL;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp
{
    public static class ACSDataSourceResultExtension
    {
        private static void UpdateCompositeFilterDescriptor(IList<IFilterDescriptor> compositefilterDescriptor)
        {
            if (compositefilterDescriptor == null) return;

            foreach (object filter in compositefilterDescriptor)
            {
                if (filter is FilterDescriptor)
                {
                    var filterDescriptor = (FilterDescriptor)filter;
                    if (filterDescriptor != null)
                    {
                        filterDescriptor.Member = filterDescriptor.Member.Replace("_", ".");
                    }
                }

                if (filter is CompositeFilterDescriptor)
                {
                    var filterDescriptor = (CompositeFilterDescriptor)filter;
                    if (filterDescriptor != null)
                    {
                        UpdateCompositeFilterDescriptor(filterDescriptor.FilterDescriptors);
                    }
                }
            }
        }
        public static DataSourceResult ToDataSourceResultForAsset(this DataSourceRequest request, IQueryable<AssetNewView> query, string columnIndexName = "", string primaryKeyField = "")
        {
            string f = string.Empty;
            if (request != null)
            {
                if (request.Filters != null)
                    query = query.Where(request.Filters) as IQueryable<AssetNewView>;

                if (request.Sorts != null)
                    query = query.Sort(request.Sorts) as IQueryable<AssetNewView>;
            }
            //var count = query.Count();
            // due to count return slow changed the code.  30/09/2020 Code changed 
            var count = query.GroupBy(i => i.AssetID).Count();

            var res = BaseEntityObject.ConvertToGridData(query, columnIndexName, primaryKeyField);
            // var count = ((request.Page - 1)  request.PageSize)+(res.Skip((request.Page - 1)  request.PageSize + 1).Take(request.PageSize + 1)).Count();

            if (request.PageSize > 0)
                res = res.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            //var list = res.ToList();
            //var cnt = res.Count();
            return new DataSourceResult
            {
                Data = res,
                Errors = "",
                AggregateResults = null,
                Total = count //result.Total
            };
        }

        public static DataSourceResult ToDataSourceResult<T>(this DataSourceRequest request, IQueryable<T> query, string columnIndexName = "", string primaryKeyField = "")
        {
            if (request != null)
            {
                if ((request.Filters != null) && (request.Filters.Count > 0))
                {
                    UpdateCompositeFilterDescriptor(request.Filters);

                    //foreach (object filter in request.Filters)
                    //{
                    //    if (filter is FilterDescriptor)
                    //    {
                    //        var filterDescriptor = (FilterDescriptor)filter;
                    //        if (filterDescriptor != null)
                    //        {
                    //            filterDescriptor.Member = filterDescriptor.Member.Replace("_", ".");
                    //        }
                    //    }

                    //    if (filter is CompositeFilterDescriptor)
                    //    {
                    //        var filterDescriptor = (CompositeFilterDescriptor) filter;
                    //        if (filterDescriptor != null)
                    //        {
                    //            UpdateCompositeFilterDescriptor(filterDescriptor);
                    //        }
                    //    }
                    //}

                    query = query.Where(request.Filters) as IQueryable<T>;
                }

                if ((request.Sorts != null) && (request.Sorts.Count > 0))
                {
                    foreach (SortDescriptor sort in request.Sorts)
                    {
                        if (sort != null)
                        {
                            sort.Member = sort.Member.Replace("_", ".");
                        }
                    }

                    query = query.Sort(request.Sorts) as IQueryable<T>;
                }
            }

            var count = query.Count();

            var list = BaseEntityObject.ConvertToGridData(query, columnIndexName, primaryKeyField);

            if (request.PageSize > 0)
                list = list.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            return new DataSourceResult
            {
                Data = list,
                Errors = "",
                AggregateResults = null,
                Total = count //result.Total
            };
        }

        //public static DataSourceResult ToTreeDataSourceResult<T>(this DataSourceRequest request, IQueryable<T> query, string columnIndexName = "", string primaryKeyField = "")
        //{
        //    if (request != null)
        //    {
        //        if ((request.Filters != null) && (request.Filters.Count > 0))
        //        {
        //            UpdateCompositeFilterDescriptor(request.Filters);

        //            query = query.Where(request.Filters) as IQueryable<T>;
        //        }

        //        if ((request.Sorts != null) && (request.Sorts.Count > 0))
        //        {
        //            foreach (SortDescriptor sort in request.Sorts)
        //            {
        //                if (sort != null)
        //                {
        //                    sort.Member = sort.Member.Replace("_", ".");
        //                }
        //            }

        //            query = query.Sort(request.Sorts) as IQueryable<T>;
        //        }
        //    }

        //    var count = query.Count();

        //    var list = BaseEntityObject.ConvertToGridData(query, columnIndexName, primaryKeyField);

        //    if (request.PageSize > 0)
        //        list = list.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        //    return new DataSourceResult
        //    {
        //        Data = list,
        //        Errors = "",
        //        AggregateResults = null,
        //        Total = count //result.Total
        //    };
        //}


    }
}
