namespace AppCore.Business.Configs
{
    public static class IdentityServiceConfig
    {
        public static string AdminRole => "Admin";
        public static string UserRole => "User";
        public static string OperationBy => "AppCore";
        public static string UserNotFoundMessage => "UserNotFound";
        public static string UsersNotFoundMessage => "UsersNotFound";
        public static string UserFoundMessage => "UserFound";
        public static string PasswordNotFoundMessage => "PasswordNotFound";
        public static string RoleNotFoundMessage => "RoleNotFound";
        public static string RolesNotFoundMessage => "RolesNotFound";
        public static string RoleFoundMessage => "RoleFound";
        public static string ClaimNotFoundMessage => "ClaimNotFound";
        public static string ClaimsNotFoundMessage => "ClaimsNotFound";
        public static string ClaimFoundMessage => "ClaimFound";
        public static string PasswordUpdatedMessage => "PasswordUpdated";
    }
}
