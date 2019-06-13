using System;

namespace Martelskiy.Api.Template.Features.Shared.Environment
{
    public class Environment
    {
        public Environment(string name, string shortName)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
        }

        public string Name { get; }
        public string ShortName { get; }

        public static readonly Environment Development = new Environment(EnvironmentName.Development, "Dev");
        public static readonly Environment Production = new Environment(EnvironmentName.Production, "Prod");
    }
}
