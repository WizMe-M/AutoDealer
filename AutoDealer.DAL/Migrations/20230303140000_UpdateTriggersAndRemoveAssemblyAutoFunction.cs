using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTriggersAndRemoveAssemblyAutoFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
drop function assembly_auto(auto integer);

create or replace function tr_log_contracts_ins() returns trigger
    language plpgsql
as
$$
declare
    value text;
begin
    value = 'INSERT IN contracts (id = ' || new.id_contract || '): ' ||
            'supplier = ' || new.id_supplier || '; total sum of details = ' || new.total_sum ||
            '; supply date = ' || new.supply_date || '; employee = ' || new.id_storekeeper;
    perform log(value);
    return new;
end;
$$;
""";
            migrationBuilder.Sql(sql);
        }
    }
}
