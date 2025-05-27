using Amazon.S3.Model;
using Amazon.S3;
using Lms_Backend.Interfaces;

namespace Lms_Backend.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3Service> _logger;

        public S3Service(IAmazonS3 s3Client, ILogger<S3Service> logger)
        {
            _s3Client = s3Client;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a report to an S3 bucket asynchronously.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task UploadReportAsync(string bucketName, string key, string content)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                ContentBody = content,
                ContentType = "application/json"
            };

            //log
            _logger.LogInformation($"Uploading report to S3 bucket '{bucketName}' with key '{key}'");
            // Upload the object to S3
            await _s3Client.PutObjectAsync(request);
        }
    }
}