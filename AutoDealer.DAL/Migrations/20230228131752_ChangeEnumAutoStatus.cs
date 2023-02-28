using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEnumAutoStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
alter table autos alter status type text;
drop type auto_status cascade;
create type auto_status as enum ('assembled', 'testing', 'selling', 'sold');
alter type auto_status owner to postgres;
alter table autos alter status type auto_status
using status::auto_status;
alter table autos alter status set default 'assembled'::auto_status;
""";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = """
alter table autos alter status type text;
drop type auto_status cascade;
create type auto_status as enum ('in_assembly', 'ready_to_test', 'in_test', 'ready_to_sale', 'sold');
alter type auto_status owner to postgres;
alter table autos alter status type auto_status
using status::auto_status;
alter table autos alter status set default 'in_assembly'::auto_status;
""";
            migrationBuilder.Sql(sql);
        }
    }
}
