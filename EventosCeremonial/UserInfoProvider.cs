using EventosCeremonial.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosCeremonial
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
    public class UserInfoProvider 
    {
        private readonly IConfiguration config;
        private readonly ILogger<UserInfoProvider> logger;
        private readonly EventosCeremonialContext dbcontext;
        public UserInfoProvider(IConfiguration configuration, EventosCeremonialContext dbcontext, ILogger<UserInfoProvider> logger)
        {
            this.logger = logger;
            config = configuration;
            this.dbcontext = dbcontext;
        }

        public async Task<UserInfo> AsegurarUsuario(string name)
        {
            await dbcontext.SaveChangesAsync();

            return new UserInfo
            {
                UserName = name,
                DisplayName = ""
            };
        }
    }
}
