using System.Collections.Generic;

namespace DotNetCraft.ConfigurationParser.Example.Settings.Databases
{
    class DatabasesSettings
    {
        public MongoDatabaseSettings MongoSettings { get; set; }
        public Dictionary<string, SqlSettings> SqlSettings { get; set; }
    }
}
