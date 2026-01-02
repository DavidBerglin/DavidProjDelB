using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;
using Models.DTO;

namespace RazorPage.Pages.Friends;

    public class FriendsByCountryModel : PageModel
    {
        private readonly IAddressesService _addressesService;
        private readonly IFriendsService _friendsService;
        private readonly IAdminService _adminService;
        public IEnumerable<GstUsrInfoFriendsDto>? Countryinfo;
        public async Task<IActionResult> OnGet()
        {
            var friends = await _friendsService.ReadFriendsAsync(true, false, null, 0, 1000); 
            Countryinfo = friends.PageItems
            .Where(f => f.Address?.Country != null)
            .GroupBy(f => f.Address.Country)
            .Select(g => new GstUsrInfoFriendsDto
                {
                    Country = g.Key,
                    NrFriends = g.Count()
                }).ToList();

            return Page();
    
        }
        public FriendsByCountryModel(IAddressesService addressesService, IAdminService adminService, IFriendsService friendsService)
        {
            _addressesService = addressesService;
            _adminService = adminService;
            _friendsService = friendsService;
        }
     
    }

