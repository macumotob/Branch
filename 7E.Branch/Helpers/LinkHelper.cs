using _7E.Branch.Models;
using MySql.Data.MySqlClient;

namespace _7E.Branch.Helpers;

public static class LinkHelper
{
    public static Link ReadRecord(MySqlDataReader r)
    {
        return new Link
        {
            Id = r["Id"].ToString()!,
            ShortCode = r["ShortCode"].ToString()!,
            LongUrl = r["LongUrl"].ToString()!,
            Title = r["Title"] is DBNull ? null : r["Title"].ToString(),
            Description = r["Description"] is DBNull ? null : r["Description"].ToString(),
            MetadataJson = r["MetadataJson"].ToString(),
            ControlParamsJson = r["ControlParamsJson"].ToString(),
            CreatedAt = (r["CreatedAt"] is DateTime dt) ? dt : DateTime.Parse(r["CreatedAt"].ToString()!),
            click_count = r["click_count"] is long l ? l : Convert.ToInt64(r["click_count"])
        };
    }
    public static Result Get(int take, int skip)
    {
        return DB.Execute((cnn, cmd) =>
        {
            var list = new List<Link>();
            cmd.CommandText = "SELECT * FROM Links ORDER BY CreatedAt DESC LIMIT @take OFFSET @skip";
            cmd.Parameters.AddWithValue("@take", take);
            cmd.Parameters.AddWithValue("@skip", skip);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(ReadRecord(reader));
            }
            return Result.Success(list);
        });
    }
}