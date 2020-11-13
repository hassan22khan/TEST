using DependencyResolver;
using IRepository;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IKernel kernel = new StandardKernel(new NinjectBindings());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run
                (new LoginForm
                (kernel.Get<IUserRepository>(),
                kernel.Get<ICoursesRepository>(),
                kernel.Get<IStudentsRepository>()
                )
                );
        }
    }
}
