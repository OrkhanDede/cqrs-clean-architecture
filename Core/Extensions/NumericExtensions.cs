namespace Core.Extensions
{
    public static class NumericExtensions
    {
        public static double? DivideNull(this double a, double b)
        {
            var result = a / b;
            if (double.IsInfinity(result))
                return null;
            return result;
        }  
        public static double? DivideNull(this double a, double? b)
        {

            if ( b.HasValue)
            {
                return a.DivideNull(b.Value);
            }

            return null;
        }


    }
}
