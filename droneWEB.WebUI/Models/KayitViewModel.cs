using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;

namespace droneWEB.WebUI.Models
{
    public class KayitViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur.")]
        public string Ad { get; set; } = null!;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        public string Soyad { get; set; } = null!;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
        public string Eposta { get; set; } = null!;

        [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 rakam olmalıdır.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No sadece rakamlardan oluşmalıdır.")]
        [CustomValidation(typeof(KayitViewModel), nameof(ValidateTcKimlikNo))]
        public string TcKimlikNo { get; set; } = null!;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Telefon 10 rakam olmalıdır.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon sadece rakamlardan oluşmalıdır.")]
        public string Telefon { get; set; } = null!;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [CustomValidation(typeof(KayitViewModel), nameof(ValidatePassword))]
        public string Sifre { get; set; } = null!;

        // TC Kimlik algoritması validasyonu
        public static ValidationResult? ValidateTcKimlikNo(string? tc, ValidationContext context)
        {
            if (tc == null) return new ValidationResult("TC Kimlik No boş olamaz.");

            if (!IsValidTc(tc))
                return new ValidationResult("Geçersiz TC Kimlik No.");

            return ValidationResult.Success;
        }

        private static bool IsValidTc(string tc)
        {
            if (tc.Length != 11 || !tc.All(char.IsDigit)) return false;
            if (tc.StartsWith("0")) return false;

            int[] digits = tc.Select(c => int.Parse(c.ToString())).ToArray();

            int sumOdd = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int sumEven = digits[1] + digits[3] + digits[5] + digits[7];

            int checkDigit10 = ((sumOdd * 7) - sumEven) % 10;
            int checkDigit11 = (digits.Take(10).Sum()) % 10;

            return digits[9] == checkDigit10 && digits[10] == checkDigit11;
        }

        // Şifre validasyonu
        public static ValidationResult? ValidatePassword(string? sifre, ValidationContext context)
        {
            if (string.IsNullOrEmpty(sifre))
                return new ValidationResult("Şifre boş olamaz.");

            if (sifre.Length < 8)
                return new ValidationResult("Şifre en az 8 karakter olmalıdır.");

            if (!Regex.IsMatch(sifre, "[A-Z]"))
                return new ValidationResult("Şifre en az 1 büyük harf içermelidir.");

            if (!Regex.IsMatch(sifre, "[a-z]"))
                return new ValidationResult("Şifre en az 1 küçük harf içermelidir.");

            if (!Regex.IsMatch(sifre, "[0-9]"))
                return new ValidationResult("Şifre en az 1 rakam içermelidir.");

            // Özel karakterler için tırnakları kaçırmadık, sadece köşeli parantez ve escape kullanıyoruz
            if (!Regex.IsMatch(sifre, @"[!@#$%^&*(),.?{}|<>_\-+=]"))
                return new ValidationResult("Şifre en az 1 özel karakter içermelidir.");

            return ValidationResult.Success;
        }
    }
}
