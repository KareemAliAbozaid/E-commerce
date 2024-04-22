namespace E_commerce.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var category = _context.Categories.AsNoTracking().ToList();
            var viewModel=_mapper.Map<IEnumerable<CategoryViewModel>>(category);
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);
            var category = _mapper.Map<Category>(model);
            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["Message"] = "Saved Successfully";
            var viewModel = _mapper.Map<CategoryViewModel>(category);
            return RedirectToAction(nameof(Index),viewModel);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
                return NotFound();
            var viewModel =_mapper.Map<CategoryFormViewModel>(category);
            return View("Form", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);
            var category = _context.Categories.Find(model.Id);
            if (category is null)
                return NotFound();
            category=_mapper.Map(model, category);
            category.DisplayOrder = model.DisplayOrder;
            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            TempData["Message"] = "Saved Successfully";
            var viewModel =_mapper.Map<CategoryViewModel>(category);    
            return RedirectToAction(nameof(Index), viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
                return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok();
        }
    }
}
