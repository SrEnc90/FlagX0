namespace FlagX0.Web.DTOs;


// Estoy usando los constructores primarios dentro de mi record
public class FlagDto
{
    public string Name { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } 
    public int Id { get; set; }
}
