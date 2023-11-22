using FluentValidation;
using TestNetAPI.Entities;

namespace TestNetAPI.Models.Validators
{
    public class RegisterUserDtoVaildator :AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoVaildator(NetDbContext dbContext)
        {
            RuleFor(x=>x.Email).NotEmpty().EmailAddress();
            RuleFor(x=>x.Password).NotEmpty().MinimumLength(8);

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var emailCheck = dbContext.Users.Any(u => u.Email == value);
                if (emailCheck)
                {
                    context.AddFailure("Email", "email jest uzywany");
                }
            });

        }


    }
}
