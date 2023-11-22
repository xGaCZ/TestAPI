using FluentValidation;
using TestNetAPI.Entities;

namespace TestNetAPI.Models.Validators
{
    public class UpdateDetailDtoValidator : AbstractValidator<UpdateDetailDto>
    {
        public UpdateDetailDtoValidator(NetDbContext dbContext) 
        {
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
