//using DigitalStore.EF;
//using DigitalStore.Identity.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.Owin;
//using System;

//namespace DigitalStore.Identity.Infrastructure
//{
//    public class AppRoleManager : RoleManager<AppRole>, IDisposable
//    {
//        public AppRoleManager(RoleStore<AppRole> store) : base(store) { }

//        public static AppRoleManager Create (IdentityFactoryOptions<AppRoleManager> options,
//            IOwinContext context)
//        {
//            return new AppRoleManager(new RoleStore<AppRole>(context.Get<DigitalStoreContext>()));
//        }
//    }
//}
