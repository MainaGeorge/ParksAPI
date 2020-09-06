using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalParksProject.Models;
using NationalParksProject.Models.ViewModels;
using NationalParksProject.Services.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParksProject.Controllers
{
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailController(INationalParkRepository nationalParkRepository,
            ITrailRepository trailRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }
        public IActionResult Index()
        {
            return View(new Trail());
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParks = await _nationalParkRepository.GetAll(AppConstants.NationalParkApiPath);
            var viewModel = new TrailViewModel()
            {
                NationalParks = nationalParks.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                })
            };

            if (!id.HasValue) return View(viewModel);

            viewModel.Trail = await _trailRepository.GetById(AppConstants.TrailsApiPath, id.Value);

            return View(viewModel);
        }
    }
}
