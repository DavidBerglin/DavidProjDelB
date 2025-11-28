using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seido.Utilities.SeedGenerator;

namespace RazorPage.Pages;

public class SeedModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly SeedGenerator _seeder;
    public string FirstName {get;set;}
    

    public SeedModel(ILogger<PrivacyModel> logger, SeedGenerator seeder)
    {
        _logger = logger;
        _seeder = seeder;
    }

    public void OnGet()
    {
        FirstName = _seeder.FirstName;
    }
}

