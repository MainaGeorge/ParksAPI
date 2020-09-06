using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParksProject.Models;
using NationalParksProject.Services.IRepository;
using System.IO;
using System.Linq;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (!ModelState.IsValid) return View(nationalPark);

            var uploadedImages = HttpContext.Request.Form.Files;

            await SetPictureProperty(uploadedImages, nationalPark);

            if (nationalPark.Id == 0)
            {
                await CreateNationalPark(nationalPark);
            }
            else
            {
                await UpdateNationalPark(nationalPark);
            }

            return RedirectToAction(nameof(Index));


        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _nationalParkRepository.DeleteAsync(AppConstants.NationalParkApiPath, id))
            {
                return Json(new { success = true, message = "successfully deleted" });
            }
            else
            {
                return Json(new { success = false, message = "something went wrong while deleting" });
            }

        }

        private async Task CreateNationalPark(NationalPark nationalPark)
        {
            await _nationalParkRepository.CreateAsync(AppConstants.NationalParkApiPath, nationalPark);
        }

        private async Task UpdateNationalPark(NationalPark nationalPark)
        {
            await _nationalParkRepository.UpdateAsync(AppConstants.NationalParkApiPath, nationalPark.Id, nationalPark);
        }

        private static byte[] ChangeImageToByteArray(IFormFileCollection files)
        {
            using var readStream = files[0].OpenReadStream();
            using var memoryStream = new MemoryStream();

            readStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        private async Task SetPictureProperty(IFormFileCollection uploadedImages, NationalPark nationalPark)
        {
            if (uploadedImages.Any())
            {
                nationalPark.Picture = ChangeImageToByteArray(uploadedImages);
            }
            else
            {
                if (nationalPark.Id != 0) // fetch only when updating
                {
                    var parkFromDb =
                        await _nationalParkRepository.GetById(AppConstants.NationalParkApiPath, nationalPark.Id);
                    nationalPark.Picture = parkFromDb.Picture;
                }
            }
        }
    }
}
