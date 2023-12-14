using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Facilitat.CLOUD.Repositories.Generic;
using static Dapper.SqlMapper;

namespace Facilitat.CLOUD.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly IDbConnection _dbConnection;
        public GenericRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                int rowsEffected = await _dbConnection.ExecuteAsync(query, entity);
                return rowsEffected > 0;
            }
            catch (Exception ex)
            {
                // TODO: Handle the exception properly
                return false;
            }
        }

        public async Task<bool> Delete(T entity)
        {
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

                int rowsEffected = await _dbConnection.ExecuteAsync(query, entity);
                return rowsEffected > 0;
            }
            catch (Exception ex)
            {
                // TODO: Handle the exception properly
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName}";
                return await _dbConnection.QueryAsync<T>(query);
            }
            catch (Exception ex)
            {
                // TODO: Handle the exception properly
                return null;
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = @Id";
                return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                // TODO: Handle the exception properly
                return null;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");

                foreach (var property in GetProperties(true))
                {
                    var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                    string propertyName = property.Name;
                    string columnName = columnAttr?.Name ?? propertyName;
                    query.Append($"{columnName} = @{propertyName},");
                }

                query.Remove(query.Length - 1, 1);
                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                int rowsEffected = await _dbConnection.ExecuteAsync(query.ToString(), entity);
                return rowsEffected > 0;
            }
            catch (Exception ex)
            {
                // TODO: Handle the exception properly
                return false;
            }
        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name + "s";
        }

        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }


        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}