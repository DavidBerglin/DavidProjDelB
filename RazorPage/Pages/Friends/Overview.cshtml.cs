using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;
using Models.DTO;

namespace RazorPage.Pages.Friends;

public class OverviewModel : PageModel
{
    private readonly IAddressesService _addressesService;
    private readonly IFriendsService _friendsService;
    private readonly IAdminService _adminService;
    public List<IFriend> Friends { get; set; } = new List<IFriend>();

     //Pagination
    public int NrOfPages { get; set; }
    public int PageSize { get; } = 5;

    public int ThisPageNr { get; set; } = 0;
    public int PrevPageNr { get; set; } = 0;
    public int NextPageNr { get; set; } = 0;
    public int PresentPages { get; set; } = 0;

    public async Task<IActionResult> OnGet(string searchString)
    {
        var friendresponse = await _friendsService.ReadFriendsAsync(true, false, null, 0, 100);
        if (friendresponse?.PageItems != null)
        {
            Friends = string.IsNullOrEmpty(searchString)
                ? friendresponse.PageItems.ToList()
                : friendresponse.PageItems
                .Where(f => f.Address?.City?.ToLower().Contains(searchString.ToLower()) == true 
                || 
                f.Address?.Country?.ToLower().Contains(searchString.ToLower()) == true)
                .ToList();
        }
        return Page();

    }
    public OverviewModel(IAddressesService addressesService, IAdminService adminService, IFriendsService friendsService)
    {
        _addressesService = addressesService;
        _adminService = adminService;
        _friendsService = friendsService;
    }
}