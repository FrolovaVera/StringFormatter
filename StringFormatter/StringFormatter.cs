using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter
{
    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new StringFormatter();
        private Reflection reflection;

        public StringFormatter()
        {
            reflection = new Reflection();
        }

        public string Format(string template, object target)
        {
            if (!Correct(template))
                throw new Exception("Brackets excepion");
            var substrings = template.Split(new string[] { "{{" }, System.StringSplitOptions.RemoveEmptyEntries);//Возвращает строковый массив, содержащий подстроки данного экземпляра, разделенные элементами заданной строки или массива знаков Юникода.
            for (int i = 0; i < substrings.Length; i++)
            {
                substrings[i] = formatSubstring(substrings[i], target);
            }
            string result = string.Join("{", substrings);
            if (template.IndexOf("{{") == 0)
                result = "{" + result;

            if (template.LastIndexOf("{{") == template.Length - 2)
                result = result + "{";
            result = result.Replace("}}", "}");
            return result;
        }
        private bool Correct(string _template)
        {
            string template = _template;
            template = _template.Replace("{{", "");
            template = template.Replace("}}", "");

            int Brackets = 0;
            foreach (var item in template.ToCharArray())
            {
                if (item == '{')
                {
                    Brackets++;
                }
                else if (item == '}')
                {
                    Brackets--;
                    if (Brackets < 0)
                    {
                        return false;
                    }


                }
            }
            if (Brackets == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string formatSubstring(string template, object target)
        {


            if (!Correct(template))
                throw new Exception("Brackets excepion");

            int startId, endId;
            Substring(template, 0, out startId, out endId);
            var resultedString = template;
            while (startId != endId)
            {
                int SubstringId;
                resultedString = Change(target, resultedString, startId, endId, out SubstringId);
                Substring(resultedString, SubstringId, out startId, out endId);
            }

            return resultedString;
        }
        private string Change(object obj, string str, int startId, int endId, out int endOfChangedSubstring)
        {
            //get string in brackets;
            var field = str.Substring(startId, endId - startId);
            field = field.Trim(' ');

            var value = reflection.GetValue(obj, field);

            string result = str;
            if (value != null)
            {
                var subStringLength = endId - startId + 2;
                string replacedString = str.Substring(startId - 1, subStringLength);
                result = str.Replace(replacedString, value.ToString());
                endOfChangedSubstring = endId - (subStringLength - value.ToString().Length - 1);
            }
            else
            {
                endOfChangedSubstring = endId;
            }

            return result;
        }
        private void Substring(string str, int startedFrom, out int startId, out int endId)
        {
            bool isFoundBoundaries = false;
            startId = str.IndexOf("{", startedFrom) + 1;


            int secondStart;
            endId = 0;
            while (isFoundBoundaries == false && startId != 0)
            {
                endId = str.IndexOf("}", startId);
                while (endId > 0 && endId < str.Length && str.IndexOf("}}", endId) == endId)
                {
                    endId = str.IndexOf("}", endId + 1);

                }
                secondStart = str.IndexOf("{", startId) + 1;

                if (secondStart < endId && secondStart != 0)
                {
                    startId = secondStart;
                }
                else
                {
                    isFoundBoundaries = true;
                }
            }

        }
    }
}
