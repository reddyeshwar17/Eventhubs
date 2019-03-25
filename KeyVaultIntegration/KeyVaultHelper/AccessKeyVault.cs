namespace KeyVaultHelperAgent
{
    using Microsoft.CloudInfrastructure.SecurityHelper;
    using System;

    public class AccessKeyVault
    {
        private KeyVaultHelper keyvault;

        public AccessKeyVault()
        {
            keyvault = new KeyVaultHelper();
        }

        public string GetSecretString(string KeyVaultKey)
        {
            string kValue = null;
            try {
                if (!string.IsNullOrWhiteSpace(KeyVaultKey))
                {
                    kValue = keyvault.GetSecretValue(KeyVaultKey).Result.ConvertToString();
                }
            }
            catch(Exception ex)
            {
                //Swallow temporarily.
            }

            return kValue;
        }
       
    }
}
