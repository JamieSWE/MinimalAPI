using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentEnrollment.Data.Configurations;

internal class UserRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {

                Name = "Administrator",
                NormalizedName = "ADMINISTARTOR"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }
       );
    }
}
