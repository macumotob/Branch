using _7E.Branch.Models;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace _7E.Branch.Helpers;
public static class DB
{


    public static string _server;



    private static string _connectionString = null;
    public static void use(string cnnString)
    {
        _connectionString = cnnString;
    }
    public static MySqlConnection open()
    {
        var conn = new MySqlConnection(_connectionString);
        conn.Open();
        if (conn.State == System.Data.ConnectionState.Open)
        {
            return conn;
        }
        else
        {
            throw new Exception("Could not open database connection");
        }
    }
    public static void close(MySqlConnection conn)
    {
        try
        {
            if (conn == null)
            {
                return;
            }
            conn.Close();
            conn.Dispose();
            conn = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public static int Exec(MySqlConnection conn, string sql, Dictionary<string, object> args = null)
    {
        try
        {
            var command = conn.CreateCommand();
            command.CommandText = sql;
            if (args != null)
            {
                foreach (var arg in args)
                {
                    command.Parameters.AddWithValue($"@{arg.Key}", arg.Value);
                }
            }

            var count = command.ExecuteNonQuery();
            command.Clone();
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;

        }
    }
    public static bool Exec(string sql, Dictionary<string, object> args = null)
    {
        try
        {

            var conn = open();

            if (conn.State == System.Data.ConnectionState.Open)
            {
                var command = conn.CreateCommand();
                //category.id = Guid.NewGuid().ToString();
                command.CommandText = sql;
                if (args != null)
                {
                    foreach (var arg in args)
                    {
                        command.Parameters.AddWithValue($"@{arg.Key}", arg.Value);
                    }
                }

                command.ExecuteNonQuery();
                //conn.Close();
                close(conn);
                return true;
            }
            //conn.Close();
            close(conn);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public static bool ExecSP(MySqlConnection conn, string procedure, Dictionary<string, object> args = null)
    {
        try
        {

            var command = conn.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = procedure;
            if (args != null)
            {
                foreach (var arg in args)
                {
                    command.Parameters.AddWithValue($"@{arg.Key}", arg.Value);
                }
            }
            var result = command.ExecuteNonQuery();
            return true;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }
    public static (MySqlDataReader reader, string error) ExecSPReader(MySqlConnection conn, string procedure, Dictionary<string, object> args = null)
    {
        try
        {

            var command = conn.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = procedure;
            if (args != null)
            {
                foreach (var arg in args)
                {
                    command.Parameters.AddWithValue($"@{arg.Key}", arg.Value);
                }
            }
            return (command.ExecuteReader(), null);


        }
        catch (Exception ex)
        {

            return (null, ex.Message);
        }

    }
    public static Result Execute<T>(T data, Func<MySqlCommand, T, Result> action)
    {
        MySqlConnection conn = null;
        try
        {
            var result = Result.Error("som error");

            using (conn = open())
            {
                using (var command = conn.CreateCommand())
                {
                    result = action(command, data);
                }
                DB.close(conn);
                return result;
            }

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
        finally
        {
            close(conn);
        }
    }
    public static Result Execute(Func<MySqlCommand, Result> action)
    {
        MySqlConnection conn = null;
        try
        {
            var result = Result.Error("som error");

            using (conn = open())
            {
                using (var command = conn.CreateCommand())
                {
                    result = action(command);
                }
                close(conn);
                return result;
            }

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
        finally
        {
            close(conn);
        }
    }
    public static Result Execute<T>(T data, Func<MySqlConnection, MySqlCommand, T, Result> action)
    {
        MySqlConnection conn = null;

        try
        {
            var result = Result.Error("som error");

            using (conn = open())
            {
                using (var command = conn.CreateCommand())
                {
                    result = action(conn, command, data);
                }
                close(conn);
                return result;
            }

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
        finally
        {
            close(conn);
        }
    }
    public static Result Execute<T>(T data, Func<MySqlConnection, T, Result> action)
    {
        MySqlConnection conn = null;

        try
        {
            var result = Result.Error("som error");

            using (conn = open())
            {
                using (var command = conn.CreateCommand())
                {
                    result = action(conn, data);
                }
                close(conn);
                return result;
            }

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
        finally
        {
            close(conn);
        }
    }

    public static Result Execute(Func<MySqlConnection, MySqlCommand, Result> action)
    {
        MySqlConnection conn = null;

        try
        {
            var result = Result.Error("som error");
            using (conn = open())
            {
                using (var command = conn.CreateCommand())
                {
                    result = action(conn, command);
                }
                close(conn);
                return result;
            }

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
        finally
        {
            close(conn);
        }
    }
}