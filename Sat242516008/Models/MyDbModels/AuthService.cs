namespace Services;

using MyDbModels;

public class AuthService
{
    // Giriş yapan kullanıcıyı burada tutuyoruz
    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public void Login(User user)
    {
        CurrentUser = user;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}