using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HourTracking.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using HourTracking.Models;
    using Microsoft.Data.Sqlite;

    namespace MyApp.Services
    {
        public class ZileService
        {
            private readonly string dbPath;

            public ZileService()
            {
                dbPath = Path.Combine(FileSystem.AppDataDirectory, "zilelucrate.db");
                Init();
            }

            private void Init()
            {
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ZileLucrate (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Data TEXT NOT NULL,
                    OreLucrate REAL NOT NULL,
                    Comentariu TEXT,
                    Platit INTEGER NOT NULL
                );";
                cmd.ExecuteNonQuery();
            }

            public void AdaugaZi(ZiLucru zi)
            {
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO ZileLucrate (Data, OreLucrate, Comentariu, Platit)
                VALUES ($data, $ore, $comentariu, $platit);";

                cmd.Parameters.AddWithValue("$data", zi.Data.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$ore", zi.OreLucrate);
                cmd.Parameters.AddWithValue("$comentariu", zi.Comentariu);
                cmd.Parameters.AddWithValue("$platit", zi.Platit ? 1 : 0);

                cmd.ExecuteNonQuery();
            }

            public List<ZiLucru> GetZile()
            {
                var rezultate = new List<ZiLucru>();

                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ZileLucrate ORDER BY Data DESC;";
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rezultate.Add(new ZiLucru
                    {
                        Id = reader.GetInt32(0),
                        Data = DateTime.Parse(reader.GetString(1)),
                        OreLucrate = reader.GetDouble(2),
                        Comentariu = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Platit = reader.GetInt32(4) == 1
                    });
                }

                return rezultate;
            }

            public double CalculeazaOreNeplatite()
            {
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT SUM(OreLucrate) FROM ZileLucrate WHERE Platit = 0;";
                var result = cmd.ExecuteScalar();

                return result != DBNull.Value && result != null ? Convert.ToDouble(result) : 0;
            }

            public void UpdateZi(ZiLucru zi)
            {
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE ZileLucrate
                    SET Data = $data,
                        OreLucrate = $ore,
                        Comentariu = $comentariu,
                        Platit = $platit
                    WHERE Id = $id;";

                cmd.Parameters.AddWithValue("$data", zi.Data.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$ore", zi.OreLucrate);
                cmd.Parameters.AddWithValue("$comentariu", zi.Comentariu);
                cmd.Parameters.AddWithValue("$platit", zi.Platit ? 1 : 0);
                cmd.Parameters.AddWithValue("$id", zi.Id);

                cmd.ExecuteNonQuery();
            }

            public void StergeZi(int id)
            {
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM ZileLucrate WHERE Id = $id;";
                cmd.Parameters.AddWithValue("$id", id);

                cmd.ExecuteNonQuery();
            }


        }

    }

}
