// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace SnomedApi.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ItemsPageReferenceSetMember
    {
        /// <summary>
        /// Initializes a new instance of the ItemsPageReferenceSetMember
        /// class.
        /// </summary>
        public ItemsPageReferenceSetMember()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ItemsPageReferenceSetMember
        /// class.
        /// </summary>
        public ItemsPageReferenceSetMember(object items = default(object), long? limit = default(long?), long? offset = default(long?), string searchAfter = default(string), IList<object> searchAfterArray = default(IList<object>), long? total = default(long?))
        {
            Items = items;
            Limit = limit;
            Offset = offset;
            SearchAfter = searchAfter;
            SearchAfterArray = searchAfterArray;
            Total = total;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public object Items { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "limit")]
        public long? Limit { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "offset")]
        public long? Offset { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "searchAfter")]
        public string SearchAfter { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "searchAfterArray")]
        public IList<object> SearchAfterArray { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public long? Total { get; set; }

    }
}
