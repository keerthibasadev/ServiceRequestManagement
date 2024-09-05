using Dapper;

namespace ServiceRequestManagement.DAL.Interface;

public interface IRepository
{
    //Inserting Records
    Task<int> InsertRecord(DynamicParameters parameter, string sql);

    //Updating Records
    Task<int> UpdateRecord(DynamicParameters parameter, string sql);

    //Delete Records
    Task<int> DeleteRecord(DynamicParameters parameter, string sql);

    //Get Single Record
    Task<T> GetRecord<T>(DynamicParameters parameter, string sql);

    //Get All Records
    Task<IEnumerable<T>> GetAllRecords<T>(DynamicParameters parameter, string sql);

}

