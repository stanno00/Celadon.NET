using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs
{
    public class ResourcesDto
    {
        public List<ResourceDto> Resources { get; set; } = new List<ResourceDto>();
    }
}