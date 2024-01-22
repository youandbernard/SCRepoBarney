using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CaseMix.Aws.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Aws.S3.Services
{
    public class S3Service : IS3Service
    {
        private readonly AwsConfiguration _awsConfiguration;
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        public S3Service(IConfiguration configuration, IOptions<AwsConfiguration> awsConfiguration)
        {
            _awsConfiguration = awsConfiguration.Value;

            var options = configuration.GetAWSOptions();
            _s3Client = options.CreateServiceClient<IAmazonS3>();
        }

        public async Task UploadAsync(string fileName, byte[] fileBytes, string folder, long userId, bool isPrivate = false)
        {          
            if (!string.IsNullOrWhiteSpace(folder))
            {
                fileName = $"{folder}/{userId}/files/{fileName}";
            }

            var fileTransferUtility = new TransferUtility(_s3Client);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(fileBytes, 0, fileBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                string extension = Path.GetExtension(fileName);
                if (extension != ".csv")
                {
                    throw new Exception("Invalid file extension.");
                }

                string contentType = "text/csv";
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _awsConfiguration.S3Bucket,
                    InputStream = ms,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = fileBytes.Length,
                    Key = fileName,
                    CannedACL = isPrivate ? S3CannedACL.Private : S3CannedACL.PublicRead,
                    ContentType = contentType,
                };

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
        }

        public async Task<byte[]> DownloadAsync(string fileName, string folder, long userId, bool isPrivate = false)
        {
            if (!string.IsNullOrWhiteSpace(folder))
            {
                fileName = $"{folder}/{userId}/files/{fileName}";
            }

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _awsConfiguration.S3Bucket,
                Key = fileName,
            };
            using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            {
                byte[] buffer = new byte[responseStream.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
