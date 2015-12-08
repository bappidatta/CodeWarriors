namespace CodeWarriors.API.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using CodeWarriors.BLL.Interfaces;
    using CodeWarriors.BLL.Logic;
    using CodeWarriors.DAL.Model;
    
    public static class SimpleInjectorWebApiInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            container.RegisterWebApiRequest<IUserBLL, UserBLL>();
            container.RegisterWebApiRequest<IPostBLL, PostBLL>();
            container.RegisterWebApiRequest<IFriendBLL, FriendBLL>();
        }
    }
}