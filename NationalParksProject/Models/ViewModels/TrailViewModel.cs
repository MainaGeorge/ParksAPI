using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NationalParksProject.Models.ViewModels
{
    public class TrailViewModel
    {
        public  IEnumerable<SelectListItem> NationalParks { get; set; }

        public Trail Trail { get; set; }
    }
}
