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
                }),
                Trail = new Trail()
            };

            if (!id.HasValue) return View(viewModel);

            viewModel.Trail = await _trailRepository.GetById(AppConstants.TrailsApiPath, id.Value);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var nationalParks = await _nationalParkRepository.GetAll(AppConstants.NationalParkApiPath);

                viewModel.NationalParks = nationalParks.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });

                return View(viewModel);
            }

            if (viewModel.Trail.Id == 0)
            {
                await _trailRepository.CreateAsync(AppConstants.TrailsApiPath, viewModel.Trail);
            }
            else
            {
                await _trailRepository.UpdateAsync(AppConstants.TrailsApiPath, viewModel.Trail.Id, viewModel.Trail);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _trailRepository.DeleteAsync(AppConstants.TrailsApiPath, id))
            {
                return Json(new { success = true, message = "deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "something went wrong while deleting" });
            }
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _trailRepository.GetAll(AppConstants.TrailsApiPath) });
        }
    }
}
