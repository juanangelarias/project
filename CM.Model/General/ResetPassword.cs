namespace CM.Model.General;

public class ResetPassword: LoginRequest
{
    public string? NewPassword { get; set; }
}