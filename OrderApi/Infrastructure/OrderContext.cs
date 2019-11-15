using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrderApi.Models;

namespace OrderApi.Infrastructure
{
    public class OrderContext
    {
        public IConfiguration configuration;
        public IMongoDatabase orderdatabase;

        public OrderContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = configuration.GetConnectionString("MongoDbConnection");

            MongoClientSettings settings = MongoClientSettings.FromUrl(
                new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            MongoClient client = new MongoClient(settings);
            if (client != null)
            {
                this.orderdatabase = client.GetDatabase(configuration.GetValue<string>("MongoDatabase"));
            }
        }

        public IMongoCollection<OrderItem> Order
        {
            get
            {
                return this.orderdatabase.GetCollection<OrderItem>("Orders");
            }
        }
    }
}


