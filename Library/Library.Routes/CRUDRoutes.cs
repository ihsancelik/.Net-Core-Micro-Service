namespace Library.Routes
{
    public static class CRUDRoutes
    {
        public const string ListAll = "listAll";
        public const string List = "list";
        public const string Create = "create";
        public const string Update = "update/{id:int}";
        public const string Delete = "delete/{id:int}";
        public const string GetById = "get/{id:int}";
        public const string Count = "count";
    }
}