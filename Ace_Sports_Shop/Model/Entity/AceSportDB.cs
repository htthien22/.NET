namespace Model.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AceSportDB : DbContext
    {
        public AceSportDB()
            : base("name=AceSportDB")
        {
        }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Tbl_Contact> Tbl_Contact { get; set; }
        public virtual DbSet<Tbl_Feedback> Tbl_Feedback { get; set; }
        public virtual DbSet<Tbl_Order> Tbl_Order { get; set; }
        public virtual DbSet<Tbl_OrderDetail> Tbl_OrderDetail { get; set; }
        public virtual DbSet<Tbl_Product> Tbl_Product { get; set; }
        public virtual DbSet<Tbl_ProductCategory> Tbl_ProductCategory { get; set; }
        public virtual DbSet<Tbl_Slider> Tbl_Slider { get; set; }
        public virtual DbSet<Tbl_User> Tbl_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Tbl_Order>()
                .Property(e => e.ShipMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Order>()
                .HasMany(e => e.Tbl_OrderDetail)
                .WithRequired(e => e.Tbl_Order)
                .HasForeignKey(e => e.OrderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tbl_OrderDetail>()
                .Property(e => e.Price)
                .HasColumnType("int");           

            modelBuilder.Entity<Tbl_Product>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Product>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Product>()
                .HasMany(e => e.Tbl_OrderDetail)
                .WithRequired(e => e.Tbl_Product)
                .HasForeignKey(e => e.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tbl_ProductCategory>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_ProductCategory>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_ProductCategory>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_ProductCategory>()
                .Property(e => e.MetaDescriptions)
                .IsFixedLength();

            modelBuilder.Entity<Tbl_ProductCategory>()
                .HasMany(e => e.Tbl_Product)
                .WithOptional(e => e.Tbl_ProductCategory)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<Tbl_Slider>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Slider>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_User>()
                .Property(e => e.GroupID)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_User>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_User>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);
        }
    }
}
