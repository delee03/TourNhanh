using Microsoft.AspNetCore.Mvc;
using TourNhanh.Controllers;
using TourNhanh.Models;
using TourNhanh.Repositories.Implementations;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TransportController : Controller
    {
        private readonly ITransportRepository _transportRepository;

        public TransportController(ITransportRepository transportRepository)
        {
            _transportRepository = transportRepository;
        }
        public async Task<ActionResult> Index()
        {
            var transports = await _transportRepository.GetAllAsync();
            ViewBag.transports = transports.ToList();
            return View(transports);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _transportRepository.GetByIdAsync(id.Value);
            if (transport == null)
            {
                return NotFound();
            }
            return View(transport);
        }


        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Transport transport)
        {
            if (ModelState.IsValid)
            {
                await _transportRepository.CreateAsync(transport);
                return RedirectToAction(nameof(Index));
            }

            return View(transport);
        }

        public async Task<IActionResult> Update(int id)
        {

            var transport = await _transportRepository.GetByIdAsync(id);
            if (transport == null)
            {
                return NotFound();
            }
            ViewBag.transport = transport;
            return View(transport);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Transport transport)
        {
            if (id != transport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingTransport = await _transportRepository.GetByIdAsync(id);

                existingTransport.Name = transport.Name;
                existingTransport.Description = transport.Description;

                await _transportRepository.UpdateAsync(existingTransport);
                return RedirectToAction(nameof(Index));
            }

            return View(transport);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var transport = await _transportRepository.GetByIdAsync(id);
            if (transport == null)
            {
                return NotFound();
            }
            ViewBag.transport = transport;
            return View(transport);
        }
        //Process the product deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _transportRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

     }
  }

