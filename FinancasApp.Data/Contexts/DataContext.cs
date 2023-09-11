using FinancasApp.Data.Entities;
using FinancasApp.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Data.Contexts
{
    //Classe para configuração do acesso ao banco de dados
    //realizado pelo EntityFramework
    //REGRA 1: Herdar DbContext!
    public class DataContext : DbContext
    {
        //REGRA 2: Implementar o método 'OnConfiguring' para
        //mapearmos a string de conexão do banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //configurando o caminho do banco de dados (connectionstring)
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BDFinancasApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        //REGRA 3: Implementar o método 'OnModelCreating' para
        //adicionar cada classe de mapeamento do projeto
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //adicionar cada classe de mapeamento do projeto
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ContaMap());
        }

        //REGRA 4: Para cada entidade do projeto declare
        //uma propriedade do tipo DbSet
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Conta> Conta { get; set; }
    }
}
