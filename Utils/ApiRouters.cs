namespace RestaurantManagementSystem.Utils
{
    public static class ApiRoutes
    {
        // User routes
        public const string CREATE_USER = "/api/v1/users/create";
        public const string GET_USERS = "/api/v1/users";
        public const string GET_USER = "/api/v1/users/{id}";
        public const string UPDATE_USER = "/api/v1/users/update/{id}";
        public const string DELETE_USER = "/api/v1/users/delete/{id}";

        // Auth routes
        public const string LOGIN = "/api/v1/auth/login";
        public const string REGISTER = "/api/v1/auth/register";
        public const string REFRESH = "/api/v1/auth/refresh";
        public const string LOGOUT = "/api/v1/auth/logout";
        public const string CHANGE_PASSWORD = "/api/v1/auth/change-password";
        public const string FORGOT_PASSWORD = "/api/v1/auth/forgot-password";


        // Category routes
        public const string CREATE_CATEGORY = "/api/v1/categories";
        public const string GET_CATEGORIES = "/api/v1/categories";
        public const string GET_CATEGORY = "/api/v1/categories/{id}";
        public const string UPDATE_CATEGORY = "/api/v1/categories/update/{id}";
        public const string DELETE_CATEGORY = "/api/v1/categories/delete/{id}";


        // Menu Item routes
        public const string CREATE_MENU_ITEM= "/api/v1/menuitems";
        public const string GET_MENU_ITEMS = "/api/v1/menuitems";
        public const string GET_MENU_ITEM = "/api/v1/menuitems/{id}";
        public const string UPDATE_MENU_ITEM = "/api/v1/menuitems/update/{id}";
        public const string DELETE_MENU_ITEM = "/api/v1/menuitems/delete/{id}";

        // File upload routes
        public const string UPLOAD_MENU_ITEM_IMAGES = "api/v1/menuitems/{id}/upload-image";
        public const string UPLOAD_CATEGORY_IMAGES = "api/v1/categories/{id}/upload-image";
        public const string DELETE_IMAGE = "api/v1/{fileId}";

    }
}
