using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;
using Models.DTO;

namespace RazorPage.Pages.Friends;

    public class OverviewModel : PageModel
    {
        private readonly IAddressesService _addressesService;
        private readonly IAdminService _adminService;
        public List<IFriend> Friends { get; set; } = new List<IFriend>();
        
        public async Task<IActionResult> OnGet()
        {
            var addresses = await _addressesService.ReadAddressesAsync(true, false, "Sweden", 0, 100); 
            var info = await _adminService.GuestInfoAsync();
            return Page();
        }
     
        public OverviewModel(IAddressesService addressesService, IAdminService adminService)
        {
            _addressesService = addressesService;
            _adminService = adminService;
        }
     
    }

