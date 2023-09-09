using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Database;


public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class Link
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public User User { get; set; }
    
    [Required]
    public string SourceLink { get; set; }
    
    [Required]
    public string ShortCode { get; set; }
    
    [DefaultValue(0)]
    public int Counter { get; set; }
    
    [Required]
    public DateTime Created { get; set; }
}