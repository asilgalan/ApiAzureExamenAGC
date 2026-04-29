using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

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
        public string GenerarUrlTemporalUsuario(string containerName, string nombreBlob)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(nombreBlob);

            // URL válida por 1 hora
            BlobSasBuilder sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = nombreBlob,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }
        public async Task<string> SubirImagenAsync(string containerName, IFormFile imagen)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);

            string nombreBlob = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
            BlobClient blobClient = containerClient.GetBlobClient(nombreBlob);


            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = imagen.ContentType
            };

            using (Stream stream = imagen.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            return nombreBlob;
        }

    }
}


