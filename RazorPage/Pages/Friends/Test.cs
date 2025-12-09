using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;
using Models.DTO;

namespace RazorPage.Pages.Friends;

public class Test : PageModel
{
    private readonly IAddressesService _addressesService;
    private readonly IFriendsService _friendsService;
    private readonly IAdminService _adminService;
    public List<IFriend> Friends { get; set; } = new List<IFriend>();

    public async Task<IActionResult> OnGet(string searchString)
    {
        var friendresponse = await _friendsService.ReadFriendsAsync(true, false, null, 0, 100);
        if (friendresponse?.PageItems != null)
        {
            Friends = string.IsNullOrEmpty(searchString)
                ? friendresponse.PageItems.ToList()
                : friendresponse.PageItems.Where(f => f.Address?.City?.ToLower() == searchString.ToLower()).ToList();
        }
        return Page();

    }
    public Test(IAddressesService addressesService, IAdminService adminService, IFriendsService friendsService)
    {
        _addressesService = addressesService;
        _adminService = adminService;
        _friendsService = friendsService;
    }

}

