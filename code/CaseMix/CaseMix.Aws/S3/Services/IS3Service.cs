using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Aws.S3.Services
{
    public interface IS3Service
    {
        Task UploadAsync(string fileName, byte[] fileBytes, string folder, long userId, bool isPrivate = false);
        Task<byte[]> DownloadAsync(string fileName, string folder, long userId, bool isPrivate = false);
    }
}
