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
using System.Data;

namespace ControlCambios.classes
{
    public class LdapService
    {
        public LdapService(){ }

        
        public bool ValidateCredentials(string domain, string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                DataTable vDatosAD = new DataTable();
                try
                {
                    DirectorySearcher search = new DirectorySearcher(context.Name);
                    search.Filter = "(&(objectClass=user)(anr=" + username + "))";
                    search.PropertiesToLoad.Add("givenName");
                    search.PropertiesToLoad.Add("sn");
                    search.PropertiesToLoad.Add("mail");
                    SearchResult result = search.FindOne();

                    vDatosAD.Columns.Add("givenName");
                    vDatosAD.Columns.Add("sn");
                    vDatosAD.Columns.Add("mail");
                    vDatosAD.Rows.Add(
                        result.Properties["givenName"][0].ToString(),
                        result.Properties["sn"][0].ToString(),
                        result.Properties["mail"][0].ToString()
                    );
                }
                catch {}

                return context.ValidateCredentials(username, password);
            }
        }

        public DataTable GetDatosUsuario(string domain, string username)
        {
            DataTable vDatosAD = new DataTable();
            try
            {
                DirectorySearcher search = new DirectorySearcher(domain);
                search.Filter = "(&(objectClass=user)(anr=" + username + "))";
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("mail");
                SearchResult result = search.FindOne();

                vDatosAD.Columns.Add("givenName");
                vDatosAD.Columns.Add("sn");
                vDatosAD.Columns.Add("mail");

                if (result != null)
                {
                    vDatosAD.Rows.Add(
                        result.Properties["givenName"][0].ToString(),
                        result.Properties["sn"][0].ToString(),
                        result.Properties["mail"][0].ToString()
                    );
                }
            }
            catch
            {
                throw;
            }
            return vDatosAD;
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