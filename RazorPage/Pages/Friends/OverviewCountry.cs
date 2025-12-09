using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;
using Models.DTO;

namespace RazorPage.Pages.Friends;

    public class OverviewCountryModel : PageModel
    {
        private readonly IAddressesService _addressesService;
        private readonly IFriendsService _friendsService;
        private readonly IAdminService _adminService;
        public IEnumerable<GstUsrInfoFriendsDto>? CityInfo;
        public async Task<IActionResult> OnGet(string id)
        {
            var friends = await _friendsService.ReadFriendsAsync(true, false, null, 0, 100); 
            CityInfo = friends.PageItems
            .Where(f => f.Address?.Country == id)
            .Where(f => f.Address?.City != null)
            .GroupBy(f => f.Address.City)
            .Select(g => new GstUsrInfoFriendsDto
                {
                    City = g.Key,
                    NrFriends = g.Count()
                }).ToList();

            return Page();
    
        }
        public OverviewCountryModel(IAddressesService addressesService, IAdminService adminService, IFriendsService friendsService)
        {
            _addressesService = addressesService;
            _adminService = adminService;
            _friendsService = friendsService;
        }
     
    }

