using Dapper;
using ServiceRequestManagement.DAL.Interface;
using System.Data;
using System.Transactions;

namespace ServiceRequestManagement.DAL;

/// <summary>
/// Connect DB and manipulate the data
/// </summary>
public class Repository : IRepository
{
    private readonly IDbConnection _db;

    /// <summary>
    /// constructor to initialize the dependency
    /// </summary>
    /// <param name="db">IDbConnection</param>
    public Repository(IDbConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Inserting Records to database
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="sql"></param>
    /// <returns>int</returns>
    public async Task<int> InsertRecord(DynamicParameters parameter, string sql)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        OpenConnection();
        using IDbTransaction transaction = _db.BeginTransaction();
        try
        {
            int Item = await _db.ExecuteAsync(sql, parameter, commandType: CommandType.StoredProcedure);
            transaction.Commit();
            transactionScope.Complete();
            return Item;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            CloseConnection();
        }
    }
    /// <summary>
    /// Updating Records
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="sql"></param>
    /// <returns>int</returns>
    public async Task<int> UpdateRecord(DynamicParameters parameter, string sql)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        OpenConnection();
        using var transaction = _db.BeginTransaction();
        try
        {
            int Item = await _db.ExecuteAsync(sql, parameter, commandType: CommandType.StoredProcedure);
            transaction.Commit();
            transactionScope.Complete();
            return Item;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            CloseConnection();
        }
    }

    /// <summary>
    /// Delete Records
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="sql"></param>
    /// <returns>int</returns>
    public async Task<int> DeleteRecord(DynamicParameters parameter, string sql)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        OpenConnection();
        using IDbTransaction transaction = _db.BeginTransaction();
        try
        {
            var Item = await _db.ExecuteAsync(sql, parameter, commandType: CommandType.StoredProcedure);
            transaction.Commit();
            transactionScope.Complete();
            return Item;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            CloseConnection();
        }
    }

    /// <summary>
    /// Get Single Record
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="sql"></param>
    /// <returns>T</returns>
    public async Task<T> GetRecord<T>(DynamicParameters parameter, string sql)
    {
        try
        {
            OpenConnection();
            return await _db.QueryFirstOrDefaultAsync<T>(sql, parameter, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            CloseConnection();
            throw;
        }
        finally
        {
            CloseConnection();
        }
    }

    /// <summary>
    /// Get all records
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="sql"></param>
    /// <returns>List of T</returns>
    public async Task<IEnumerable<T>> GetAllRecords<T>(DynamicParameters parameter, string sql)
    {
        try
        {
            OpenConnection();
            return await _db.QueryAsync<T>(sql, parameter, commandType: CommandType.StoredProcedure, commandTimeout: 0);
        }
        catch (Exception)
        {
            CloseConnection();
            throw;
        }
        finally
        {
            CloseConnection();
        }
    }

    /// <summary>
    /// Open new connection to database
    /// </summary>
    private void OpenConnection()
    {
        CloseConnection();
        _db.Open();
    }

    /// <summary>
    /// Closed new connection to database
    /// </summary>
    private void CloseConnection()
    {
        if (_db.State == ConnectionState.Open)
            _db.Close();
    }
}
