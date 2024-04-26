using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Controllers
{
    public class TransportController : Controller
    {
        private readonly ITransportRepository _transportRepository;

        public TransportController(ITransportRepository transportRepository)
        {
            _transportRepository = transportRepository;
        }

        // GET: Transports
        public async Task<IActionResult> Index()
        {
            return View(await _transportRepository.GetAllAsync());
        }

        // GET: Transports/Details/5
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

        // GET: Transports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Transport transport)
        {
            if (ModelState.IsValid)
            {
                await _transportRepository.CreateAsync(transport);
                return RedirectToAction(nameof(Index));
            }
            return View(transport);
        }

        // GET: Transports/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Transports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Transport transport)
        {
            if (id != transport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _transportRepository.UpdateAsync(transport);
                return RedirectToAction(nameof(Index));
            }
            return View(transport);
        }

        // GET: Transports/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Transports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _transportRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
