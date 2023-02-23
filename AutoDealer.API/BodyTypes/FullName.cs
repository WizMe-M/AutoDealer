using System.ComponentModel.DataAnnotations;

namespace AutoDealer.API.BodyTypes;

public record FullName([Required] string FirstName, [Required] string LastName, string MiddleName);