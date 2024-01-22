using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Aws.S3.Model
{
    public class AwsConfiguration
    {
        public string Id { get; set; }
        public string Profile { get; set; }
        public string Region { get; set; }
        public string S3Bucket { get; set; }
    }
}
