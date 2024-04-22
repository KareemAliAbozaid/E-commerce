using E_commerce.Web.Core.Consts;
using E_commerce.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;
using static System.Reflection.Metadata.BlobBuilder;

namespace E_commerce.Web.Controllers
{
    public class ProductsController : Controller
    {
        private List<string> _allawedExtension = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAlawedsize = 2097152;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductsController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult GetProducts()
        {
            var skip = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);



            var sortColumnIndex = Request.Form["order[0][column]"];
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];

            IQueryable<Product> products = _context.Products;
            if (!string.IsNullOrEmpty(searchValue))
                products = products.Where(p => p.Name.Contains(searchValue));
            products = products.OrderBy($"{sortColumn} {sortColumnDirection}");

            var data = products.Skip(skip).Take(pageSize).ToList();
            var mappedData = _mapper.Map<IEnumerable<ProductViewModel>>(data);
            var recordsTotal = products.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
            return Ok(jsonData);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(p => p.Category).SingleOrDefault(p => p.Id == id);
            if (product is null)
                return NotFound();
            var viewModel = _mapper.Map<ProductViewModel>(product);
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Form", PopulateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProdectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", PopulateViewModel(model));
            }
            var product = _mapper.Map<Product>(model);
            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allawedExtension.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAlawedExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxAlawedsize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxFilesize);
                    return View("Form", PopulateViewModel(model));
                }
                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/products", imageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                product.ImageUrl = $"/images/products/{imageName}"; ;
            }
            _context.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = product.Id });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product is null)
                return NotFound();
            var viewModel = _mapper.Map<ProdectFormViewModel>(product);
            return View("Form", PopulateViewModel(viewModel));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProdectFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var product = _context.Products.Find(model.Id);
            if (product is null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/products", product.ImageUrl);

                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allawedExtension.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAlawedExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxAlawedsize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxFilesize);
                    return View("Form", PopulateViewModel(model));
                }
                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/products", imageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                model.ImageUrl = $"/images/products/{imageName}"; ;
            }

            else if (model.Image is null && !string.IsNullOrEmpty(product.ImageUrl))
                model.ImageUrl = product.ImageUrl;

            product = _mapper.Map(model, product);
            product.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = product.Id });
        }

        public IActionResult AllowItem(ProdectFormViewModel model)
        {
            var product = _context.Products.SingleOrDefault(c => c.Name == model.Name && c.CategoryId == model.CategoryId);
            var isAllowed = product is null || product.Id.Equals(model.Id);

            return Json(isAllowed);
        }

        private ProdectFormViewModel PopulateViewModel(ProdectFormViewModel? model = null)
        {
            ProdectFormViewModel viewModel = model is null ? new ProdectFormViewModel() : model;

            var category = _context.Categories.OrderBy(a => a.Name).ToList();

            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(category);

            return viewModel;
        }
    }
}
