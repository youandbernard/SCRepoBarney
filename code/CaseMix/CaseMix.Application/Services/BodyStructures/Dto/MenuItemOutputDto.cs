using System.Collections.Generic;

namespace CaseMix.Services.BodyStructures.Dto
{
    public class MenuItemOutputDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool ShowButtonDevProc { get; set; } = false;

        public List<MenuItemOutputDto> Children { get; set; }
    }
}
