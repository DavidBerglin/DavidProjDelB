using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace RazorPage.Pages.Friends;

    public class OverviewModel : PageModel
    {
        private readonly IAddressesService _addressesService;
        public List<IFriend> Friends { get; set; } = new List<IFriend>();
        public List<IAddress> Addresses { get; set; } = new List<IAddress>();
        
        public async Task<IActionResult> OnGet()
        {
            var addresses = await _addressesService.ReadAddressesAsync(true, false, "Denmark",0, 100);
            return Page();
        }
        public OverviewModel(IAddressesService addressesService)
        {
            _addressesService = addressesService;
        }
     
    }

