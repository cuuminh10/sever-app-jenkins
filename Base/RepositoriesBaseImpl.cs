using gmc_api.Base.dto;
using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gmc_api.Base
{
    public class RepositoriesBaseImpl<T, P> : IRepositoriesBase<T, P> where T : class where P : class
    {
        private readonly GMCContext _context;
        private readonly string _tableName;
        private readonly string _columnId;
        public RepositoriesBaseImpl(GMCContext context, string tableName, string columnId)
        {
            _context = context;
            _tableName = tableName;
            _columnId = columnId;
        }

        public T AddObject(T obj)
        {
            if (obj.GetType().GetProperty("AAStatus") != null)
                obj.GetType().GetProperty("AAStatus").SetValue(obj, "Alive");
            var tmp = _context.Set<T>().Add(obj);
            try
            {
                _context.SaveChanges();
            }
            catch (SqlException)
            {
                return null;
            }
            return obj;
        }

        public int DeleteObject(int id)
        {
            var mapProp = new SortedDictionary<string, Object>
            {
                { _columnId, id }
            };
            var sqlBuilding = Utils.BuildingUpdateSQL(mapProp, this._tableName, this._columnId, true);
            int countUpdate;
            try
            {
                countUpdate = _context.Database.ExecuteSqlRaw(sqlBuilding);
            }
            catch (SqlException)
            {
                return 0;
            }
            return countUpdate;
        }

        public IEnumerable<T> GetAll(string conditionAppend = "")
        {
            var contextObj = _context.Set<T>();
            var sql = "SELECT * FROM dbo." + this._tableName + " Where AAStatus = 'Alive' ";
            if (!string.IsNullOrEmpty(conditionAppend))
            {
                sql += " AND " + conditionAppend;
            }
            return contextObj.FromSqlRaw(sql).ToList<T>();
        }
        public IEnumerable<P> GetPagingData(string conditionAppend, string orderByCondiion, Paging paging)
        {
            var contextObj = _context.Set<P>();
            var sql = "SELECT *, totalRows = COUNT(*) OVER() FROM dbo." + this._tableName + " Where AAStatus = 'Alive' ";
            if (!string.IsNullOrEmpty(conditionAppend))
            {
                sql += conditionAppend;
            }
            if (paging != null)
            {
                if (orderByCondiion.Trim().Length > 0)
                    sql += " ORDER BY " + orderByCondiion + " OFFSET " + (paging.pageNo - 1) * paging.numberRows + " ROWS FETCH NEXT " + paging.numberRows + " ROWS ONLY ";
            }
            else
            {
                sql += " ORDER BY " + orderByCondiion;
            }
            return contextObj.FromSqlRaw(sql).ToList<P>();
        }

        public IEnumerable<T> GetObjectByCondition(SortedDictionary<string, Object> mapProp)
        {
            var contextObj = _context.Set<T>();
            var sqlBuilding = Utils.BuildingSelectSQL(mapProp, this._tableName);
            return contextObj.FromSqlRaw(sqlBuilding).ToList<T>();
        }

        public T GetObjectById(int id)
        {
            var contextObj = _context.Set<T>();
            var sql = "SELECT * FROM dbo." + this._tableName + " Where AAStatus = 'Alive' AND " + _columnId + " = " + id;
            return contextObj.FromSqlRaw(sql).FirstOrDefault();
        }

        public int UpdateObject(SortedDictionary<string, object> mapProp)
        {
            var sqlBuilding = Utils.BuildingUpdateSQL(mapProp, this._tableName, this._columnId);
            int countUpdate;
            try
            {
                countUpdate = _context.Database.ExecuteSqlRaw(sqlBuilding);
            }
            catch (SqlException)
            {
                return 0;
            }
            return countUpdate;
        }
    }
}
