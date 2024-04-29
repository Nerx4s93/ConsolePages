using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace ConsolePages
{
    public static class PageMenager
    {
        private static Thread thread;

        private static List<Type> pagesTypes;

        private static Page page;
        private static bool loaded;

        public static void StartHandle(string pageName)
        {
            thread = new Thread(() =>
            {
                GetPages();
                LoadPage(pageName);

                while (true)
                {
                    if (!loaded)
                    {
                        page.OnLoaded();
                        loaded = true;
                    }

                    string input = Console.ReadLine();
                    page.OnInputData(input);
                }
            });
            thread.Start();
        }

        public static void LoadPage(string pageName)
        {
            Type pageType = pagesTypes.Find(item => item.Name == pageName);
            page = (Page)Activator.CreateInstance(pageType);

            Console.Clear();

            loaded = false;
        }

        public static void AbortThread()
        {
            thread.Abort();
        }

        private static void GetPages()
        {
            List<Type> list = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.IsSubclassOf(typeof(Page)))
                    {
                        list.Add(type);
                    }
                }
            }

            pagesTypes = list;
        }
    }
}
