using WTLib.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WTLib.Utils
{
    public static class Deed
    {
        public static bool TryCallWait(Task task)
        {
            if (task == null)
                return false;

            try
            {
                task.Wait();
                return false;
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions != null)
                    foreach (var item in ae.InnerExceptions)
                    {
                        Log.Trace.Error(
                            "Task wait exception, Type：{1}{0}Source：{2}{0}Message：{3}{0}{0}",
                            Environment.NewLine,
                            item.GetType(),
                            item.Source,
                            item.Message);
                    }
                return true;
            }
        }

        public static Task TryContinue(Action action, Action<Task> continuationAction, [CallerMemberName] string memberName = "")
        {
            var task = Task.Run(() =>
            {
                if (action == null)
                    return;

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Log.Trace.Error(ex.Message);
                }
            });
            task.ContinueWith(s =>
            {
                if (continuationAction == null)
                    return;

                try
                {
                    continuationAction(s);
                }
                catch (Exception ex)
                {
                    Log.Trace.Error("*** {4} Exception *** {0}Message: {1}{0}Source: {2}{0}StackTrace: {3}{0}",
                        Environment.NewLine,
                        ex.Message,
                        ex.Source,
                        ex.StackTrace,
                        memberName);
                }
            });
            return task;
        }

        #region GetPageDatas
        private static readonly int PageSize = 300;

        public static IEnumerable<T> GetPageDatas<T>(
            int pageSize, Func<int, int> getPageCountFunc, Func<int, int, IEnumerable<T>> getDatasFunc)
            where T : new()
        {
            if (pageSize <= 0)
                return null;
            if (getPageCountFunc == null)
                return null;
            if (getDatasFunc == null)
                return null;

            var page = getPageCountFunc(pageSize);
            if (page <= 0)
                return null;

            IEnumerable<T> pageData = new List<T>();

            for (int i = 1; i <= page; i++)
            {
                var perPageData = getDatasFunc(pageSize, i);
                if (perPageData != null)
                {
                    pageData = pageData.Union(perPageData);
                }
            }

            return pageData;
        }

        public static void GetPageDatas<T>(
            Func<int, int> getPageCountFunc,
            Func<int, int, IEnumerable<T>> getDatasFunc,
            Action<T> receivedAction)
            where T : new()
        {
            GetPageDatas<T>(PageSize, getPageCountFunc, getDatasFunc, receivedAction);
        }

        public static void GetPageDatas<T>(
            int pageSize, Func<int, int> getPageCountFunc, Func<int, int, IEnumerable<T>> getDatasFunc, Action<T> receivedAction)
            where T : new()
        {
            if (pageSize <= 0)
                return;
            if (getPageCountFunc == null)
                return;
            if (getDatasFunc == null)
                return;
            if (receivedAction == null)
                return;

            var page = getPageCountFunc(pageSize);
            if (page <= 0)
                return;

            for (int i = 1; i <= page; i++)
            {
                var datas = getDatasFunc(pageSize, i);
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        receivedAction(item);
                    }
                }
            }
        }

        public static void GetPageDatas<T>(
            int pageSize,
            string filter,
            Func<int, string, int> getPageCountFunc,
            Func<int, int, string, IEnumerable<T>> getDatasFunc, Action<T> receivedAction)
            where T : new()
        {
            if (pageSize <= 0)
                return;
            if (getPageCountFunc == null)
                return;
            if (getDatasFunc == null)
                return;
            if (receivedAction == null)
                return;

            var page = getPageCountFunc(pageSize, filter);
            if (page <= 0)
                return;

            for (int i = 1; i <= page; i++)
            {
                var datas = getDatasFunc(pageSize, i, filter);
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        receivedAction(item);
                    }
                }
            }
        }

        public static void GetPageDatas<T>(
            Func<int, bool, int> getPageCountFunc,
            Func<int, int, bool, IEnumerable<T>> getDatasFunc,
            Action<T> receivedAction,
            bool isAll) where T : new()
        {
            GetPageDatas(PageSize, getPageCountFunc, getDatasFunc, receivedAction, isAll);
        }

        public static void GetPageDatas<T>(
            int pageSize,
            Func<int, bool, int> getPageCountFunc,
            Func<int, int, bool, IEnumerable<T>> getDatasFunc,
            Action<T> receivedAction,
            bool isAll) where T : new()
        {
            if (pageSize <= 0)
                return;
            if (getPageCountFunc == null)
                return;
            if (getDatasFunc == null)
                return;

            var page = getPageCountFunc(pageSize, isAll);
            if (page <= 0)
                return;

            for (int i = 1; i <= page; i++)
            {
                var datas = getDatasFunc(pageSize, i, isAll);
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        receivedAction(item);
                    }
                }
            }
        }
        #endregion
    }
}
