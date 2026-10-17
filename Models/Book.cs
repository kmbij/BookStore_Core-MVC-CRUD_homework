using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "請輸入書名")]
    [StringLength(100)]
    [Display(Name = "書名")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入作者")]
    [StringLength(50)]
    [Display(Name = "作者")]
    public string Author { get; set; } = string.Empty;

    [Display(Name = "分類")]
    [StringLength(30)]
    public string? Category { get; set; }

    [Range(0, 100000, ErrorMessage = "價格需介於 0 ~ 100000")]
    [DataType(DataType.Currency)]
    [Display(Name = "價格")]
    public decimal Price { get; set; }

    [Range(0, 9999)]
    [Display(Name = "庫存")]
    public int Stock { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "出版日期")]
    public DateTime PublishDate { get; set; } = DateTime.Today;
}
