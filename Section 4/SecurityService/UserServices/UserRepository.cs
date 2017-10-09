using System.Collections.Generic;
using System.Linq;
using System;

namespace SecurityService.UserServices
{
    public class UserRepository : IUserRepository
    {
        private readonly List<CustomUser> _users = new List<CustomUser>
        {
            new CustomUser{
                SubjectId = "123",
                UserName = "postmanadmin",
                Password = "postmanadmin",
                Email = "postmanadmin@email.ch"
            },
             new CustomUser{
                SubjectId = "456",
                UserName = "postmanuser",
                Password = "postmanuser",
                Email = "postmanuser@email.ch"
            }
        };

        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }

            return false;
        }

        public CustomUser FindBySubjectId(string subjectId)
        {
            return _users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        public CustomUser FindByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
