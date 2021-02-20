using Dapper;
using Dapper.Contrib.Extensions;
using MemberManagementSystem.Services.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Dynamic;

namespace MemberManagementSystem.Services
{
    public class Dapperr : IDapper
    {
        private readonly IConfiguration _config;
        private int _connectionTimeout = 20;

        public Dapperr(IConfiguration config)
        {
            _config = config;
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// 建立資料庫連線
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <returns>資料庫連線</returns>
        public IDbConnection GetDbConnection(string connectionString)
        {
            IDbConnection dBConnection;
            dBConnection = new SqlConnection(_config.GetConnectionString(connectionString));

            if (dBConnection.State != ConnectionState.Open)
            {
                dBConnection.Open();
            }

            return dBConnection;
        }

        #region Query 系列

        /// <summary>
        /// 查詢資料
        /// </summary>
        /// <typeparam name="TReturn">回覆的資料類型</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="querySql">SQL敘述</param>
        /// <param name="param">查詢參數物件</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>資料物件</returns>
        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string connectionString, string querySql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                return await con.QueryAsync<TReturn>(querySql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// 查詢第一筆資料
        /// (無結果回傳Null)
        /// </summary>
        /// <typeparam name="TResult">回傳的資料型態</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="querySql">SQL敘述</param>
        /// <param name="param">查詢參數</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>資料物件</returns>
        public async Task<TResult> QueryFirstOrDefaultAsync<TResult>(string connectionString, string querySql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<TResult>(querySql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// 查詢是否只有一筆資料
        /// (無資料回傳Null，多筆報Exception錯誤)
        /// </summary>
        /// <typeparam name="TResult">回傳的資料型態</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="querySql">SQL敘述</param>
        /// <param name="param">查詢參數</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>資料物件</returns>
        public async Task<TResult> QuerySingleOrDefaultAsync<TResult>(string connectionString, string querySql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                return await con.QuerySingleOrDefaultAsync<TResult>(querySql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
            }
        }
        #endregion

        #region Execute 系列

        /// <summary>
        /// Excute Non-Query SQL，允許一次傳入多道SQL指令
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="excuteSql">SQL敘述</param>
        /// <param name="param">參數物件</param>
        /// <param name="enableTransaction">包Transaction執行</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>影響資料筆數</returns>
        public async Task<int> ExecuteNonQueryAsync(string connectionString, string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                if (!enableTransaction)
                {
                    return await con.ExecuteAsync(excuteSql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
                }
                else
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var result = await con.ExecuteAsync(excuteSql, param, trans, _connectionTimeout, commandType).ConfigureAwait(false);
                            trans.Commit();
                            return result;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ExecuteScalar，執行查詢並傳回第一個資料列的第一個資料行中查詢所傳回的結果
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="excuteSql">SQL敘述</param>
        /// <param name="param">參數物件</param>
        /// <param name="enableTransaction">包Transaction執行</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>執行回覆結果</returns>
        public async Task<object> ExecuteScalarAsync(string connectionString, string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                if (!enableTransaction)
                {
                    return await con.ExecuteScalarAsync(excuteSql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
                }
                else
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var result = await con.ExecuteScalarAsync(excuteSql, param, trans, _connectionTimeout, commandType).ConfigureAwait(false);
                            trans.Commit();
                            return result;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
        #endregion

        #region Update 系列

        /// <summary>
        /// 更新單筆資料
        /// </summary>
        /// <typeparam name="T">更新資料物件Type</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="updateEntity">更新物件</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T>(string connectionString, T updateEntity)
            where T : class
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                return await con.UpdateAsync(updateEntity, null, _connectionTimeout).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="tableName">更新Table名稱</param>
        /// <param name="key">指定updateEntity裡面的Where Condition,如果多個key用","分隔, key must in updateEntity property</param>
        /// <param name="updateEntity">更新的物件</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string connectionString, string tableName, string keyName, object updateEntity)
        {
            if (string.IsNullOrWhiteSpace(keyName) || updateEntity == null)
            {
                return false;
            }

            string[] keyList = keyName.Split(',');
            List<string> keyFilter = new List<string>();
            List<string> updateProperties = new List<string>();
            foreach (var pro in updateEntity.GetType().GetProperties())
            {
                if (keyList.Contains(pro.Name))
                {
                    keyFilter.Add($"[{pro.Name}] {(pro.PropertyType.IsArray ? "in" : "=")} @{pro.Name}");
                }
                else
                {
                    updateProperties.Add($"[{pro.Name}] = @{pro.Name}");
                }
            }

            string sql = $"update [{tableName}] set {string.Join(", ", updateProperties)} where {string.Join(" and ", keyFilter)}";

            using (IDbConnection con = GetDbConnection(connectionString))
            {
                return await con.ExecuteAsync(sql, updateEntity, null, _connectionTimeout, CommandType.Text).ConfigureAwait(false) > 0;
            }
        }
        #endregion

        #region Insert 系列

        /// <summary>
        /// 新增單筆或多筆.
        /// </summary>
        /// <typeparam name="T">新增資料物件Type or IEnumable</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="insertEntity">新增物件</param>
        /// <param name="enableTransaction">是否使用Transaction</param>
        /// <returns>The ID(primary key) of the newly inserted record if it is identity using the defined type, otherwise null</returns>
        public async Task<int> InsertAsync<T>(string connectionString, T insertEntity, bool enableTransaction = false)
            where T : class
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                if (!enableTransaction)
                {
                    return await con.InsertAsync(insertEntity, null, _connectionTimeout).ConfigureAwait(false);
                }
                else
                {
                    using (var transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        try
                        {
                            var insertResult = await con.InsertAsync(insertEntity, transaction, _connectionTimeout).ConfigureAwait(false);
                            transaction.Commit();
                            return insertResult;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新增多筆資料 - 使用 SqlBulkCopy
        /// </summary>
        /// <typeparam name="T">資料物件類別</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="entities">資料物件集合</param>
        /// <returns></returns>
        public void BulkInsertAsyncUseSqlBulkCopy<T>(string connectionString, IEnumerable<T> entities)
        {
            using (IDbConnection conn = GetDbConnection(connectionString))
            {
                using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection))
                {
                    bulkCopy.DestinationTableName = ((TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true).First()).Name; // 資料實體對應的資料表名稱;
                    var table = new DataTable();

                    var properties = typeof(T).GetProperties();

                    foreach (var prop in properties)
                    {
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }

                    foreach (T item in entities)
                    {
                        DataRow row = table.NewRow();
                        foreach (var prop in properties)
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }

                        table.Rows.Add(row);
                    }

                    bulkCopy.WriteToServer(table);
                }
            }

        }
        #endregion

        #region Delete 系列

        /// <summary>
        /// 刪除單筆或多筆資料
        /// </summary>
        /// <typeparam name="T">刪除單筆資料 or 刪除多筆資料</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="deleteEntity">單筆資料 or 多筆資料</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(string connectionString, T deleteEntity)
            where T : class
        {
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                return await con.DeleteAsync(deleteEntity, null, _connectionTimeout).ConfigureAwait(false);
            }
        }
        #endregion
    }
}