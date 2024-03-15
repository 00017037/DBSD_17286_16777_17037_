using AutoMapper;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.DAL.Repositories;
using DBSD_17037_16777_17286.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly IRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        // GET: Employees
        public IActionResult Index()
        {
            
            var employees = _employeeRepository.GetAll();
            var viewModels = employees.Select(_mapper.Map<EmployeeViewModel>);
            return View(viewModels);
        }

        // GET: Employees/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeViewModel = _employeeRepository.GetById(id.Value);
            if (employeeViewModel == null)
            {
                return NotFound();
            }

            return View(employeeViewModel);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            // Populate data for dropdowns (if necessary) e.g., Departments
            // ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeeViewModel);
                _employeeRepository.Insert(employee);
                return RedirectToAction(nameof(Index));
            }
            // (Populate dropdown data again if needed)
            return View(employeeViewModel);
        }

        // GET: Employees/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee =  _employeeRepository.GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            // (Populate dropdown data again if needed)
            return View(_mapper.Map<EmployeeViewModel>(employee));
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<Employee>(employeeViewModel);
                    _employeeRepository.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employeeViewModel.Id))
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
            // (Populate dropdown data again if needed)
            return View(employeeViewModel);
        }

        // GET: Employees/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeViewModel = _employeeRepository.GetById(id.Value);
            if (employeeViewModel == null)
            {
                return NotFound();
            }

            return View(employeeViewModel);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeeRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        private bool EmployeeExists(int id)
        {
            return _employeeRepository.GetById(id) != null;
        }
    }
}
