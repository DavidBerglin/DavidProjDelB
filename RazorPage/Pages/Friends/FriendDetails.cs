using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace myFirstRazorPage.Pages
{
    //Demonstrate how to read Query parameters
    public class FriendDetails : PageModel
    {
        //Just like for WebApi
        readonly ILogger<FriendDetails>? _logger = null;
        readonly IFriendsService? _service = null;

        //public member becomes part of the Model in the Razor page
        public IFriend? Friend { get; set; }
        public string ErrorMessage { get; set; } = null;

        //Will execute on a Get request
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

        //Inject services just like in WebApi
        public FriendDetails(IFriendsService service, ILogger<FriendDetails> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
