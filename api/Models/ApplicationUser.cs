using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string Address { get; set; }

    public string mobileNumber { get; set; }
}