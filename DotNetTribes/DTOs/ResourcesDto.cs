using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs
{
    public class ResourcesDto
    {
        public ICollection<ResourceDto> Resources { get; set; } = new List<ResourceDto>();
    }
}