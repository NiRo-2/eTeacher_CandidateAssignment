namespace Lms_Backend.Interfaces
{
    public interface IS3Service
    {
        Task UploadReportAsync(string bucketName, string key, string content);
    }
}