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

    public partial class ConceptDescriptionsResult
    {
        /// <summary>
        /// Initializes a new instance of the ConceptDescriptionsResult class.
        /// </summary>
        public ConceptDescriptionsResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ConceptDescriptionsResult class.
        /// </summary>
        public ConceptDescriptionsResult(IList<Description> conceptDescriptions = default(IList<Description>))
        {
            ConceptDescriptions = conceptDescriptions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "conceptDescriptions")]
        public IList<Description> ConceptDescriptions { get; set; }

    }
}