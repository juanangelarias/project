namespace CM.Model.General;

public class ChangePasswordRequest
{
    public long UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}