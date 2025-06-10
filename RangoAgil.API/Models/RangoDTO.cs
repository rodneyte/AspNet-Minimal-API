using System.ComponentModel.DataAnnotations;

namespace RangoAgil.API.Models;
public class RangoDTO
{
    public int Id { get; set; }
    [MaxLength(3)]
    public required string Nome { get; set; }
}

