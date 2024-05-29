using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventosCeremonial.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                base.OnModelCreating(builder);
            
            }
            catch (Exception ex)
            {
                logger.LogError("error en OnModelCreating", ex);

            }

        }
    }
}
