using Azure.Storage.Blobs;

namespace ApiAzureExamenAGC.Services
{
    public class ServicesBlobStorage
    {

        private readonly BlobServiceClient client;

        public ServicesBlobStorage(BlobServiceClient client)
        {
            this.client = client;
        }

        public string GetContainerUrlAsync(string containerName)
        {

            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);

            return containerClient.Uri.AbsoluteUri;
        }
    }

}
