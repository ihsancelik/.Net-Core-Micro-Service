namespace Library.Helpers.Constraints
{
    using static Project;
    using static Condition;
    public abstract class DbConstraints
    {
        public const string ApiCoreDbDevConnection = "";
        public const string ApiCoreDbDevRemoteConnection = "";
        public const string ApiCoreDbDevRemoteTestConnection = "";
        public const string ApiCoreDbConnection = "";


        public const string ApiDbDevConnection = "";
        public const string ApiDbDevRemoteConnection = "";
        public const string ApiDbDevRemoteTestConnection = "";
        public const string ApiDbConnection = "";

        public static string GetConnectionString(Project project, Condition condition)
        {
            string connectionString = string.Empty;
            if (project == Core)
            {
                if (condition == Development)
                    connectionString = ApiCoreDbDevConnection;
                if (condition == DevelopmentRemote)
                    connectionString = ApiCoreDbDevRemoteConnection;
                if (condition == DevelopmentRemoteTest)
                    connectionString = ApiCoreDbDevRemoteTestConnection;
                if (condition == Release)
                    connectionString = ApiCoreDbConnection;
            }
            else if (project == General)
            {
                if (condition == Development)
                    connectionString = ApiDbDevConnection;
                if (condition == DevelopmentRemote)
                    connectionString = ApiDbDevRemoteConnection;
                if (condition == DevelopmentRemoteTest)
                    connectionString = ApiDbDevRemoteTestConnection;
                if (condition == Release)
                    connectionString = ApiDbConnection;
            }

            return connectionString;
        }
    }

    public enum Project
    {
        Core,
        General
    }
    public enum Condition
    {
        Development,
        DevelopmentRemote,
        DevelopmentRemoteTest,
        Release
    }
}
