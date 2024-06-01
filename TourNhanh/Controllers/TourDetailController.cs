using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;
using TourNhanh.ViewModel;

namespace TourNhanh.Controllers
{
    public class TourDetailController:Controller
    {
        private readonly ITourDetail _tourDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ITourImage _tourImageRepository;
        private readonly IReviewRepository _reviewRepository;

        public TourDetailController(ITourDetail tourDetailRepository, ILocationRepository locationRepository, IHotelRepository hotelRepository, ITourRepository tourRepository, ITourImage tourImageRepository, IReviewRepository reviewRepository)
        {
            _tourDetailRepository = tourDetailRepository;
            _locationRepository = locationRepository;
            _hotelRepository = hotelRepository;
            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
            _reviewRepository = reviewRepository;
    }



        // GET: TourDetails
        public async Task<IActionResult> Index(int tourId)
        {
            var details = await _tourDetailRepository.GetByTourIdAsync(tourId);


            /*  List<string> locationDetails = new List<string>();

              foreach (var list in details)
              {
                  if (list.Location != null && list.Location.Address != null)
                  {
                      locationDetails.AddRange(list.Location.Address.Split('-'));
                  }
              }             
              //mỗi tour quy định có tối đa là 3 địa điểm tối thiểu là 1 địa điểm, môi địa điểm có 3 điểm tham quan nữa 
              if (locationDetails.Count > 9)
              {
                  locationDetails.RemoveAt(locationDetails.Count - 1);            
              }

              switch (locationDetails.Count)
              {
                  case 9:
                      ViewBag.Location1 = locationDetails[0];
                      goto case 8;
                  case 8:
                      ViewBag.Location2 = locationDetails[1];
                      goto case 7;
                  case 7:
                      ViewBag.Location3 = locationDetails[2];
                      goto case 6;
                  case 6: 
                      ViewBag.Location4 = locationDetails[3];
                      goto case 5;
                  case 5:
                      ViewBag.Location5 = locationDetails[4];
                      goto case 4;
                  case 4:
                       ViewBag.Location6 = locationDetails[5];
                       goto case 3;            
                  case 3:             
                      ViewBag.Location7= locationDetails[6];
                      goto case 2;
                  case 2:         
                      ViewBag.Location8 = locationDetails[7];
                      goto case 1;
                  case 1:            
                      ViewBag.Location9 = locationDetails[8];
                      break;
              }
              //Lấy ra Address của Location => cắt từng phần tử theo chuỗi sau dấu phẩy

            */
            foreach(var item in details)
            {
                ViewBag.Slots = item.Tour.RemainingSlots;
            }

            ViewBag.TourId = tourId;
            List<string> imageURL = new List<string>();       
            var tourimage = await _tourImageRepository.GetByTourIdAsync(tourId);
            foreach(var img in tourimage)
            {
               imageURL.Add(img.ImageUrl);         
            }
            ViewBag.TourImage = imageURL;

            //lấy trung bình đánh giá
            var reviews = await _reviewRepository.GetReviewsByTourId(tourId);
            
            int countRating1 = reviews.Count(v => v.Rating == 1);
            int countRating2 = reviews.Count(v => v.Rating == 2);
            int countRating3 = reviews.Count(v => v.Rating == 3);
            int countRating4 = reviews.Count(v => v.Rating == 4);
            int countRating5 = reviews.Count(v => v.Rating == 5);
            int totalCount = countRating1 + countRating2 + countRating3 + countRating4 + countRating5;
            // Tính trung bình cộng
            float average = (countRating1 * 1 + countRating2 * 2 + countRating3 * 3 + countRating4 * 4 + countRating5 * 5) / (float)totalCount;
            ViewBag.AverageRating = average;
            ViewBag.Count = reviews.Count();
            //Lấy ra list các ảnh
            var tour = await _tourRepository.GetByIdAsync(tourId);
            ViewBag.Tour = tour;
            // lấy ra đúng model Tour từ Tour Detail
            return View(details);
        }

        // GET: TourDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.GetByIdAsync(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }

            return View(tourDetail);
        }

        // GET: TourDetails/Create
        public async Task<IActionResult> Create(int tourId)
        {
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name", "Address");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name", "Address");
            var tourDetail = new TourDetail { TourId = tourId };
            ViewBag.TourId = tourId;
            return View(tourDetail);
        }

        // POST: TourDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (ModelState.IsValid)
            {
                await _tourDetailRepository.CreateAsync(tourDetail);
                return RedirectToAction(nameof(Index), new { tourId = tourDetail.TourId });
            }
            return View(tourDetail);
        }

        // GET: TourDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.GetByIdAsync(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name");
            return View(tourDetail);
        }

        // POST: TourDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (id != tourDetail.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _tourDetailRepository.UpdateAsync(tourDetail);
                return RedirectToAction(nameof(Index), new { tourId = tourDetail.TourId });
            }
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name");
            return View(tourDetail);
        }

        // GET: TourDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.GetByIdAsync(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }

            return View(tourDetail);
        }

        // POST: TourDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tourDetail = await _tourDetailRepository.GetByIdAsync(id);
            var tourId = tourDetail?.TourId;
            await _tourDetailRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { tourId = tourId });
        }
    }
}