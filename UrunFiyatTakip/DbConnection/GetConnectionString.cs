namespace ProductsPrices.DbConnection
{
    public class GetConnectionString
    {
        public string GetConnection  
        {
            get { return DbConnectionStrings.localString; }
        }
    }
}
