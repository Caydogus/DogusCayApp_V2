namespace DogusCay.WebUI.Services.TokenServices
{
    public interface ITokenService
    {
        // Token'ı Session'a kaydeden metot
        void SetUserToken(string token);

        // Session'dan token'ı okuyan metot (API istekleri için kullanılacak)
        string GetUserToken(); // Artık bir metot

        // Session'dan token'ı silen metot (Logout için)
        void ClearUserToken();

        // Aşağıdaki property'ler WebUI'nin kendi oturumundan (cookie'den) bilgileri almak için kullanılabilir.
        // API'ye gönderilecek token ile doğrudan ilgileri yoktur.
        int GetUserId { get; }
        string GetUserRole { get; }
        string GetUserNameSurname { get; }
    }
}