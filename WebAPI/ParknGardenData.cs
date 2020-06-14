using System.Collections.Generic;
using System.Web.Http;

namespace WebAPI
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ParknGardenData : DbContext
    {
        public ParknGardenData()
            : base("name=ParknGardenData")
        {
            base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Auth> Auths { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceHasItem> InvoiceHasItems { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderHasItem> OrderHasItems { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockHasItem> StockHasItems { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLevel> UserLevels { get; set; }
        public virtual DbSet<InvoiceStatu> InvoiceStatus { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Auth>()
                .Property(e => e.PasswordHash)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Auth>()
                .Property(e => e.PasswordSalt)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.SessionKey)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceHasItems)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.PictureSource)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Size)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.InvoiceHasItems)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.OrderHasItems)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.StockHasItems)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderHasItems)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Stocks)
                .WithMany(e => e.Orders)
                .Map(m => m.ToTable("StockHasOrder").MapLeftKey("OrderID").MapRightKey("StockID"));

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stock>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Stock>()
                .HasMany(e => e.StockHasItems)
                .WithRequired(e => e.Stock)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Store)
                .HasForeignKey(e => e.StoreID);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Suppliers)
                .WithMany(e => e.Stores)
                .Map(m => m.ToTable("StoreHasSupplier").MapLeftKey("StoreID").MapRightKey("SupplierID"));

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Auth)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AuthorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Salary)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Stores)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AdministratorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Stores1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.ManagerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserLevel>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<UserLevel>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceStatu>()
                .Property(e => e.Name)
                .IsUnicode(false);
            modelBuilder.Entity<Log>()
                .Property(e => e.LogEntry)
                .IsUnicode(false);
        }
    }
}
