using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Pages.Register
{

    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;

        

        [BindProperty]
        public InputModel Input { get; set; }

        public Index(
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityProviderStore identityProviderStore,
            IEventService events,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
            _events = events;
        }

        public IActionResult OnGet(string returnUrl)
        {
            BuildModelAsync(returnUrl);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // check if we are in the context of an authorization request
            
            var user = await _userManager.FindByNameAsync(Input.Username);
            if (user != null)
            {
                ModelState.AddModelError("Input.Username", "Duplicate username");
            }


            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new ApplicationUser()
                {
                    Email = Input.Email,
                    UserName=Input.Username
                    
                },Input.Password);

                
                if (result.Succeeded)
                {
                    return Redirect(Input.ReturnUrl ?? "~/");
                }
                
                ModelState.AddModelError(string.Empty, "Something went wrong...");
            }

            // something went wrong, show form with error
            BuildModelAsync(Input.ReturnUrl);
            return Page();
        }

        private void BuildModelAsync(string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };
        }
    }
}