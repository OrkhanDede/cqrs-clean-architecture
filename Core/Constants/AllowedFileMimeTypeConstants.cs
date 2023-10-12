namespace Core.Constants
{
    public static class AllowedFileMimeTypeConstants
    {
        public static string[] MimeTypes = new string[]
        {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.ms-excel",
            "application/vnd.ms-powerpoint",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "video/mp4",
            "application/mp4",
            "audio/mpeg"

        };
    }

    public static class AllowedFileExtension
    {
        public static string[] Extensions = new string[]
        {
            ".pdf",
            ".doc",
            ".docx",
            ".xlsx",
            ".xls",
            ".ppt",
            ".pptx",
            ".mp4",
            ".mp3",
            ".png",
            ".jpg",
            ".jpeg",
            ".svg"
        };
    }
}
