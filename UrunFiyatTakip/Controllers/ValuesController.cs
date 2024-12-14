using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ProductsPrices.DbConnection;
using UrunFiyatTakip.Entity;

namespace UrunFiyatTakip.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;
        private IConfiguration _configuration;
        public ValuesController(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        [HttpPost("GetUrun")]
        public IActionResult GetUrun()
        {
            var urunler = new List<Urun>();
            using (IDbConnection db = new SqlConnection(new GetConnectionString().GetConnection))
            {
                urunler = db.Query<Urun>("Select * From Urunler").ToList();
            }
            return Ok(urunler);
        }

        [HttpPost("CreateUrun")]
        public IActionResult CreateUrun(Urun urun)
        {
            using (IDbConnection db = new SqlConnection(new GetConnectionString().GetConnection))
            {
                string sqlQuery = @"INSERT INTO Urunler (UrunAdi, UrunStok, UrunFiyat1, UrunFiyat2, UrunFiyat3, UrunFiyat4) 
                                    OUTPUT INSERTED.UrunId 
                                    VALUES (@UrunAdi, @UrunStok, @UrunFiyat1, @UrunFiyat2, @UrunFiyat3, @UrunFiyat4);";
                                    

                return Ok(db.Query(sqlQuery, urun));
            }

        }
        [HttpPost("UpdateUrun")]
        public IActionResult UpdateUrun(Urun urun)
        {
            using (IDbConnection db = new SqlConnection(new GetConnectionString().GetConnection))
            {
                string sqlQuery = @"UPDATE Urunler SET 
                                    UrunAdi = @UrunAdi,
                                    UrunStok = @UrunStok,
                                    UrunFiyat1 = @UrunFiyat1,
                                    UrunFiyat2 = @UrunFiyat2,
                                    UrunFiyat3 = @UrunFiyat3,
                                    UrunFiyat4 = @UrunFiyat4
                                    OUTPUT INSERTED.UrunId
                                    WHERE UrunId = @UrunId;";
                int rowsAffected = db.Execute(sqlQuery, urun);
                return Ok(rowsAffected);
            }
        }
        [HttpPost("DeleteUrun")]
        public IActionResult DeleteUrun(int urunId)
        {
            using (IDbConnection db = new SqlConnection(new GetConnectionString().GetConnection))
            {
                string sqlQuery = @"DELETE FROM Urunler WHERE UrunId = @UrunId";
                int rowsAffected = db.Execute(sqlQuery, new { UrunId = urunId });
                return Ok(rowsAffected);
            }
        }


    }
}
