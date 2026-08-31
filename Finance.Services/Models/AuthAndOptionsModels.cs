namespace Finance.Services.Models;

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class TokenModel
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Username { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
}

public class PayrollOptions
{
    public int PayPeriodsPerYear { get; set; } = 26;
    public decimal TaxRate { get; set; } = 0.22m;
    public decimal OvertimeMultiplier { get; set; } = 1.5m;
    public decimal DefaultOtherDeductions { get; set; } = 0m;
}
