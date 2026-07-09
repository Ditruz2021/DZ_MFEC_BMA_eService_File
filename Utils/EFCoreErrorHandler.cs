using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.Data.SqlClient;

namespace dotnet_starter.Utils
{
    public static class EFCoreErrorHandler
    {
        public static string HandleException(Exception ex)
        {
            if (ex is DbUpdateException dbUpdateEx && dbUpdateEx.InnerException is PostgresException pgEx)
            {
                var field = ExtractConstraintField(pgEx);
                return pgEx.SqlState switch
                {
                    "23505" => $"Duplicate record: {field} already exists.",
                    "23503" => $"{field} - Foreign key constraint failed.",
                    "23514" => $"{field} - Check constraint violated.",
                    "23502" => $"{field} - Column cannot be null.",
                    "23P01" => $"{field} - Exclusion constraint violated.",
                    "22003" => $"{field} - Numeric value out of range.",
                    "22P02" => $"{field} - Invalid text format.",
                    "22001" => $"{field} - String value too long.",
                    "22008" => $"{field} - Invalid datetime format.",
                    "22012" => $"{field} - Division by zero.",
                    _ => $"Postgres error: {pgEx.MessageText}"
                };
            }

            if (ex is DbUpdateException sqlUpdateEx && sqlUpdateEx.InnerException is SqlException sqlEx)
            {
                return sqlEx.Number switch
                {
                    515 => $"Cannot insert null into a non-nullable column. {sqlEx.Message}",
                    2627 => $"Duplicate key value violates unique constraint. {sqlEx.Message}",
                    547 => $"Foreign key violation. {sqlEx.Message}",
                    2601 => $"Cannot insert duplicate key row. {sqlEx.Message}",
                    245 => $"Conversion failed (possibly string to number). {sqlEx.Message}",
                    241 => $"Invalid datetime format. {sqlEx.Message}",
                    _ => $"SQL Server error: {sqlEx.Message}"
                };
            }

            if (ex is DbUpdateConcurrencyException)
            {
                return $"The record was modified or deleted by another process. {ex.Message}";
            }

            return $"Unexpected database error occurred. {ex.Message}";
        }

        private static string ExtractConstraintField(PostgresException pgEx)
        {
            return !string.IsNullOrEmpty(pgEx.ConstraintName)
                ? $"Field '{pgEx.ConstraintName}'"
                : "A constraint";
        }
    }
}
