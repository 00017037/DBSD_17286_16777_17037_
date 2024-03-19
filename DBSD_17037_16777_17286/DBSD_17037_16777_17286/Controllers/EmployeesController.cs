using AutoMapper;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.DAL.Repositories;
using DBSD_17037_16777_17286.DAL.Repositories.DBSD_17037_16777_17286.DAL.Repositories;
using DBSD_17037_16777_17286.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IRepository<Employee> employeeRepository, IMapper mapper,IRepository<Person> personRepository,IRepository<Department> departmentRepository)
        
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            
            var employees = await _employeeRepository.GetAll();
            Console.WriteLine(employees);
            var viewModels =  employees.Select(_mapper.Map<EmployeeViewModel>);
            return View(viewModels);
        }

        // GET: Employees/Details/5
        public async Task< IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetById(id.Value);
            var employeeView =  _mapper.Map<EmployeeViewModel>(employee);
            if (employeeView == null)
            {
                return NotFound();
            }

            return View(employeeView);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var managers = await _employeeRepository.GetAll(); // Assuming you have a method to get all managers
            var departments = await _departmentRepository.GetAll(); // Assuming you have a method to get all departments

            // Populate SelectListItem for managers
            ViewBag.ManagerList = managers.Select(m => new SelectListItem
            {
                Text = $"{m.Person.FirstName} {m.Person.LastName}",
                Value = m.Id.ToString(),
            }).ToList();

            // Populate SelectListItem for departments
            ViewBag.DepartmentList = departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString(),
            }).ToList();
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

                var person = new Person { FirstName = employeeViewModel.FirstName,LastName = employeeViewModel.LastName };

                var createdPersonId = _personRepository.Insert(person);
                var employee = new Employee
                {
                    Id = employeeViewModel.Id,
                    ManagerId = employeeViewModel.ManagerId,
                    PersonId =createdPersonId,
                    DepartmentId = employeeViewModel.DepartmentId,
                    HireDate = employeeViewModel.HireDate,
                    HourlyRate = employeeViewModel.HourlyRate,
                    isMarried = employeeViewModel.IsMarried
                };
                _employeeRepository.Insert(employee);
                return RedirectToAction(nameof(Index));
            }

            // (Populate dropdown data again if needed)
            return View(employeeViewModel);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await  _employeeRepository.GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            // Fetch data for the dropdown lists
            var managers = await _employeeRepository.GetAll(); // Assuming you have a method to get all managers
            var departments = await _departmentRepository.GetAll(); // Assuming you have a method to get all departments

            // Populate SelectListItem for managers
            ViewBag.ManagerList = managers.Select(m => new SelectListItem
            {
                Text = $"{m.Person.FirstName} {m.Person.LastName}",
                Value = m.Id.ToString(),
                Selected = m.Id == employee.ManagerId // Set selected based on the employee's manager
            }).ToList();

            // Populate SelectListItem for departments
            ViewBag.DepartmentList = departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = d.Id == employee.DepartmentId // Set selected based on the employee's department
            }).ToList();
            var employeeView = _mapper.Map<EmployeeViewModel>(employee);
            // (Populate dropdown data again if needed)
            return View(employeeView);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var person = await _personRepository.GetById(employeeViewModel.PersonId);
                    person.FirstName = employeeViewModel.FirstName;
                    person.LastName = employeeViewModel.LastName;
                    _personRepository.Update(person);

                    var employee = new Employee
                    {
                        Id = employeeViewModel.Id,
                        ManagerId = employeeViewModel.ManagerId,
                        PersonId = employeeViewModel.PersonId,
                        DepartmentId = employeeViewModel.DepartmentId,
                        HireDate = employeeViewModel.HireDate,
                        HourlyRate = employeeViewModel.HourlyRate,
                        isMarried = employeeViewModel.IsMarried
                    };
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
        public async  Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee =  await _employeeRepository.GetById(id.Value);
            var employeeView = _mapper.Map<EmployeeViewModel>(employee);

            if (employeeView == null)
            {
                return NotFound();
            }

            return View(employeeView);
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
