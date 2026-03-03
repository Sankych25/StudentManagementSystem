using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int pageSize = 5;

            var students = await _studentRepository
                .GetAllAsync(searchString, page, pageSize);

            int totalRecords = await _studentRepository
                .GetTotalCountAsync(searchString);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(students);
        }
        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentRepository.AddAsync(student);
                TempData["Success"] = "Student created successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Something went wrong while creating.";
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentRepository.GetByIdAsync(id.Value);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _studentRepository.UpdateAsync(student);

                TempData["Success"] = "Student updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Update failed!";
            return View(student);
        }
        // POST: Students/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                TempData["Error"] = "Student not found!";
                return RedirectToAction(nameof(Index));
            }

            await _studentRepository.DeleteAsync(id);

            TempData["Success"] = "Student deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}