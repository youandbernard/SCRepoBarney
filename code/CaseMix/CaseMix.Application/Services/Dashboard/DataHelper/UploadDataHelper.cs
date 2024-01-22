using CaseMix.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Dashboard.DataHelper
{
    public class UploadDataHelper
    {
        public string PrepareDataHospital(Hospital hospital)
        {
            string resultData = string.Empty;

            var bulk = new
            {
                index = new
                {
                    _index = "hospitals",
                    _type = "_doc",
                    _id = hospital.Id
                }
            };

            var serializeBulk = JsonConvert.SerializeObject(bulk,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );

            resultData += serializeBulk;
            resultData += System.Environment.NewLine;

            var serializeData = JsonConvert.SerializeObject(hospital,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );

            resultData += serializeData;
            resultData += System.Environment.NewLine;


            return resultData;
        }
    }
}
