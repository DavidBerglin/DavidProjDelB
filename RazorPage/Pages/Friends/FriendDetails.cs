using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Models.Interfaces;
using Services.Interfaces;

namespace myFirstRazorPage.Pages
{
    //Demonstrate how to read Query parameters
    public class FriendDetails : PageModel
    {
        readonly IFriendsService _service;
        readonly IAddressesService _addressService;
        readonly IPetsService _petsService;

        public IFriend? Friend { get; set; }
      
        [BindProperty]
        public AddressIM AddressIM { get; set; } = new ();
        public bool EditAddress {get;set;}

        public async Task<IActionResult> OnGet(Guid id, bool edit = false)
        {   
            // hämta vännen först
            var result = await _service.ReadFriendAsync(id, false);
            Friend = result?.Item;
            // stoppar direkt om ingen vän hittas
            if (Friend == null) return Page();
            // sätta edit mode för adressen
            EditAddress = edit;

            // Tenary operator, beroende på om vännen har en address eller inte så fylls fälten i formuläret
            AddressIM = Friend.Address != null 
            ? new AddressIM(Friend.Address, Friend.FriendId) 
            : new AddressIM() { FriendId = Friend.FriendId };
            
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(Guid id, Guid petId)
        {   
            // ta bort husdjuret och sen ladda om sidan på nytt
            await _petsService.DeletePetAsync(petId);
            return RedirectToPage("./FriendDetails", new { id = id });
        }
        public async Task<IActionResult> OnPostEdit()
        {
            var existingAddress = await _addressService.ReadAddressAsync(AddressIM.AddressId, false);
            var friendIds = existingAddress?.Item?.Friends?.Select(f => f.FriendId).ToList();
            
            await _addressService.UpdateAddressAsync(new AddressCuDto
            {
                AddressId = AddressIM.AddressId,
                StreetAddress = AddressIM.StreetAddress,
                City = AddressIM.City,
                ZipCode = AddressIM.ZipCode,
                Country = AddressIM.Country,
                FriendsId = friendIds
            });
    
            
            return RedirectToPage("./FriendDetails", new { id = AddressIM.FriendId });
        }
        

        //Inject services just like in WebApi
        public FriendDetails(IFriendsService service, IAddressesService addressService, IPetsService petsService)
        {
            _service = service;
            _addressService = addressService;
            _petsService = petsService;
        }
    }
    
    public class AddressIM
    {
        public Guid AddressId { get; set; }
        public Guid FriendId { get; set; }
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string Country { get; set; } = string.Empty;

        public AddressIM() { }

        public AddressIM(IAddress address, Guid friendId)
        {
            AddressId = address.AddressId;
            StreetAddress = address.StreetAddress;
            City = address.City;
            ZipCode = address.ZipCode;
            Country = address.Country;
            FriendId = friendId;
        }
       
    }
}
