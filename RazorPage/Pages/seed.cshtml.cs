using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seido.Utilities.SeedGenerator;
using Services.Interfaces;

namespace RazorPage.Pages;

public class SeedModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    readonly IAdminService _adminService;
    [BindProperty]
    public int NrOfItemsToSeed {get;set; } = 100;
    

    public SeedModel(ILogger<PrivacyModel> logger, IAdminService adminService)
    {
        _logger = logger;
        _adminService = adminService;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task <IActionResult> OnPost()
    {
        await _adminService.SeedAsync(NrOfItemsToSeed);
        return Page();
    }
}

