using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SecurityService.UserServices
{
    public class CustomProfileService : IProfileService
    {
        protected readonly ILogger Logger;


        protected readonly IUserRepository _userRepository;

        public CustomProfileService(IUserRepository userRepository, ILogger<CustomProfileService> logger)
        {
            _userRepository = userRepository;
            Logger = logger;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            Logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            CustomUser user = _userRepository.FindBySubjectId(context.Subject.GetSubjectId());

            var claims = new List<Claim>
            {
                new Claim("role", "resources.user"),
                new Claim("username", user.UserName),
                new Claim("email", user.Email)
            };

            if (user.UserName == "postmanadmin")
            {
                claims.Add(new Claim("role", "resources.admin"));
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userRepository.FindBySubjectId(context.Subject.GetSubjectId());
            context.IsActive = user != null;
        }
    }
}