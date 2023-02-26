namespace Library.Routes
{
    public static class ApiCoreRoutes
    {
        public static class AccountRoutes
        {
            public const string ChangePassword = "changepass";
            public const string ForgotPassword = "forgotpassword";
            public const string Register = "register";
            public const string ResetPassword = "resetpass/{id}";
            public const string ResetPasswordOutSource = "resetpass";
        }

        public static class APIRoutes
        {
            public const string GetOfflineUsers = "getOfflineUsers";
            public const string GetOnlineUsers = "getOnlineUsers";
            public const string GetServerInfo = "getProducts";
            public const string LogoutUser = "logoutUser/{id:int}";
            public const string LogoutAllUsers = "logoutAllUsers";
            public const string StopServer = "stopServer";
        }

        public static class AuthenticateRoutes
        {
            public const string Authenticate = "authenticate";
            public const string AuthenticateByRefreshToken = "authenticatebyrefreshtoken";
            public const string RevokeAuthenticate = "revokeauthenticate";
            public const string RevokeAuthenticateByAdmin = "revokeauthenticatebyadmin";
        }
        public static class LoggingRoutes
        {
            public const string GetApiLogs = "getApiLogs";
        }
        public static class PlatformRoutes
        {
            public const string GetListByProductId = "getListByProductId/{productId:int}";
        }
        public static class PriorityRoutes
        {
            public const string GetPriorityByVersion = "getPriorityByVersion/{productId:int}/{versionInfoId:int}";
        }
        public static class ProductRoutes
        {
            public const string AddSetup = "addSetup/{platformId:int}/{productId:int}/{versionInfoId:int}";
            public const string AddModule = "addModule/{productId:int}/{moduleId:int}";
            public const string AddVersion = "addVersion/{productId:int}/{versionInfoId:int}/{priorityId:int}";
            public const string ExistSetup = "existSetup/{platformId:int}/{productId:int}/{versionInfoId:int}";
            public const string GetMiracleWorld = "getMiracleWorld/{platform}";
            public const string GetProducts = "getProducts";
            public const string RemoveModule = "removeModule/{productId:int}/{moduleId:int}";
            public const string RemoveVersion = "removeVersion/{productId:int}/{versionInfoId:int}";
        }
        public static class QTLogRoutes
        {
            public const string CreateUserMachineInfo = "createUserMachineInfo";
            public const string GetUserMachineInfo = "getUserMachineInfo";
        }
        public static class QTRoutes
        {
            public const string Download = "download/{productTag}/{platform}/{version}/{isPlugin}";
            public const string GetNotice = "getNotice";
            public const string GetProducts = "getProducts";
            public const string GetProductVersions = "getProductVersions/{productTag}";
            public const string GetUsageData = "getUsageData/{username}/{productTag}";
            public const string GetUserInfo = "getUserInfo/{username}";
            public const string SetUsageData = "setUsageData/{username}/{productTag}";
            public const string GetLatestVersion = "getLatestVersion/{productTag}/{platform}";
        }
        public static class RoleRoutes
        {
            public const string GetByUsername = "getByUsername/{username}";
        }
        public static class TestRoutes
        {
            public const string AuthTest = "AuthTest";
            public const string NonAuthTest = "NonAuthTest";
        }
        public static class UserRoutes
        {
            public const string AddProduct = "addProduct/{userId:int}/{productId:int}";
            public const string AddModule = "addModule/{userId:int}/{productId:int}/{moduleId:int}/{isActive}";
            public const string AddVersion = "addVersion/{userId:int}/{productId:int}";
            public const string GetOutSource = "getoutsource";
            public const string GetProducts = "getProducts/{userId:int}";
            public const string GetProductVersions = "getProductVersions/{userId:int}/{productId:int}";
            public const string GetProductModules = "getProductModules/{userId:int}/{productId:int}";
            public const string GetProductLimitation = "getProductLimitation/{userId:int}/{productId:int}";
            public const string GetProfileImage = "getProfileImage/{id:int}";
            public const string GetProfileImageOutSource = "getProfileImageOutSource";
            public const string RemoveProduct = "removeProduct/{userId:int}/{productId:int}";
            public const string RemoveModule = "removeModule/{userId:int}/{productId:int}/{moduleId:int}";
            public const string UpdateOutSource = "updateoutsource";
            public const string AddProductOutSource = "addProductOutSource/{userId:int}/{tag}/{versionId:int}";

        }

        public static class ProductModuleRoutes
        {
            public const string GetListByProductId = "getListByProductId/{productId:int}";
        }

        public static class VersionInfoRoutes
        {
            public const string GetVersionListByProductId = "getVersionListByProductId/{productId:int}";
            public const string GetVersionListByUserProduct = "getVersionListByUserProduct/{productId:int}";
            public const string GetByIdOutSource = "getByIdOutSource/{id:int}";
            public const string GetListOutSource = "getListOutSource";
        }

        public static class PingRoutes
        {
            public const string PingUnAuthorize = "pingUnauthorize";
            public const string PingAuthorize = "pingAuthorize";
            public const string PingOnline = "pingOnline";
            public const string PingOffline = "pingOffline";
        }

        public static class UserWatchRoutes
        {
            public const string GetOnlineUsers = "getOnlineUsers";
            public const string GetOnlineUser = "getOnlineUser/{userId:int}";
        }

        public static class ServerInfoRoutes
        {
            public const string GetInfo = "getInfo";
            public const string GetDependencyExceptions = "getDependencyExceptions/{libName}";
        }

        public static class TicketRoutes
        {
            public const string GetUserInfo = "getUserInfo/{userId:int}";
        }
    }
}