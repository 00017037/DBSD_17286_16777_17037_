using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBSD_17037_16777_17286.Controllers
{
    public class EmployeeController : Controller
    {

        
            private readonly IRepository<Employee> _employeeRepository;

            public EmployeeController(IRepository<Employee> employeeRepository)
            {
                _employeeRepository = employeeRepository;
            }

            // GET: /Employees
            public IActionResult Index()
            {
                var employees = _employeeRepository.GetAll().ToList();
                return View(employees);
            }

            // GET: /Employees/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: /Employees/Create
            [HttpPost]
            [ValidateAntiForgeryToken] // Prevent CSRF attacks
            public IActionResult Create(Employee employee)
            {
                if (ModelState.IsValid)
                {
                    _employeeRepository.Insert(employee);
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }

            // GET: /Employees/Edit/5
            public IActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var employee = _employeeRepository.GetById(id.Value);
                return View(employee);
            }

            // POST: /Employees/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Employee employee)
            {
                if (ModelState.IsValid)
                {
                    _employeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }

            // GET: /Employees/Delete/5
            public IActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = _employeeRepository.GetById(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }

            // POST: /Employees/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                _employeeRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
        }
}
