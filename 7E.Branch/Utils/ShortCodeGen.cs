using Microsoft.EntityFrameworkCore;


namespace _7E.Branch.Utils;


public static class ShortCodeGen
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


    public static async Task<string> GenerateUniqueAsync(DbContext db, int len, CancellationToken ct)
    {
        var rnd = Random.Shared;
        while (true)
        {
            var code = new string(Enumerable.Range(0, len).Select(_ => Alphabet[rnd.Next(Alphabet.Length)]).ToArray());
            var exists = await db.Set<Models.Link>().AnyAsync(x => x.ShortCode == code, ct);
            if (!exists) return code;
        }
    }
}