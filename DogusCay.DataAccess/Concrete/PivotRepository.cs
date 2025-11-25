using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using Microsoft.EntityFrameworkCore;


namespace DogusCay.DataAccess.Concrete
{
    public class PivotRepository : IPivotRepository
    {
        private readonly DogusCayContext _context;

        public PivotRepository(DogusCayContext context)
        {
            _context = context;
        }

        public async Task<List<Dictionary<string, object>>> GetTableDynamicAsync(
            string tableName,
            string filterColumn = null,
            string filterValue = null)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var sql = $"SELECT * FROM {tableName}";

            if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                sql += $" WHERE {filterColumn} = @p0";

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            if (sql.Contains("@p0"))
            {
                var p = cmd.CreateParameter();
                p.ParameterName = "@p0";
                p.Value = filterValue;
                cmd.Parameters.Add(p);
            }

            var list = new List<Dictionary<string, object>>();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    row.Add(reader.GetName(i), reader.IsDBNull(i) ? null : reader.GetValue(i));

                list.Add(row);
            }

            await conn.CloseAsync();
            return list;
        }

    }
}