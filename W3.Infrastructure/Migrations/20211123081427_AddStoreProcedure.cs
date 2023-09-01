using Microsoft.EntityFrameworkCore.Migrations;

namespace W3.Infrastructure.Migrations
{
    public partial class AddStoreProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var ForgotPassword=@"CREATE or ALTER     procedure [dbo].[updatePassword](@id int, @password nvarchar(max))
                                  as
                                  update dbo.AspNetUsers set PasswordHash = @password where Id = @id;
                                  select* from dbo.AspNetUsers where Id = @id";
            migrationBuilder.Sql(ForgotPassword);

            var getmenu = @"CREATE or ALTER   PROCEDURE [dbo].[GetMenus] @role NVARCHAR(8)
                            AS

                            BEGIN

                            SELECT *
                            FROM sidemenus t

                            INNER JOIN subMenus   AS k   ON t.MenuId= k.MenuId

                            WHERE t.Role = @role

                            END";
            migrationBuilder.Sql(getmenu);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
