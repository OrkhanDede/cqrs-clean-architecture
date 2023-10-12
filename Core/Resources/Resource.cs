namespace Core.Resources
{
    public class Resource
    {
    }

    public static class ResourceKey
    {
        #region Validation keys

        public const string Required = "REQUIRED";
        public const string Range = "RANGE";
        public const string DataIsNotValidFormat = "DATA_IS_NOT_VALID_FORMAT";
        public const string AllowedFileFormat = "ALLOWED_FILE_FORMAT";
        public const string AllowedFileMimeType = "ALLOWED_FILE_MIME_TYPE";

        #endregion

        #region exception message keys

        public const string WrongRequest = "WRONG_REQUEST";
        public const string RecordNotFound = "RECORD_NOT_FOUND";
        public const string RecordNotEditable = "RECORD_NOT_EDITABLE";

        public const string RecordAlreadyExist = "RECORD_ALREADY_EXIST";
        public const string LockedUser = "LOCKED_USER";
        public const string InvalidSmsConfirmationCode = "INVALID_SMS_CONFIRMATION_CODE";
        public const string InvalidSmsConfirmationCodeResend = "INVALID_SMS_CONFIRMATION_CODE_RESEND";
        public const string InvalidSignIn = "INVALID_SIGN_IN";
        public const string InvalidRecaptchaToken = "INVALID_RECAPTCHA_TOKEN";
        public const string InternalServer = "INTERNAL_SERVER";
        public const string InsufficientTableAccess = "INSUFFICIENT_TABLE_ACCESS";
        public const string IdMustBeGuid = "ID_MUST_BE_GUID";
        public const string CorporateUserSignIn = "CORPORATE_USER_SIGN_IN";
        public const string UserNotFoundContactToAdmin = "USER_NOT_FOUND_CONTACT_TO_ADMIN";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string FolderNotFound = "FOLDER_NOT_FOUND";
        public const string ParentFolderNotFound = "PARENT_FOLDER_NOT_FOUND";
        public const string InvalidToken = "INVALID_TOKEN";
        public const string CanImpersonateYourself = "CANT_IMPERSONATE_YOURSELF";

        #endregion
    }
}
