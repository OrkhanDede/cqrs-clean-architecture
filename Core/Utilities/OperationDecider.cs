namespace Core.Utilities
{
    public class OperationDecider
    {
        public static bool CanDo(bool IsAdmin, bool havePermission, bool IsOwner)
        {

            if (IsAdmin || havePermission)
                return true;

            else if (IsOwner)
                return true;


            return false;
        }
    }
}
