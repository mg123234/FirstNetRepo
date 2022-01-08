using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoMVC01.Models;
using System.Linq.Expressions;
using AutoMapper;
using DemoMVC01.Areas.AdminStudent.ViewModels;


namespace DemoMVC01.Areas.AdminStudent.Controllers
{
    [Area("AdminStudent")]
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        // Dependency Injection
        public StudentController(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: StudentController
        [HttpGet]
        public ActionResult Index(string searchValue, string orderBy, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "name_asc";
            }
            var segments = orderBy.Split("_");

            Expression<Func<Student, object>> orderByFunc;

            var students = _context.Students.Include(x => x.Major)
                .Where(x => string.IsNullOrEmpty(searchValue) || x.Name.Contains(searchValue));

            if (segments[0] == "phone")
            {
                orderByFunc = x => x.PhoneNumber;
            }
            else if (segments[0] == "email")
            {
                orderByFunc = x => x.EmailAddress;
            }
            else if (segments[0] == "majorName")
            {
                orderByFunc = x => x.Major.Name;
            }
            else
            {
                orderByFunc = x => x.Name;
            }

            if (segments[1] == "asc")
            {
                students = students.OrderBy(orderByFunc);
            }
            else
            {
                students = students.OrderByDescending(orderByFunc);
            }

            students = students.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var studentMax = _context.Students
                .Where(x => string.IsNullOrEmpty(searchValue) || x.Name.Contains(searchValue)) // linq
                .Count();

            ViewBag.MaxPage = Math.Ceiling((decimal)studentMax / pageSize);

            var result = _mapper.Map<List<StudentVM>>(students);

            return View(result);
        }



        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Major)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<StudentDetailsVM>(student);
            return View(result);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,EmailAddress,MajorId")] StudentCreateVM studentCreateVM)
        {

            if (ModelState.IsValid)
            {
                var result = _mapper.Map<Student>(studentCreateVM);
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name", studentCreateVM.MajorId);
            return View(studentCreateVM);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            var result = _mapper.Map<StudentEditVM>(student);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name", student.MajorId);
            return View(result);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,EmailAddress,MajorId")] StudentEditVM studentEditVM)
        {
            if (id != studentEditVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _mapper.Map<Student>(studentEditVM);
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(studentEditVM.Id))
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
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name", studentEditVM.MajorId);
            return View(studentEditVM);
        }

        // GET: Student/Delete/5
        /*
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Major)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }*/

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(List<int> idList)
        {
            Student student;
            foreach (var item in idList)
            {
                student = await _context.Students.FindAsync(item);
                _context.Students.Remove(student);
            }
            await _context.SaveChangesAsync();

            return Json(true);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
