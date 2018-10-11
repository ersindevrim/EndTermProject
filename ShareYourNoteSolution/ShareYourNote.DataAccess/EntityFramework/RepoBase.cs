
namespace ShareYourNote.DataAccess.EntityFramework
{
    public class RepoBase
    {
        protected static DataContext dataContext;
        private static object _Lock = new object();

        protected RepoBase()
        {
            //Artık new lenemez :D
            dataContext = CreateContext();
        }

        public static DataContext CreateContext()
        {
            if (dataContext == null)
            {
                lock (_Lock)
                {
                    if (dataContext == null)
                    {
                        dataContext = new DataContext();
                    }
                }
            }

            return dataContext;
        }
    }
}
