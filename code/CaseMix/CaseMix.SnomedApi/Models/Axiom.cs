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

    public partial class Axiom
    {
        /// <summary>
        /// Initializes a new instance of the Axiom class.
        /// </summary>
        public Axiom()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Axiom class.
        /// </summary>
        public Axiom(bool? active = default(bool?), string axiomId = default(string), string definitionStatus = default(string), string definitionStatusId = default(string), int? effectiveTime = default(int?), string id = default(string), string moduleId = default(string), IList<Relationship> relationships = default(IList<Relationship>), bool? released = default(bool?))
        {
            Active = active;
            AxiomId = axiomId;
            DefinitionStatus = definitionStatus;
            DefinitionStatusId = definitionStatusId;
            EffectiveTime = effectiveTime;
            Id = id;
            ModuleId = moduleId;
            Relationships = relationships;
            Released = released;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "active")]
        public bool? Active { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "axiomId")]
        public string AxiomId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "definitionStatus")]
        public string DefinitionStatus { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "definitionStatusId")]
        public string DefinitionStatusId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "effectiveTime")]
        public int? EffectiveTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "moduleId")]
        public string ModuleId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "relationships")]
        public IList<Relationship> Relationships { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "released")]
        public bool? Released { get; set; }

    }
}