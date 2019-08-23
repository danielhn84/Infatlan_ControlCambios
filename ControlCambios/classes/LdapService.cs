using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ControlCambios.classes
{
    public class LdapService
    {
        public LdapService(){ }

        
        public bool ValidateCredentials(string domain, string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                return context.ValidateCredentials(username, password);
            }
        }

        public bool IsUserInAdGroup(string domain, string username, string adGroupName)
        {
            bool result = false;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    var group = GroupPrincipal.FindByIdentity(context, adGroupName);
                    if (group != null && user.IsMemberOf(group))
                        result = true;
                }
            }
            return result;
        }

        public String[] GetGroups(String username, String password)
        {
            String vDomain = Domain.GetCurrentDomain().Name;
            var allRoles = new List<string>();
            var root = new DirectoryEntry(vDomain, username,password);
            var searcher = new DirectorySearcher(root,string.Format(CultureInfo.InvariantCulture, "(&(objectClass=user)({0}={1}))","samAccountName",username));

            searcher.PropertiesToLoad.Add("memberOf");
            SearchResult result = searcher.FindOne();
            if (result != null && !string.IsNullOrEmpty(result.Path))
            {
                DirectoryEntry user = result.GetDirectoryEntry();
                PropertyValueCollection groups = user.Properties["memberOf"];
                foreach (string path in groups)
                {
                    string[] parts = path.Split(',');
                    if (parts.Length > 0)
                    {
                        foreach (string part in parts)
                        {
                            string[] p = part.Split('=');
                            if (p[0].Equals("cn", StringComparison.OrdinalIgnoreCase))
                            {
                                allRoles.Add(p[1]);
                            }
                        }
                    }
                }
            }
            return allRoles.ToArray();
        }

    }
}