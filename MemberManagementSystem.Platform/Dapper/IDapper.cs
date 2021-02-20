using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Services.Interface
{
    public interface IDapper : IDisposable
    {
        /// <summary>
        /// 建立資料庫連線
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <returns>資料庫連線</returns>
        IDbConnection GetDbConnection(string connectionString);

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
        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string connectionString, string querySql, object param = null, CommandType commandType = CommandType.Text);

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
        Task<TResult> QueryFirstOrDefaultAsync<TResult>(string connectionString, string querySql, object param = null, CommandType commandType = CommandType.Text);

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
        Task<TResult> QuerySingleOrDefaultAsync<TResult>(string connectionString, string querySql, object param = null, CommandType commandType = CommandType.Text);
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
        Task<int> ExecuteNonQueryAsync(string connectionString, string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text);

        /// <summary>
        /// ExecuteScalar，執行查詢並傳回第一個資料列的第一個資料行中查詢所傳回的結果
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="excuteSql">SQL敘述</param>
        /// <param name="param">參數物件</param>
        /// <param name="enableTransaction">包Transaction執行</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>執行回覆結果</returns>
        Task<object> ExecuteScalarAsync(string connectionString, string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text);
        #endregion

        #region Update 系列

        /// <summary>
        /// 更新單筆資料
        /// </summary>
        /// <typeparam name="T">更新資料物件Type</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="updateEntity">更新物件</param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(string connectionString, T updateEntity)
        where T : class;

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="tableName">更新Table名稱</param>
        /// <param name="keyName">指定updateEntity裡面的Where Condition,如果多個key用","分隔, key must in updateEntity property</param>
        /// <param name="updateEntity">更新的物件</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string connectionString, string tableName, string keyName, object updateEntity);
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
        Task<int> InsertAsync<T>(string connectionString, T insertEntity, bool enableTransaction = false)
            where T : class;

        /// <summary>
        /// 新增多筆資料 - 使用 SqlBulkCopy
        /// </summary>
        /// <typeparam name="T">資料物件類別</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="entities">資料物件集合</param>
        /// <returns></returns>
        void BulkInsertAsyncUseSqlBulkCopy<T>(string connectionString, IEnumerable<T> entities);
        #endregion

        #region Delete 系列
        /// <summary>
        /// 刪除單筆或多筆資料
        /// </summary>
        /// <typeparam name="T">刪除單筆資料 or 刪除多筆資料</typeparam>
        /// <param name="connectionString">連線字串</param>
        /// <param name="deleteEntity">單筆資料 or 多筆資料</param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(string connectionString, T deleteEntity)
            where T : class;
        #endregion
    }
}
