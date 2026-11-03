using FluentValidation;

public class EmailValidator : AbstractValidator<EmailValidatorModel>
{
    public EmailValidator()
    {
        RuleFor(x => x.Email)
       .NotEmpty().WithMessage("Email is required.")
       .EmailAddress().WithMessage("Invalid email format.");
        //    .Must(BeFromSpecificDomain).WithMessage("Only emails from 'example.com' are allowed.");
    }
    // can use specific domain
    private bool BeFromSpecificDomain(string email)
    {
        var domain = email.Split('@').LastOrDefault();
        return domain == "example.com";
    }
}
public class PasswordValidator : AbstractValidator<EmailValidatorModel>
{
    public PasswordValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
