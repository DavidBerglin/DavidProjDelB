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
            var addresses = await _addressesService.ReadAddressesAsync(true, false, null, 0, 100); 
            var info = await _adminService.GuestInfoAsync();
            Countryinfo = info.Item.Friends.GroupBy(f => f.Country)
                .Select(g => new GstUsrInfoFriendsDto
                {
                    Country = g.Key,
                    NrFriends = g.Sum(x => x.NrFriends)
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

