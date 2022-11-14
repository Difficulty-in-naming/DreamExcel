using System;

namespace CrossPlatformGenerator.Core
{
    public class Parse
    {
        public static bool BoolParse(string value)
        {
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;
            else if (int.TryParse(value, out int i))
            {
                if (i > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}