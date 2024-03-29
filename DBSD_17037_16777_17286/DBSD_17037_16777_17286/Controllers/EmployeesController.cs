using AutoMapper;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.DAL.Repositories;
using DBSD_17037_16777_17286.DAL.Repositories.DBSD_17037_16777_17286.DAL.Repositories;
using DBSD_17037_16777_17286.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;

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

        public IActionResult ExportToJson()
        {
            try
            {
                var json = _employeeRepository.ExportToJson(0, null, null, null, true, null, null);

                if (!string.IsNullOrEmpty(json))
                {

                    return File(Encoding.UTF8.GetBytes(json), "application/json", "employee.json");
                }
                else
                {
                    return Content("No data available to export to JSON.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting data to JSON: {ex.Message}");
                return Content("An error occurred while exporting data to JSON.");
            }
        }

        public IActionResult ExportToXml()
        {
            try
            {
                var xml = _employeeRepository.ExportToXml(0, null, null, null, false, null, null);

                if (!string.IsNullOrEmpty(xml))
                {
                    return File(Encoding.UTF8.GetBytes(xml), "application/xml", "employee.xml");
                }
                else
                {
                    return Content("No data available to export to XML.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting data to XML: {ex.Message}");
                return Content("An error occurred while exporting data to XML.");
            }
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


        //Filter: Employees/Filter 
        public async Task<int> GetTotalRows()
        {
            var employees = await this._employeeRepository.GetAll();
            int count = 0;
            foreach (var item in employees)
            {
                count++;
            }

            return count;
        }
        public async Task<IActionResult> Filter(EmployeeFilterModel filterModel)
        {
            try
            {
              
                int  totalRows = await GetTotalRows();

                filterModel.PageSize = 10;


                filterModel.TotalPages = (int)Math.Ceiling((double)totalRows / filterModel.PageSize);
                // Assuming EmployeeRepository has a method to filter employees
                var filteredEmployees = await _employeeRepository.Filter(
                    filterModel.FirstName,
                    filterModel.LastName,
                    filterModel.DepartmentName,
                    filterModel.HireDate,
                    filterModel.SortField,
                    filterModel.SortDesc,
                    filterModel.Page,
                    filterModel.PageSize);


                var viewModels = filteredEmployees.Select(_mapper.Map<EmployeeViewModel>);
                filterModel.Employees = viewModels;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            return View(filterModel);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {

                var person = new Person { FirstName = employeeViewModel.FirstName,LastName = employeeViewModel.LastName };

                var createdPersonId = _personRepository.Insert(person);
                employeeViewModel.PhotoFile = await HandlePhotoUpload(employeeViewModel.PhotoUpload);

                var employee = new Employee
                {
                    Id = employeeViewModel.Id,
                    ManagerId = employeeViewModel.ManagerId,
                    PersonId =createdPersonId,
                    Photo = employeeViewModel.PhotoFile,
                    DepartmentId = employeeViewModel.DepartmentId,
                    HireDate = employeeViewModel.HireDate,
                    HourlyRate = employeeViewModel.HourlyRate,
                    IsMarried = employeeViewModel.IsMarried
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
                    employeeViewModel.PhotoFile = await HandlePhotoUpload(employeeViewModel.PhotoUpload);


                    var employee = new Employee
                    {
                        Id = employeeViewModel.Id,
                        ManagerId = employeeViewModel.ManagerId,
                        PersonId = employeeViewModel.PersonId,
                        DepartmentId = employeeViewModel.DepartmentId,
                        HireDate = employeeViewModel.HireDate,
                        Photo = employeeViewModel.PhotoFile,
                        HourlyRate = employeeViewModel.HourlyRate,
                        IsMarried = employeeViewModel.IsMarried
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
        private async Task<byte[]> HandlePhotoUpload(IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photoFile.CopyToAsync(memoryStream);
                    return memoryStream.ToArray(); // Convert the uploaded photo to byte array
                }
            }

            return null; // Return null if no photo is uploaded
        }

        // Method to display image
        private string GetImageSrc(byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                var imageBase64Data = Convert.ToBase64String(imageData);
                return $"data:image/png;base64,{imageBase64Data}";
            }
            return null;
        }

        private string GetImageMimeType(byte[] imageData)
        {
            byte[] pngSignature = new byte[] { 137, 80, 78, 71 }; // PNG signature bytes
            byte[] jpgSignature = new byte[] { 255, 216, 255 }; // JPEG signature bytes

            if (imageData.Take(4).SequenceEqual(pngSignature))
            {
                return "png";
            }
            else if (imageData.Take(3).SequenceEqual(jpgSignature))
            {
                return "jpeg";
            }
            else
            {
                // If the format is unknown or unsupported, default to PNG
                return "png";
            }
        }
    }



}
