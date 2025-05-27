using Amazon.S3.Model;
using Amazon.S3;
using Lms_Backend.Services;
using Moq;
using Microsoft.Extensions.Logging;

namespace Lms_Backend.Tests;

public class S3ServiceTests
{
    //Testing the S3Service class which uploads reports to an S3 bucket.
    [Fact]
    public async Task UploadReportAsync_ValidInput_CallsS3Client()
    {
        // Arrange
        var mockS3 = new Mock<IAmazonS3>();
        mockS3.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default))
              .ReturnsAsync(new PutObjectResponse());

        var s3Service = new S3Service(mockS3.Object, Mock.Of<ILogger<S3Service>>());

        string bucket = "test-bucket";
        string key = "test-report.json";
        string content = "{\"test\":\"data\"}";

        // Act
        await s3Service.UploadReportAsync(bucket, key, content);

        // Assert
        mockS3.Verify(s => s.PutObjectAsync(It.Is<PutObjectRequest>(r =>
            r.BucketName == bucket &&
            r.Key == key &&
            r.ContentBody == content
        ), default), Times.Once);
    }
}