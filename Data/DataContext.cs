using Microsoft.EntityFrameworkCore;
using RelatedEntities.Models;

namespace RelatedEntities.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base (options){}

        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<Order> Orders {get; set;}

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

            //Configuracion de la entidad User 
            modelbuilder.Entity<User>()
            .HasKey(u => u.Id); //llave primaria de la entidad User 
            
            modelbuilder.Entity<User>()//User
            .HasMany(u => u.Orders)// Un User puede estar en varios Orders 
            .WithOne(o => o.User)//Un Order solo tiene un User 
            .HasForeignKey(o => o.UserId);//llave foranea en la entida Order de User

            //Configuracion de la entidad Product 
            modelbuilder.Entity<Product>()
            .HasKey(p => p.Id); //llave primaria de la entidad Product

            modelbuilder.Entity<Product>()//Product
            .HasMany(p => p.Orders)//Un Product puede estar en varios Orders
            .WithOne(o => o.Product)// Un Product solo tiene un Order 
            .HasForeignKey(o => o.ProductId); //llave foranea en la entidad Order de Product


            //Configuracion de la entidad   Orders 
            modelbuilder.Entity<Order>()
            .HasKey(o => o.Id); //llave primaria en la entidad Order

            modelbuilder.Entity<Order>()//Order
            .HasOne(o => o.User)//un Order solo tine un User 
            .WithMany(u => u.Orders)//un User tiene muchos Order
            .HasForeignKey(o => o.UserId);//llave foranea en la entidad Order de User

            modelbuilder.Entity<Order>()//Orden
            .HasOne(o => o.User)// una order tiene un User
            .WithMany(u => u.Orders)//un User tiene muchos Orders
            .HasForeignKey(o => o.ProductId);//llave foranea en la entidad Order de Product


        }
       
    }
}