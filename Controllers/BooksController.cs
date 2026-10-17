using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

public class BooksController : Controller
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context) => _context = context;

    // READ：列表（含關鍵字搜尋）
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Books.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));

        ViewData["Search"] = search;
        return View(await query.OrderBy(b => b.Id).ToListAsync());
    }

    // READ：單筆明細
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        return book == null ? NotFound() : View(book);
    }

    // CREATE
    public IActionResult Create() => View(new Book());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        if (!ModelState.IsValid) return View(book);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // UPDATE
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var book = await _context.Books.FindAsync(id);
        return book == null ? NotFound() : View(book);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book)
    {
        if (id != book.Id) return NotFound();
        if (!ModelState.IsValid) return View(book);

        try
        {
            _context.Update(book);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Books.Any(b => b.Id == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        return book == null ? NotFound() : View(book);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
