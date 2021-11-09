using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace LigricCommonTypesMvvm.Converters
{
    public class CollectionToUniqueCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var elements = (IEnumerable)value;

            List<IDictionary<string, string>> result = new List<IDictionary<string, string>>();

            foreach (var item in elements)
            {
                Dictionary<string,string> properties = new Dictionary<string, string>();
                foreach (var keyValuePair in item.DictionaryFromType())
                {
                    properties.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                }

                result.Add(properties);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal static class Extantions
    {
        public static Dictionary<string, object> DictionaryFromType(this object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
    }
}
