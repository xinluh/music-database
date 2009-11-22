using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace libdb
{
    class Debug
    {
        public static void HandleException(Exception e)
        {
            throw e;
            //System.Console.WriteLine("exception in {1}: {0}", e.Message,e.TargetSite);
        }

        public static void WriteError(string error)
        {
            Console.WriteLine("err: {0}", error);
        }
        public static void WriteWarning(string warning)
        {
            Console.WriteLine("warning: {0}", warning);
        }    
    }

    static public class str
    {
        public const string LANG_GOOD = "good";
        public const string EXCEPTION_NULL_FIELDHOLDER =
            "the requested field corresponds to a null placeholder in the fields array";

        public const string EXCEPTION_NO_TABLE_RELATIONSHIP =
            "there is no table relationship defined for the specified tables";
        //public const string EXCEPTION_ = "";

    }

    static public class PathUtils
    {
        // http://blogs.msdn.com/michkap/archive/2005/02/19/376617.aspx
        static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string FixPathString(string path)
        {
            string[] invalidchar = {"/", ": ", ":", "*", "?", "\"", "<", ">", "|"};
            string[] replacement = {"-", " - ", "-", "", "", "'", "-", "-", "-"};

            for (int i = 0; i < invalidchar.Length; i++)
                path = path.Replace(invalidchar[i], replacement[i]);

            return RemoveDiacritics(path);
        }
    }

    public class NaturalSortComparer: IComparer<string>
    {
        public static NaturalSortComparer Default { get { return new NaturalSortComparer(); } }

        #region IComparer<string> Members

        public int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
                return Comparer<string>.Default.Compare(x, y);

            string[] X = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
            string[] Y = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
            int n1, n2, ret;
            bool b1, b2;

            for (int i = 0; i < Math.Min(X.Length, Y.Length); i++)
            {
                b1 = int.TryParse(X[i], out n1);
                b2 = int.TryParse(Y[i], out n2);

                if (b1 && b2 && (ret = Comparer<int>.Default.Compare(n1, n2)) != 0)  return ret;
                else if (b1 && !b2) return -1;
                else if (!b1 && b2) return 1;
                else if ((ret = Comparer<string>.Default.Compare(X[i], Y[i])) != 0)  return ret;
            }

            if (X.Length != Y.Length)
                return (X.Length > Y.Length) ? 1 : -1;

            return 0;
        }

        #endregion
    }
}
