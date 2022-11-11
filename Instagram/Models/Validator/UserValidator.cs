using FluentValidation;

namespace Instagram.Models.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Ad boş ola bilməz!");
            RuleFor(x => x.UserSurname).NotEmpty().WithMessage("Soyad boş ola bilməz!");
            RuleFor(x => x.UserNickName).NotEmpty().WithMessage("Login hissəsi boş ola bilməz");
            RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Şifrə boş ola bilməz!");

        }
    }
}
