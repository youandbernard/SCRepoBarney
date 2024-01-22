using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Core.Shared.Models
{
    public class TreeNodeInput
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public bool? PartialSelected { get; set; } = false;
        public TreeNodeGroup Data { get; set; }
        public List<TreeNodeInput> Children { get; set; }
    }

    public class TreeNodeGroup
    {
        public string? Group { get; set; } = string.Empty;
        public bool? IsGroup { get; set; }
    }
}
