using System;
using System.IO;

namespace KeyvaultInt
{
    using System;
    using KeyVaultHelperAgent;
    using Microsoft.Azure.KeyVault;    //using Microsoft.CloudInfrastructure.SecurityHelper;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Configuration;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Management;
    class Program
    {
        //public static KeyVaultHelper keyvault;
        public static AccessKeyVault keyhelper;          
        static void Main(string[] args)
        {
            string environment = null, installerArgs1 = null, installerArgs2 = null;
            int argsLength = args.Length;
            if (argsLength < 3)
            {
                Console.WriteLine("Insufficient arguements passed");
                Console.ReadLine();
                return;
            }

            else
            {
                environment = args[0];
                installerArgs1 = args[1];
                installerArgs2 = args[2];

            }
            //
            var keyClient = new KeyVaultClient((authority, resource, scope) =>
            {
                var authenticationContext = new AuthenticationContext(authority, null);
                X509Certificate2 certificate;
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, "0910BE605954E8E055DDAAFC90B22E431294F170", false);
                    if (certificateCollection == null || certificateCollection.Count == 0)
                    {
                        throw new Exception("Certificate not installed in the store");
                    }

                    certificate = certificateCollection[0];
                }
                finally
                {
                    store.Close();
                }

                var clientAssertionCertificate = new ClientAssertionCertificate(ConfigurationManager.AppSettings["KeyVault.Authentication.ClientId"], certificate);
                return GetAccessToken(authority, resource, scope, clientAssertionCertificate);

            });
       
            var key = "";

            if (environment.ToLower().Equals("test"))
            {

                key = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "testwebADpwd").GetAwaiter().GetResult().Value;

            }
            else if (environment.ToLower().Equals("dev"))
            {
                key = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "testdADpwd").GetAwaiter().GetResult().Value;

            }
            else if (environment.ToLower().Equals("prod"))
            {

                key = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "testpADpwd").GetAwaiter().GetResult().Value;
            }

            ReplacePwd(key);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            RunInstaller(installerArgs1, installerArgs2);
            MoveOneBoxConfigBack(key);

            //var key1 = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "scmtwebADpwd").GetAwaiter().GetResult().Value;
            //var key3 = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "esbpwebADpwd").GetAwaiter().GetResult().Value;


            //var key2 = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "scmdwebADpwd").GetAwaiter().GetResult().Value;

            //var key4 = keyClient.GetSecretAsync(ConfigurationManager.AppSettings["KeyVault.Vault.Address"], "AxScmServiceADAppEncryptedClientSecret").GetAwaiter().GetResult().Value;
            //Console.WriteLine("The values are : " + key0 + ", " + key2 + ", " + key3 + ", " + key4);
            //Console.ReadLine();

        }
        public static void RunInstaller(string installerArgs1, string installerArgs2)
        {

            try
            {

                System.Diagnostics.ProcessStartInfo procStartInfo =
                  new System.Diagnostics.ProcessStartInfo();

                procStartInfo.Arguments = installerArgs1 + " " + installerArgs2;

                procStartInfo.FileName = "ScmInstaller.exe";
                procStartInfo.RedirectStandardOutput = true;
                // procStartInfo.FileName = @"\\test\D$\ecm";
                procStartInfo.UseShellExecute = false;               
                procStartInfo.CreateNoWindow = true;
                procStartInfo.Verb = "runas";
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(result);

            }
            catch (Exception objException)
            {
                // Log the exception
            }


        }

        public static void ReplacePwd(string key)
        {
            string text = null;

            text = Directory.GetCurrentDirectory() + "\\Config.txt";
            Console.WriteLine(text);
            text = File.ReadAllText((Directory.GetCurrentDirectory().ToString() + "\\Config.txt"));
            text = text.Replace("PasswordPlaceHolder", key);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Config.txt", text);

        }

        public static void MoveOneBoxConfigBack(string key)
        {
            string text = null;
            text = Directory.GetCurrentDirectory() + "\\Config.txt";
            Console.WriteLine(text);
            text = File.ReadAllText((Directory.GetCurrentDirectory().ToString() + "\\Config.txt"));
            text = text.Replace(key, "PasswordPlaceHolder");
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Config.txt", text);
        }

        /**
        public static string GetAADToken(string resourceName, string clientSecretKey)
        {


            //Encrypted client guid for the AD App AxScmService
            string clientId = "";


            //token for AxScmService AD App for connecting endpoints
            string appToken = "";

            //Login Authority for ADAL
            string authorityUri = "https://login.windows.net/{0}";

            //Resource for which AAD token has to be acquired
            string resourceUri = "http://local.microsoft.onmicrosoft.com/pricing";

            string authority = string.Format(authorityUri, appToken);


            //Encrypted Secret key for the AxScmService Ad App
           // string clientSecretKey = "AxScmServiceADAppEncryptedClientSecret";

            string clientSecret = "";
            //keyvault.GetSecretString(clientSecretKey);



            AuthenticationResult _currentToken = null;


            var authContext = new AuthenticationContext(authority);
            _currentToken = GetAccessToken(authority, resourceUri, string.Empty, new ClientCredential(clientId, clientSecret)).GetAwaiter().GetResult();

            return _currentToken.AccessToken;
        }
    
    **/
        private static async Task<string> GetAccessToken(string authority, string resource, string scope, ClientAssertionCertificate clientCredential)
        {
            var context = new AuthenticationContext(authority, null);
            var result = await context.AcquireTokenAsync(resource, clientCredential);
            return result.AccessToken;
        }


        private static async Task<AuthenticationResult> GetAccessToken(string authority, string resource, string scope, ClientCredential clientCredential)
        {
            var context = new AuthenticationContext(authority, null);
            var result = await context.AcquireTokenAsync(resource, clientCredential);
            return result;
        }

        /// <summary>
        /// Generates the authentication callback which uses certificates for access.
        /// </summary>
        public static KeyVaultClient.AuthenticationCallback CreateAccessTokenLookupByCertificate(
            string clientId,
            Func<X509Certificate2> certificateLookup)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentException($"Invalid value for clientId: [{clientId}]");
            }

            return async (authority, resource, scope) =>
            {
                var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
                var certificate = certificateLookup();
                if (certificate == null)
                {
                    throw new InvalidOperationException("A certificate was not found for communicating with KeyVault.");
                }

                var clientAssertionCertificate = new ClientAssertionCertificate(clientId, certificate);
                var authenticationResult = await context.AcquireTokenAsync(resource, clientAssertionCertificate);
                if (authenticationResult == null)
                {
                    throw new InvalidOperationException(
                        $"Could not acquire access token for Key Vault client ID '{clientId}' and certificate thumbprint {certificate.Thumbprint}.");
                }

                return authenticationResult.AccessToken;
            };
        }

        static void Main1(string[] args)
        {
            keyhelper = new AccessKeyVault();
            var key1 = keyhelper.GetSecretString("testtADpwd");
            var key2 = keyhelper.GetSecretString("testdADpwd");
            var key3 = keyhelper.GetSecretString("testpADpwd");
            var key4 = keyhelper.GetSecretString("test");
            Console.WriteLine("The values are : " + key1 + ", " + key2 + ", " + key3 + ", " + key4);
            Console.ReadLine();
        }


    }
}

