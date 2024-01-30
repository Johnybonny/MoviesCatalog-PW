using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.WebApp;
using Pasternak.MoviesCatalog.WebApp.Models;

namespace Pasternak.MoviesCatalog.WebApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly BLC.BLC _blc;

        public MoviesController(BLC.BLC blc)
        {
            _blc = blc;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieFilter, string filterType)
        {
            ViewBag.MoviesFilter = movieFilter;

            var movies = _blc.GetMovies();

            if (!string.IsNullOrEmpty(movieFilter))
            {
                switch (filterType)
                {
                    case "ReleaseYear":
                        movies = movies.Where(m => m.ReleaseYear.ToString() == movieFilter);
                        break;
                    case "Title":
                        movies = movies.Where(m => m.Title.Contains(movieFilter, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "MovieGenre":
                    default:
                        movies = movies.Where(m => m.MovieGenre.ToString().Contains(movieFilter, StringComparison.OrdinalIgnoreCase));
                        break;
                }
            }

            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _blc.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.Directors = _blc.GetDirectors();
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                _blc.CreateNewMovie(collection["Title"],
                    int.Parse(collection["ReleaseYear"]),
                    (Genre)Enum.Parse(typeof(Genre), collection["MovieGenre"]),
                    collection["Director"]);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            ViewBag.Directors = _blc.GetDirectors();
            if (id == null)
            {
                return NotFound();
            }

            var movie = _blc.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection)
        {
            try
            {
                _blc.EditMovie(collection["ID"],
                    collection["Title"],
                    int.Parse(collection["ReleaseYear"]),
                    (Genre)Enum.Parse(typeof(Genre), collection["MovieGenre"]),
                    collection["Director"]);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _blc.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var movie = _blc.GetMovie(id);
            if (movie != null)
            {
                _blc.DeleteMovie(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(string id)
        {
            return _blc.GetMovie(id) != null;
        }
    }
}
