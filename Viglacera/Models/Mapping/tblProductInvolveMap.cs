using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Viglacera.Models.Mapping
{
    public class tblProductInvolveMap : EntityTypeConfiguration<tblProductInvolve>
    {
        public tblProductInvolveMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tblProductInvolve");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idP).HasColumnName("idP");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
