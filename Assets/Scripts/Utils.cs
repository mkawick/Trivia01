using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Utils
{
    public static bool IsInList(string needle, string[] answers)
    {
        foreach (var answer in answers)
        {
            if (needle == answer)
                return true;
        }
        return false;
    }
}
