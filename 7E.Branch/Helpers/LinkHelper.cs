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
    private static void SetCommandParameters(MySqlCommand cmd,Link item)
    {
        cmd.Parameters.AddWithValue("@p_id", item.Id);
        cmd.Parameters.AddWithValue("@p_shortcode", item.ShortCode);
        cmd.Parameters.AddWithValue("@p_longurl", item.LongUrl);
        cmd.Parameters.AddWithValue("@p_title", item.Title ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@p_description", item.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@p_metadata_json", string.IsNullOrEmpty(item.MetadataJson) ? DBNull.Value : item.MetadataJson);
        cmd.Parameters.AddWithValue("@p_control_params_json", string.IsNullOrEmpty(item.ControlParamsJson) 
            ? DBNull.Value : item.ControlParamsJson);
    }

    public static Result Create(Link item)
    {
        return DB.Execute((cnn, cmd) =>
        {
            cmd.CommandText = "sp_upsert_link";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetCommandParameters(cmd,item);
            var affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                return Result.Success(item);
            }
            return Result.Error("Failed to create link");
        });
    }

}