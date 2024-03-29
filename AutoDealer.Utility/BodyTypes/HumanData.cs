﻿namespace AutoDealer.Utility.BodyTypes;

public record EmployeeData(FullName FullName, Passport Passport, Post Post);

public record ClientData(FullName FullName, BirthData BirthData, FullPassport Passport);

public record FullName(string FirstName, string LastName, string? MiddleName);

public record BirthData(DateOnly Birthdate, string Birthplace);

public record Passport(string Series, string Number);

public record FullPassport(string Series, string Number, string Issuer, string DepartmentCode);

#region Validators

public class EmployeeDataValidator : AbstractValidator<EmployeeData>
{
    public EmployeeDataValidator()
    {
        RuleFor(data => data.FullName)
            .SetValidator(new FullNameValidator());
        RuleFor(data => data.Passport)
            .SetValidator(new PassportValidator());
    }
}

public class ClientDataValidator : AbstractValidator<ClientData>
{
    public ClientDataValidator()
    {
        RuleFor(data => data.FullName)
            .SetValidator(new FullNameValidator());
        RuleFor(data => data.BirthData)
            .SetValidator(new BirthDataValidator());
        RuleFor(data => data.Passport)
            .SetValidator(new FullPassportValidator());
    }
}

public class FullNameValidator : AbstractValidator<FullName>
{
    public FullNameValidator()
    {
        RuleFor(data => data.FirstName)
            .NotEmpty()
            .Length(0, 30)
            .WithName("First name");
        RuleFor(data => data.LastName)
            .NotEmpty()
            .Length(0, 30)
            .WithName("Last name");
        RuleFor(data => data.MiddleName)
            .Length(0, 30).When(name => name.MiddleName is { })
            .WithName("Middle name");
    }
}

public class BirthDataValidator : AbstractValidator<BirthData>
{
    public BirthDataValidator()
    {
        RuleFor(data => data.Birthdate)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithName("Date of birth");
        RuleFor(data => data.Birthplace)
            .NotEmpty()
            .Length(3, 200)
            .WithName("Place of birth");
    }
}

public class PassportValidator : AbstractValidator<Passport>
{
    public PassportValidator()
    {
        RuleFor(passport => passport.Series)
            .NotEmpty()
            .Matches(PassportRegex.SeriesRegex).WithMessage("{PropertyName} must be in format '1234'")
            .WithName("Passport series");

        RuleFor(passport => passport.Number)
            .NotEmpty()
            .Matches(PassportRegex.NumberRegex).WithMessage("{PropertyName} must be in format '123456'")
            .WithName("Passport number");
    }
}

public class FullPassportValidator : AbstractValidator<FullPassport>
{
    public FullPassportValidator()
    {
        RuleFor(passport => passport.Series)
            .NotEmpty()
            .Matches(PassportRegex.SeriesRegex).WithMessage("{PropertyName} must be in format '1234'")
            .WithName("Passport series");

        RuleFor(passport => passport.Number)
            .NotEmpty()
            .Matches(PassportRegex.NumberRegex).WithMessage("{PropertyName} must be in format '123456'")
            .WithName("Passport number");

        RuleFor(passport => passport.Issuer)
            .NotEmpty()
            .Length(10, 100);

        RuleFor(passport => passport.DepartmentCode)
            .NotEmpty()
            .Matches(PassportRegex.DepartmentCodeRegex).WithMessage("{PropertyName} must be in format '123-123'")
            .WithName("Department code");
    }
}

#endregion