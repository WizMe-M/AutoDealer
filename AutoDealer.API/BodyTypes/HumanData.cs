namespace AutoDealer.API.BodyTypes;

public record EmployeeData(FullName FullName, Passport Passport, Post Post);

public record ClientData(FullName FullName, BirthData BirthData, FullPassport Passport);

public record FullName(string FirstName, string LastName, string? MiddleName);

public record BirthData(DateOnly Birthdate, string Birthplace);

public record Passport(string Series, string Number);

public record FullPassport(string Series, string Number, string Issuer, string DepartmentCode);