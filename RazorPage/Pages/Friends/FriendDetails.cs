using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace myFirstRazorPage.Pages
{
    //Demonstrate how to read Query parameters
    public class FriendDetails : PageModel
    {
        readonly ILogger<FriendDetails>? _logger = null;
        readonly IFriendsService? _service = null;
        readonly IPetsService? _petsService = null;

        public IFriend? Friend { get; set; }
        public string? ErrorMessage { get; set; } = null;

        public IActionResult OnGet(string id)
        {
            try
            {
                Guid _id = Guid.Parse(id);
                Friend =  _service?.ReadFriendAsync(_id, false).Result.Item ?? null;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return Page();
        }
        public IActionResult OnPostDelete(Guid id)
        {
            try
            {
                _petsService?.DeletePetAsync(id);
                return RedirectToPage("/Friends/Overview");
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return Page();
            }
        }

        //Inject services just like in WebApi
        public FriendDetails(IFriendsService service, IPetsService petsService, ILogger<FriendDetails> logger)
        {
            _logger = logger;
            _service = service;
            _petsService = petsService;
        }
    }
}
