using System;
// using System.Security.Cryptography.X509Certificates;
// using Elastic.Clients.Elasticsearch;
// using Elastic.Transport;
// using Elasticsearch.Net;

namespace Services
{
    public class ElasticService
    {
        public void Connect()
        {
            // var uris = new[]
            // {
            //     new Uri("http://localhost:9200"),
            //     new Uri("http://localhost:9201"),
            //     new Uri("http://localhost:9202"),
            // };

            // var connectionPool = new SniffingConnectionPool(uris);
            // var settings = new ConnectionSettings(connectionPool)
            //     .DefaultIndex("people");

            // var client = new ElasticClient(settings);

            // var cert = new X509Certificate("/path/to/cluster-ca-certificate.pem");

            // var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
            //     .CertificateFingerprint("elastic-stack-ca.p12")
            //     .Authentication(new BasicAuthentication("elastic", "test"));

            // var client = new ElasticsearchClient(settings);


            // var settings = new ConnectionSettings(connectionPool)
            //     .BasicAuthentication("icelasticsearch", "<Password>")
            //     .ServerCertificateValidationCallback(
            //     CertificateValidations.AuthorityIsRoot(cert)
            //     )
            //     .DefaultMappingFor<Testing>(i => i.IndexName("testing"));
        }
    }
}