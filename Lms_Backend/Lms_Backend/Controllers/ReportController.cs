using Lms_Backend.Interfaces;
using Lms_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Lms_Backend.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly IDataContext _context;
        private readonly IS3Service _s3Service;
        private readonly IConfiguration _configuration;
        private readonly IEnrollmentService _enrollmentService;
        public ReportController(IDataContext context, IS3Service s3Service, IConfiguration configuration, IEnrollmentService enrollmentService)
        {
            _context = context;
            _s3Service = s3Service;
            _configuration = configuration;
            _enrollmentService = enrollmentService;
        }

        /// <summary>
        /// Uploads a report to S3 bucket asynchronously.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadReportToS3()
        {
            // Read bucket name dynamically from config + validate
            string bucketName = _configuration["AWS:S3BucketName"] ?? string.Empty;

            if (string.IsNullOrEmpty(bucketName))
                return BadRequest("S3 bucket name is not configured.");

            // Get all enrollment DTOs
            var dtos = _enrollmentService.GetAllEnrollmentDtos();

            // Serialize the DTOs to JSON
            string json = JsonSerializer.Serialize(dtos);
            await _s3Service.UploadReportAsync(bucketName, $"report-{DateTime.Now:yyyyMMddHHmmss}.json", json);

            return Ok("Report uploaded to S3 (simulated)");
        }
    }
}