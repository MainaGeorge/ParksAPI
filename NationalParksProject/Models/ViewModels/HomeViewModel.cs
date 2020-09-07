using System.Collections.Generic;

namespace NationalParksProject.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }

        public IEnumerable<Trail> Trails { get; set; }
    }
}
