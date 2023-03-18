using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WalkingDiary.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WalkingDiary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WalkingDiaryDbContext _context;
    private readonly IWebHostEnvironment _env;
    public HomeController(WalkingDiaryDbContext context, IWebHostEnvironment env, ILogger<HomeController> logger)
    {
        _logger = logger;
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var userId = await GetCurrentUserId();
        var walks = await _context.Walks.Where(w => w.UserId == userId).ToListAsync<Walk>();
        return View(walks);
    }

    [HttpPost]
    public async Task<IActionResult> AddWalk( Walk walk)
    {
        walk.UserId = await GetCurrentUserId();        
        walk.CreatedAt = DateTime.Now;
        walk.UpdatedAt = DateTime.Now;
        if (walk.Duration != null)
            walk.Duration = TimeSpan.FromMinutes(int.Parse(Request.Form["duration"]));

        if (ModelState.IsValid)
        {
            // Check that either duration or distance is specified
            if (walk.Duration == TimeSpan.Zero && walk.Distance <= 0)
            {
                ModelState.AddModelError("", "Either duration or distance must be specified");
                return View(walk);
            }

            // Save the walk to the database
            _context.Walks.Add(walk);
            await _context.SaveChangesAsync();

            // Upload the photos and save their URLs to the database
            if (Request.Form.Files.Count > 0)
            {
                foreach (var file in Request.Form.Files)
                {
                    var photo = new WalkPhoto();
                    photo.WalkId = walk.Id;
                    photo.Url = await SavePhotoAsync(file);
                    photo.CreatedAt = DateTime.Now;
                    _context.WalkPhotos.Add(photo);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // проходим по всем элементам в ModelState
        foreach (var item in ModelState)
        {
            // если для определенного элемента имеются ошибки
            if (item.Value.ValidationState == ModelValidationState.Invalid)
            {
                Console.WriteLine($"\nОшибки для свойства {item.Key}:\n");
                // пробегаемся по всем ошибкам
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine($"{error.ErrorMessage}\n");
                }
            }
        }
        return RedirectToAction("Index");
    }

    private async Task<string> SavePhotoAsync(IFormFile file)
    {
        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return "/uploads/" + fileName;
    }

    private async Task<int> GetCurrentUserId()
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
        return user.Id;
    }

    public IActionResult GetPhotos(int walkId)
    {
        var photos = _context.WalkPhotos.Where(p => p.WalkId == walkId).ToList();
        return PartialView("_Photos", photos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
