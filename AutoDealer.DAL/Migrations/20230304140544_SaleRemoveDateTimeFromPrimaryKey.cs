using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SaleRemoveDateTimeFromPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
create or replace function sell_auto(auto integer, client integer, employee integer) returns void
    language plpgsql
as
$$
declare
    auto_cost     decimal;
    real_status   auto_status;
    trim_id       int;
    active_margin record;
begin

    select cost, status, id_trim
    into auto_cost, real_status, trim_id
    from autos
    where id_auto = auto;

    if real_status != 'selling' then
        raise exception 'It is available to sale only "selling" autos! Current status is: %', real_status;
    end if;


    select *
    from margins
    where start_date <= current_date
      and id_trim = trim_id
    order by start_date desc
    limit 1
    into active_margin;

    if active_margin is null then
        raise exception 'Is is not possible to sell auto without margin';
    end if;

    auto_cost = auto_cost * (1 + active_margin.margin / 100.0);

    update autos
    set status = 'sold'
    where id_auto = auto;

    insert into sales(id_auto, id_client, id_employee, total_sum)
    values (auto, client, employee, auto_cost);
end;
$$;

create or replace function return_auto(auto integer) returns void
    language plpgsql
as
$$
begin
    update autos
    set status = 'selling'
    where id_auto = auto;

    delete
    from sales
    where id_auto = auto;
end;
$$;
""";
            migrationBuilder.Sql(sql);
            
            migrationBuilder.DropPrimaryKey(
                name: "pk_sales",
                table: "sales");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sales",
                table: "sales",
                column: "id_auto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_sales",
                table: "sales");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sales",
                table: "sales",
                columns: new[] { "id_auto", "execution_date" });
        }
    }
}
