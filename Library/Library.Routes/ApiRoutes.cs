namespace Library.Routes
{
    public static class ApiRoutes
    {
        public static class AboutRoutes
        {
            public const string Get = "get";
            public const string GetAbout = "getAbout";
            public const string GetImage = "getImage";
        }

        public static class FeedBackRoutes
        {
            public const string GetOptions = "getOptions";
        }

        public static class LiveTicketRoutes
        {
            public const string GetContents = "getContents/{roomName}";
            public const string GetChats = "getChats/{roomName}";
            public const string GetCustomerName = "getCustomerName/{roomName}";
            public const string IsConnectedChange = "isConnectedChange/{roomName}";
        }
        public static class MarketRoutes
        {
            public const string PurchaseOrder = "purchase";
            public const string ReadyToPurchase = "readyToPurchase";
            public const string GetCurrency = "getCurrency";
        }
        public static class NewsRoutes
        {
            public const string GetImage = "getImage/{id:int}";
            public const string GetByTag = "getByTag/{tag}";
            public const string NewsList = "getAll/{tag}";
        }
        public static class ProductRoutes
        {
            public const string GetImage = "getImage/{id:int}";
            public const string GetByTag = "getByTag/{tag}";
            public const string GetProducts = "getProducts";
            public const string AddProductDetail = "addProductDetail/{productId:int}";
            public const string RemoveProductDetail = "removeProductDetail/{productId:int}/{productDetailId:int}";
        }
        public static class SliderRoutes
        {
            public const string GetImage = "getImage/{id:int}";
        }
        public static class TicketRoutes
        {
            public const string GetImage = "getImage/{messageId:int}";
            public const string IsClosedChange = "isClosedChange/{groupId:int}";
            public const string GetGroupReadInfo = "getGroupReadInfo/{groupId:int}";
            public const string GetGroupCloseInfo = "getGroupCloseInfo/{groupId:int}";
            public const string GetUserInfo = "getUserInfo/{userId:int}";

            public const string GetMessageGroups = "getMessageGroups";
            public const string GetTicketMessages = "getTicketMessages/{groupId:int}";
        }
    }
}