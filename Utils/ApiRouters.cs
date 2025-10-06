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

        public const string GOOGLE_LOGIN = "/api/v1/auth/google-login";
        public const string FACEBOOK_LOGIN = "/api/v1/auth/facebook-login";
        public const string REGISTER = "/api/v1/auth/register";
        public const string REFRESH = "/api/v1/auth/refresh";
        public const string LOGOUT = "/api/v1/auth/logout";
        public const string INTROSPECT = "/api/v1/auth/introspect";
        public const string CHANGE_PASSWORD = "/api/v1/auth/change-password";
        public const string FORGOT_PASSWORD = "/api/v1/auth/forgot-password";
        
    }
}
