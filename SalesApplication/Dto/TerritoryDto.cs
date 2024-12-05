﻿using SalesApplication.Models;

namespace SalesApplication.Dto
{
    public class TerritoryDto
    {
        public string TerritoryId { get; set; } = null!;

        public string TerritoryDescription { get; set; } = null!;

        public int RegionId { get; set; }

        //public virtual Region Region { get; set; } = null!;
    }
}
