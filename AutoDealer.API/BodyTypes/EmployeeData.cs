namespace AutoDealer.API.BodyTypes;

public record EmployeeData(FullName FullName, Passport Passport, Post Post);

public record FullName(string FirstName, string LastName, string? MiddleName);

public record Passport(string Series, string Number);