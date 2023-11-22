using FluentValidation;
using TestNetAPI.Entities;

namespace TestNetAPI.Models.Validators
{
    public class CreateDetailDtoValidator : AbstractValidator<CreateDetailDto>
    {
        public CreateDetailDtoValidator(NetDbContext dbContext)
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email jest wymagany.")
               .EmailAddress().WithMessage("Nieprawidłowy format adresu e-mail.")
               .Must(email => IsEmailUnique(email, dbContext))
                   .WithMessage("Adres e-mail jest już używany.");

          // sprawdza czy email istnieje w bazie jeśli tak to uzytkownik dostaje błąd
          // jeśli nie istnieje to konto zostaje założone jeśli hasło spełnia wymmagania  

        }

        private bool IsEmailUnique(string email, NetDbContext dbContext)
        {
            return !dbContext.Users.Any(u=>u.Email == email);
        }
    }
}
