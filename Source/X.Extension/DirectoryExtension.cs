using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extension
{
    public static partial class DirectoryExtension
    {
        /// <summary>
        /// 递归删除目录
        /// </summary>
        /// <param name="source"></param>
        public static void Remove(this DirectoryInfo source)
        {
            var path = source.FullName;

            //find more info about directory deletion
            //and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true

            foreach (var directory in source.GetDirectories())
            {
                directory.Remove();
            }

            try
            {
                source.Delete(true);
            }
            catch (IOException)
            {
                source.Delete(true);
            }
            catch (UnauthorizedAccessException)
            {
                source.Delete(true);
            }
        }
    }
}
