using Microsoft.AspNetCore.Mvc;
using NationalParksProject.Models;
using NationalParksProject.Services.IRepository;
using System.Threading.Tasks;

namespace NationalParksProject.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View(new NationalPark());
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _nationalParkRepository.GetAll(AppConstants.NationalParkApiPath) });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalPark = new NationalPark();

            if (!id.HasValue) return View(nationalPark);

            nationalPark = await _nationalParkRepository.GetById(AppConstants.NationalParkApiPath, id.Value);

            if (nationalPark == null) return NotFound();

            return View(nationalPark);
        }
    }
}
