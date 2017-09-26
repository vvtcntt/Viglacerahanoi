using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Viglacera.Models.Mapping
{
    public class tblCustomerMap : EntityTypeConfiguration<tblCustomer>
    {
        public tblCustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.Mobile)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("tblCustomer");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
        }
    }
}
