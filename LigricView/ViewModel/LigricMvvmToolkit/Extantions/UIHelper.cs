using LigricMvvmToolkit.Multibinding.Foundation.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using static LigricMvvmToolkit.Extantions.DependencyObjectExtensions;

namespace LigricMvvmToolkit.Extantions
{
    public static partial class ExtensionMethods
    {

        private class ConstractorExtantions<T> where T : DependencyObject
        {
            private static readonly Dictionary<Type, Func<T>> constructors = new Dictionary<Type, Func<T>>();

            private static readonly Type[] emptyTypes = new Type[0];

            public static Func<T> GetConstructor(Type type)
            {
                if (!constructors.TryGetValue(type, out var func))
                {
                    var constructor = type.GetConstructor
                        (
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                            null, emptyTypes, null
                        );
                    if (constructor == null)
                    {
                        func = new Func<T>(() => default(T));
                    }
                    else
                    {
                        DynamicMethod dynamic = new DynamicMethod(string.Empty,
                                    type,
                                    Type.EmptyTypes,
                                    type);
                        ILGenerator il = dynamic.GetILGenerator();

                        il.DeclareLocal(type);
                        il.Emit(OpCodes.Newobj, constructor);
                        il.Emit(OpCodes.Stloc_0);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ret);

                        func = (Func<T>)dynamic.CreateDelegate(typeof(Func<T>));
                    }
                    constructors.Add(type, func);
                }
                return func;
            }
        }

        /// <summary>Создаёт клон экземпляра класса <typeparamref name="T"/>.</summary>
        /// <typeparam name="T">Тип экземплярв.</typeparam>
        /// <param name="obj">Исходный экземпляр.</param>
        /// <returns>Возвращает новый экземпляр типа <typeparamref name="T"/>
        /// являющийся копией исходного экземляра.</returns>
        /// <remarks>Если класс <typeparamref name="T"/> реализует интерфейс <see cref="ICloneable"/>,
        /// то копия создаётся методом <see cref="ICloneable.Clone"/>.
        /// Иначе - методом 
        /// <a href="https://docs.microsoft.com/ru-ru/dotnet/api/system.object.memberwiseclone?view=net-5.0">
        /// Object.MemberwiseClone()</a>.</remarks>
        public static T Clone<T>(this T obj)
        {
            if (obj is ICloneable cln && cln.Clone() is T t)
                return t;

            return (T)memberwiseClone(obj);
        }

        private static readonly Func<object, object> memberwiseClone
            = (Func<object, object>)(typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance))
            .CreateDelegate(typeof(Func<object, object>));

        /// <summary>Создаёт клон экземпляра класса <typeparamref name="T"/>.
        /// <typeparamref name="T"/> - <see cref="DependencyObject"/> или производный от него.</summary>
        /// <typeparam name="T">Тип экземплярв.</typeparam>
        /// <param name="dObj">Исходный экземпляр.</param>
        /// <param name="newObject">Делегат функции создающей новый экземпляр объекта из заданного,
        /// без копирования значений <see cref="DependencyProperty"/>.</param>
        /// <returns>Возвращает новый экземпляр типа <typeparamref name="T"/>
        /// являющийся копией исходного экземляра.</returns>
        /// <remarks>Создаётся копия методом <see cref="Clone{T}(T)"/>.<br/>
        /// После этого в клон копируются привязки или значения из всех
        /// <see cref="DependencyProperty"/> заданных в исходном экземпляре.</remarks>
        public static T CloneDO<T>(this T dObj, Func<T, T> newObject)
            where T : DependencyObject
        {
            T clone = newObject(dObj);
            if (clone == null)
            {
                throw new Exception("Не удаётся создать новый экземпляр этого типа.");
            }

            dObj.CopyDpTo(clone);

            return clone;
        }

        /// <summary>Копирует значения и привязки всех <see cref="DependencyProperty"/>
        /// из объекта источника в целевой объект.</summary>
        /// <typeparam name="T">Тип объектов.</typeparam>
        /// <param name="source">Объект источник.</param>
        /// <param name="target">Целевой объект.</param>
        public static void CopyDpTo<T>(this T source, T target)
            where T : DependencyObject
        {
            Dictionary<string,DependencyProperty> properties = new Dictionary<string, DependencyProperty>();
            ////// Looking for DependencyProperties by Reflex.
            {
                var propEnumerator = source.GetType().GetTypeInfo().DeclaredProperties.Where(x => true).GetEnumerator();
                while (propEnumerator.MoveNext())
                {
                    if (propEnumerator.Current.PropertyType == typeof(DependencyProperty)
                        /*&& propEnumerator.Current.CanWrite*/)
                    {
                        string fullName = propEnumerator.Current.Name;
                        var basePropertyName = fullName.Remove(fullName.Length - 8);
                        properties.Add(basePropertyName,(DependencyProperty)propEnumerator.Current.GetValue(source));
                    }
                }
            }
            ////// Deliting properties that are not in another object.
            {
                var propEnumerator = target.GetType().GetTypeInfo().DeclaredProperties.Where(x => true).GetEnumerator();
                while (propEnumerator.MoveNext())
                {
                    string fullName = propEnumerator.Current.Name;

                    if (fullName.Length < 8)
                        continue;

                    var basePropertyName = fullName.Remove(fullName.Length - 8); 
                    if (propEnumerator.Current.PropertyType == typeof(DependencyProperty) && !properties.TryGetValue(basePropertyName, out var value)
                        /*&& propEnumerator.Current.CanWrite*/)
                    {
                        target.ClearValue((DependencyProperty)propEnumerator.Current.GetValue(target));
                    }
                }
            }

            foreach (var property in properties)
            {
                Binding binding = new Binding()
                {
                    ////// Grid
                    Source = source,
                    ////// DependencyProperty
                    Path = new PropertyPath(property.Key), 
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                };
                BindingOperations.SetBinding(target, property.Value, binding);
            }
        }

        ///<inheritdoc cref="CloneDO{T}(T, Func{T, T})"/>
        public static T CloneDO<T>(this T dObj)
            where T : DependencyObject
        {
            Func<T> constructor = ConstractorExtantions<T>.GetConstructor(dObj.GetType());
            T clone = constructor();
            if (clone == null)
            {
                throw new Exception("Не удаётся создать новый экземпляр этого типа.");
            }

            dObj.CopyDpTo(clone);

            return clone;
        }
    }


    public static class UIHelper
    {
        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        public static T FindVisualParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            // get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return default(T);

            // check if the parent matches the type we’re looking for
            if (parentObject is T parent && parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }

        /// <summary>
        /// Will navigate down the VisualTree to find an element that is of the provided type.
        /// </summary>
        /// <typeparam name="T">The type of object to search for</typeparam>
        /// <param name="element">The element to start searching at</param>
        /// <returns>The found child or null if not found</returns>
        public static T GetVisualChild<T>(this DependencyObject element) where T : DependencyObject
        {
            T child = default(T);
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(element, i);
                if (obj is T)
                {
                    child = (T)obj;
                    break;
                }
                else
                {
                    child = GetVisualChild<T>(obj);
                    if (child != null)
                        break;
                }
            }

            return child;
        }

#region Clone
        public static T DeepClone<T>(this T source) where T : UIElement
        {
            T result; // Get the type 
            Type type = source.GetType(); // Create an instance 
            result = Activator.CreateInstance(type) as T;
            CopyProperties<T>(source, result, type);
            DeepCopyChildren<T>(source, result);
            return result;
        }

        private static void DeepCopyChildren<T>(T source, T result) where T : UIElement
        {
            // Deep copy children. 
            Panel sourcePanel = source as Panel;
            if (sourcePanel != null)
            {
                Panel resultPanel = result as Panel;
                if (resultPanel != null)
                {
                    foreach (UIElement child in sourcePanel.Children)
                    {
                        // RECURSION! 
                        UIElement childClone = DeepClone(child);
                        resultPanel.Children.Add(childClone);
                    }
                }
            }
        }

        private static void CopyProperties<T>(T source, T result, Type type) where T : UIElement
        {
            // Copy all properties. 
            IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties();
            foreach (var property in properties)
            {
                if (property.Name != "Name")
                { // do not copy names or we cannot add the clone to the same parent as the original. 
                    if ((property.CanWrite) && (property.CanRead))
                    {
                        object sourceProperty = property.GetValue(source);
                        UIElement element = sourceProperty as UIElement;
                        if (element != null)
                        {
                            UIElement propertyClone = element.DeepClone();
                            property.SetValue(result, propertyClone);
                        }
                        else
                        {
                            try
                            {
                                property.SetValue(result, sourceProperty);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex);
                            }
                        }
                    }
                }
            }
        }
#endregion
    }
}
