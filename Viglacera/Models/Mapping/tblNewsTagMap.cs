using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Viglacera.Models.Mapping
{
    public class tblNewsTagMap : EntityTypeConfiguration<tblNewsTag>
    {
        public tblNewsTagMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(200);

            this.Property(t => t.Tag)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("tblNewsTag");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idn).HasColumnName("idn");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Tag).HasColumnName("Tag");
        }
    }
}
