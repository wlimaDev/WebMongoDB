using MongoDB.Driver;

namespace WebMongoDB.Models
{
    public class ContextMongoDb
    {
        public static string  ConnectionString { get; set; }
        public static string DataBaseName { get; set; }
        public static bool IsSSL { get; set; }
        private IMongoDatabase _database { get; }



        public ContextMongoDb()
        {
            try
            {
                MongoClientSettings setting = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));

                if(IsSSL)
                {
                    setting.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    };
                }

                var mongoClient = new MongoClient(setting);
                _database = mongoClient.GetDatabase(DataBaseName);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possivel conectar");
            }
        }

        public IMongoCollection<Usuario> Usuario 
        {
            get { return _database.GetCollection<Usuario>("Usuario"); }
        }
    }
}
