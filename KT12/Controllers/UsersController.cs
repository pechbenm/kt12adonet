using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using КТ12.Data;
using КТ12.Models;
using КТ12.ViewModels;

namespace КТ12.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context) => _context = context;

    // GET: /Users?search=...
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Users
            .Include(u => u.Profile)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(u =>
                u.UserName.Contains(search) ||
                u.Email.Contains(search) ||
                (u.Profile != null &&
                    (u.Profile.FirstName.Contains(search) || u.Profile.LastName.Contains(search))));
        }

        ViewData["Search"] = search;
        return View(await query.OrderBy(u => u.UserName).ToListAsync());
    }

    // GET: /Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var user = await _context.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user is null ? NotFound() : View(user);
    }

    // GET: /Users/Create
    public IActionResult Create() => View(new UserFormViewModel());

    // POST: /Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserFormViewModel model)
    {
        await ValidateUniquenessAsync(model);
        ValidateBirthDate(model);

        if (!ModelState.IsValid) return View(model);

        var user = new User
        {
            UserName = model.UserName.Trim(),
            Email = model.Email.Trim(),
            Profile = new UserProfile
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber?.Trim(),
                Address = model.Address?.Trim(),
                Bio = model.Bio?.Trim(),
                AvatarUrl = model.AvatarUrl?.Trim()
            }
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Пользователь «{user.UserName}» создан вместе с профилем.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var user = await _context.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null) return NotFound();

        return View(new UserFormViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.Profile?.FirstName ?? string.Empty,
            LastName = user.Profile?.LastName ?? string.Empty,
            DateOfBirth = user.Profile?.DateOfBirth,
            PhoneNumber = user.Profile?.PhoneNumber,
            Address = user.Profile?.Address,
            Bio = user.Profile?.Bio,
            AvatarUrl = user.Profile?.AvatarUrl
        });
    }

    // POST: /Users/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserFormViewModel model)
    {
        await ValidateUniquenessAsync(model);
        ValidateBirthDate(model);

        if (!ModelState.IsValid) return View(model);

        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == model.Id);

        if (user is null) return NotFound();

        user.UserName = model.UserName.Trim();
        user.Email = model.Email.Trim();

        user.Profile ??= new UserProfile();
        user.Profile.FirstName = model.FirstName.Trim();
        user.Profile.LastName = model.LastName.Trim();
        user.Profile.DateOfBirth = model.DateOfBirth;
        user.Profile.PhoneNumber = model.PhoneNumber?.Trim();
        user.Profile.Address = model.Address?.Trim();
        user.Profile.Bio = model.Bio?.Trim();
        user.Profile.AvatarUrl = model.AvatarUrl?.Trim();

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Данные пользователя «{user.UserName}» обновлены.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Users/Delete/5  (страница подтверждения)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var user = await _context.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user is null ? NotFound() : View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is not null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Пользователь «{user.UserName}» и его профиль удалены.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateUniquenessAsync(UserFormViewModel model)
    {
        var userName = (model.UserName ?? string.Empty).Trim();
        var email = (model.Email ?? string.Empty).Trim();

        if (await _context.Users.AnyAsync(u => u.Id != model.Id && u.UserName == userName))
            ModelState.AddModelError(nameof(model.UserName), "Это имя пользователя уже занято.");

        if (await _context.Users.AnyAsync(u => u.Id != model.Id && u.Email == email))
            ModelState.AddModelError(nameof(model.Email), "Пользователь с таким email уже существует.");
    }

    private void ValidateBirthDate(UserFormViewModel model)
    {
        if (model.DateOfBirth is { } date && date.Date > DateTime.Today)
            ModelState.AddModelError(nameof(model.DateOfBirth), "Дата рождения не может быть в будущем.");
    }
}
