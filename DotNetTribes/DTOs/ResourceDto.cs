using System;
using DotNetTribes.Enums;

namespace DotNetTribes.DTOs
{
    public class ResourceDto
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public long UpdatedAt { get; set; }
    }
}