using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public IConfiguration _iconfiguration;
        public MyContext(DbContextOptions<MyContext> options, IConfiguration configuration) : base(options)
        {

            this._iconfiguration = configuration;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts{ get; set; }
        public DbSet<Department> Departments{ get; set; }
        public DbSet<EmployeeAttachment> EmployeeAttachments{ get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Reimburse> Reimburses{ get; set; }
        public DbSet<ReimburseHistory> ReimburseHistories{ get; set; }
        public DbSet<RequestForm> RequestForms{ get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One to One
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Accounts)
                .WithOne(b => b.Employees)
                .HasForeignKey<Account>(b => b.Id);

            //One to many
            modelBuilder.Entity<Employee>()
                .HasMany(c => c.Reimburses)
                .WithOne(c => c.Employees);

            //One to many
            modelBuilder.Entity<Reimburse>()
                .HasMany(c => c.ReimburseHistories)
                .WithOne(c => c.Reimburses);

            //One to many
            modelBuilder.Entity<Reimburse>()
                .HasMany(c => c.RequestForms)
                .WithOne(c => c.Reimburses);

            //One to many
            modelBuilder.Entity<Department>()
                .HasMany(c => c.Employees)
                .WithOne(c => c.Departments);

            //One to many
            modelBuilder.Entity<Job>()
                .HasMany(c => c.Employees)
                .WithOne(c => c.Jobs);
        }

        }
    class CustomResolver : DefaultContractResolver
    {
        private readonly List<string> _namesOfVirtualPropsToKeep = new List<string>(new String[] { });

        public CustomResolver() { }

        public CustomResolver(IEnumerable<string> namesOfVirtualPropsToKeep)
        {
            this._namesOfVirtualPropsToKeep = namesOfVirtualPropsToKeep.Select(x => x.ToLower()).ToList();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            var propInfo = member as PropertyInfo;
            if (propInfo != null)
            {
                if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal
                    && !_namesOfVirtualPropsToKeep.Contains(propInfo.Name.ToLower()))
                {
                    prop.ShouldSerialize = obj => false;
                }
            }
            return prop;
        }

    }
}
