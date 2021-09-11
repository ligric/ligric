namespace Common
{
    public static class ExtensionMethods
    {
        /// <summary>Добавляет элементы последовательности <paramref name="source"/> в конец коллекции <paramref name="collection"/>.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="collection">Коллекция в которую добавляются элементы.</param>
        /// <param name="source">Последовательность добавляемых элементов.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
        {
            foreach (var item in source)
                collection.Add(item);
        }

        /// <summary>Инициализирует коллекцию <paramref name="collection"/> элементами последовательности <paramref name="source"/>.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="collection">Инициализируемая коллекция.</param>
        /// <param name="source">Последовательность элементов помещаемых в коллекцию <paramref name="collection"/>.</param>
        /// <remarks>Коллекция <paramref name="collection"/> очищается методом <see cref="ICollection{T}.Clear"/>
        /// после чего в неё добавляются все элементы последовательноссти <paramref name="source"/>.</remarks>
        public static void Initial<T>(this ICollection<T> collection, IEnumerable<T> source)
        {
            collection.Clear();
            collection.AddRange(source);
        }


        /// <summary>Метод заменяющий элемент в индексированной коллекции.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="list">Индексированная коллекция.</param>
        /// <param name="predicate">Предикат для поиска элемента в коллекции, котороый надо заменить.</param>
        /// <param name="newItem">Элемент на который будет заменён найденный элемент.</param>
        /// <returns><see langword="true"/> если элемент был найден и заменён, иначе - <see langword="false"/>.</returns>
        public static bool Replace<T>(this IList<T> list, Predicate<T> predicate, T newItem)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list[i] = newItem;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Метод заменяющий или добавляющий элемент в индексированную коллекции.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="list">Индексированная коллекция.</param>
        /// <param name="predicate">Предикат для поиска элемента в коллекции, котороый надо заменить.</param>
        /// <param name="newItem">Элемент на который будет заменён найденный элемент.</param>
        /// <returns><see langword="true"/> если элемент был найден и заменён,
        /// <see langword="false"/> - элемент не был найден и <paramref name="newItem"/> добаяляется в коллекцию.</returns>
        public static bool ReplaceOrAdd<T>(this IList<T> list, Predicate<T> predicate, T newItem)
        {
            if (list.Replace(predicate, newItem))
                return true;

            list.Add(newItem);
            return false;
        }

        /// <summary>Удаляет первый элемент в индексированной коллекцию,
        /// удовлетворяющий предикату <paramref name="predicate"/>.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="list">Индексированная коллекция.</param>
        /// <param name="predicate">Предикат для поиска элемента в коллекции, котороый надо удалить.</param>
        /// <returns><see langword="true"/> если элемент был найден и удалён, иначе - <see langword="false"/>.</returns>
        public static bool RemoveFirst<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>Возвращает коллекцию поэлементно.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="collection">Исходная коллекция.</param>
        /// <returns>Последовательность элементов коллекции.</returns>
        /// <remarks>Используется для защиты исходной коллекции от изменений.</remarks>
        public static IEnumerable<T> GetEnumerable<T>(this IEnumerable<T> collection)
        {
            foreach (T item in collection)
                yield return item;
        }

        /// <summary>Добавляет элемент в конец последовательности.</summary>
        /// <typeparam name="TSource">Тип элемента последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="element">Добавляемый элемент.</param>
        /// <returns>Последовательность с добавленным элементом.</returns>
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element)
        {
            foreach (var item in source)
            {
                yield return item;
            }

            yield return element;
        }

        /// <summary>Добавляет элемент в начало последовательности.</summary>
        /// <typeparam name="TSource">Тип элемента последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="element">Добавляемый элемент.</param>
        /// <returns>Последовательность с добавленным элементом.</returns>
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
        {
            yield return element;
            foreach (var item in source)
            {
                yield return item;
            }

        }

        /// <summary>Удаляет из индексированной коллекции все элементы удобвлетворяющие условию.</summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="list">Индексированная коллекция.</param>
        /// <param name="predicate">Предикат с условием.</param>
        /// <returns>Количество удалённых элементов.</returns>
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            int count = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    count++;
                }
            }

            return count;
        }
    }
}
