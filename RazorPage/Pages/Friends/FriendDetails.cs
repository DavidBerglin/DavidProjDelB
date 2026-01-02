using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Models.Interfaces;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace myFirstRazorPage.Pages
{
    //Demonstrate how to read Query parameters
    public class FriendDetails : PageModel
    {
        readonly IFriendsService _service;
        readonly IAddressesService _addressService;
        readonly IPetsService _petsService;
        readonly IQuotesService _quotesService;

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
        public async Task<IActionResult> OnPostDelete(Guid friendId, Guid petId, Guid quoteId)
        {   
            // Kontrollera vilket ID som är giltigt och ta bort antingen pet eller quote
            if (petId != Guid.Empty)
            {
                // Ta bort husdjuret
                await _petsService.DeletePetAsync(petId);
            }
            else if (quoteId != Guid.Empty)
            {
                // Ta bort citatet
                await _quotesService.DeleteQuoteAsync(quoteId);
            }
            
            return RedirectToPage("./FriendDetails", new { id = friendId });
        }
        
        public async Task<IActionResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                EditAddress = true;
                var result = await _service.ReadFriendAsync(AddressIM.FriendId, false);
                Friend = result?.Item;
                return Page();
            }
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
        public FriendDetails(IFriendsService service, IAddressesService addressService, IPetsService petsService, IQuotesService quotesService)
        {
            _service = service;
            _addressService = addressService;
            _petsService = petsService;
            _quotesService = quotesService;
        }
    }
    
    public class AddressIM
    {
        public Guid AddressId { get; set; }
        public Guid FriendId { get; set; }
        [Required]
        [StringLength(100)]
        public string StreetAddress { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
         [Required]
        [Range(10000, 99999)]
        public int ZipCode { get; set; }
         [Required]
        [StringLength(100)]
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
