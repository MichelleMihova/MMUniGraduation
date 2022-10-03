using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models.Create;

namespace MMUniGraduation.Controllers
{
    public class CreateCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreateCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        // GET: CreateCourses
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.CreateCourse.ToListAsync());
        //}

        // GET: CreateCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var createCourse = await _context.CreateCourse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (createCourse == null)
            {
                return NotFound();
            }

            return View(createCourse);
        }

        // GET: CreateCourses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreateCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Signature,Description,ParetntID,CourseStartDate,SkipCoursEndDate")] CreateCourse createCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(createCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createCourse);
        }

        // GET: CreateCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var createCourse = await _context.CreateCourse.FindAsync(id);
            if (createCourse == null)
            {
                return NotFound();
            }
            return View(createCourse);
        }

        // POST: CreateCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Signature,Description,ParetntID,CourseStartDate,SkipCoursEndDate")] CreateCourse createCourse)
        {
            if (id != createCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(createCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreateCourseExists(createCourse.Id))
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
            return View(createCourse);
        }

        // GET: CreateCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var createCourse = await _context.CreateCourse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (createCourse == null)
            {
                return NotFound();
            }

            return View(createCourse);
        }

        // POST: CreateCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var createCourse = await _context.CreateCourse.FindAsync(id);
            _context.CreateCourse.Remove(createCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreateCourseExists(int id)
        {
            return _context.CreateCourse.Any(e => e.Id == id);
        }
    }
}
