using System;
using System.Configuration;
using ConfigurationParser.Example.Settings.Databases;
using ConfigurationParser.Example.Settings.ExternalSystem;
using ConfigurationParser.Example.Settings.Smtp;

namespace ConfigurationParser.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ExternalSystemSettings externalSystemSettings = (dynamic)ConfigurationManager.GetSection("ExternalSystemSettings");
            Console.WriteLine("-==================== ExternalSystemSettings ====================-");
            Console.WriteLine("   AuthenticationSettings");
            Console.WriteLine("          Login: {0}", externalSystemSettings.AuthenticationSettings.Login);
            Console.WriteLine("          Password: {0}", externalSystemSettings.AuthenticationSettings.Password);
            Console.WriteLine("          Url: {0}", externalSystemSettings.AuthenticationSettings.Url);
            Console.WriteLine("   StaffSettings");
            Console.WriteLine("          Token: {0}", externalSystemSettings.StaffSettings.Token);
            Console.WriteLine("          Url: {0}", externalSystemSettings.StaffSettings.Url);
            Console.WriteLine();

            DatabasesSettings databasesSettings = (dynamic)ConfigurationManager.GetSection("DatabasesSettings");
            Console.WriteLine("-======================= DatabasesSettings ======================-");
            Console.WriteLine("   MongoDatabaseSettings");
            Console.WriteLine("          DatabaseName: {0}", databasesSettings.MongoSettings.DatabaseName);
            Console.WriteLine("          ConnectionString: {0}", databasesSettings.MongoSettings.ConnectionString);
            Console.WriteLine("   SqlSettings");
            foreach (var setting in databasesSettings.SqlSettings)
            {
                Console.WriteLine("          Tenant: {0}; Connection: {1}", setting.Key, setting.Value.ConnectionString);
            }
            Console.WriteLine();

            SmtpSettings smtpSettings = (dynamic)ConfigurationManager.GetSection("SmtpSettings");
            Console.WriteLine("-========================= SmtpSettings =========================-");
            Console.WriteLine("   Host: {0}", smtpSettings.Host);
            Console.WriteLine("   Sender: {0}", smtpSettings.Sender);
            Console.WriteLine("   Recipients");
            foreach (var setting in smtpSettings.Recipients)
            {
                Console.WriteLine("          Recipient: {0}", setting);
            }

            Console.WriteLine("-================================================================-");
            Console.ReadLine();
        }
    }
}
