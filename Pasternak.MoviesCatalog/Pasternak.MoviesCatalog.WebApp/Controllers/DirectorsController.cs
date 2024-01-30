using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pasternak.MoviesCatalog.WebApp;
using Pasternak.MoviesCatalog.WebApp.Models;

namespace Pasternak.MoviesCatalog.WebApp.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly BLC.BLC _blc;

        public DirectorsController(BLC.BLC blc)
        {
            _blc = blc;
        }

        // GET: Directors
        public async Task<IActionResult> Index(string directorFilter, string filterType)
        {
            ViewBag.DirectorFilter = directorFilter;

            var directors = _blc.GetDirectors();

            if (!string.IsNullOrEmpty(directorFilter))
            {
                switch (filterType)
                {
                    case "Age":
                        directors = directors.Where(d => d.Age.ToString() == directorFilter);
                        break;
                    case "Nationality":
                        directors = directors.Where(d => d.Nationality.Contains(directorFilter, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "Name":
                    default:
                        directors = directors.Where(d => d.Name.Contains(directorFilter, StringComparison.OrdinalIgnoreCase));
                        break;
                }
            }

            return View(directors);
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = _blc.GetDirector(id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age,Nationality")] Director director)
        {
            _blc.CreateNewDirector(director.Name, director.Age, director.Nationality);
            return RedirectToAction(nameof(Index));
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = _blc.GetDirector(id);
            if (director == null)
            {
                return NotFound();
            }
            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Age,Nationality")] Director director)
        {
            if (id != director.ID)
            {
                return NotFound();
            }

            try
            {
                _blc.EditDirector(id, director.Name, director.Age, director.Nationality);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(director.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = _blc.GetDirector(id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var director = _blc.GetDirector(id);
            if (director != null)
            {
                _blc.DeleteDirector(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(string id)
        {
            return _blc.GetDirector(id) != null;
        }
    }
}
