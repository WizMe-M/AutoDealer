using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixFunctionsColNamesPresetData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
select ;
drop function log(type log_type, value text);
drop function return_auto(auto integer, sale_time timestamp);
drop function sell_auto(auto integer, client integer, seller integer);
drop function set_margin(trim_id integer, begins_act_from date, margin_value numeric);

create or replace function sell_auto(auto integer, client integer, seller integer) returns void
    language plpgsql
as
$$
declare
    auto_cost     decimal;
    real_status   auto_status;
    car_model_id       int;
    active_margin record;
    employee_post post;
begin

    select cost, status, id_car_model
    into auto_cost, real_status, car_model_id
    from autos
    where id_auto = auto;

    if real_status != 'selling' then
        raise exception 'It is available to sale only "selling" autos! Current status is: %', real_status;
    end if;

    select post
    into employee_post
    from employees
    where id_employee = seller;

    if employee_post != 'seller' then
        raise exception 'It is allowed to sale only for employees with post "seller"! Current post is: %', employee_post;
    end if;

    select *
    from margins
    where start_date <= current_date
      and id_car_model = car_model_id
    order by start_date desc
    limit 1
    into active_margin;

    if active_margin is null then
         raise exception 'Margin for specified auto (ID: %. Does it exist?) and current date (%) is missing', auto, current_date;
   end if;

    auto_cost = auto_cost * (1 + active_margin.value / 100.0);

    update autos
    set status = 'sold'
    where id_auto = auto;

    insert into sales(id_auto, id_client, id_employee, total_sum)
    values (auto, client, seller, auto_cost);
end;
$$;

create or replace function set_margin(car_model_id integer, begins_act_from date, margin_value numeric) returns void
    language plpgsql
as
$$
declare
    margin_row record;
begin
    select *
    from margins
    where id_car_model = car_model_id
      and start_date >= begins_act_from
    order by start_date desc
    limit 1
    into margin_row;

    if margin_row is not null then
        raise exception 'There is already margin for such car model with earlier date (%)!', margin_row.start_date;
    end if;

    insert into margins (id_car_model, start_date, value)
    values (car_model_id, begins_act_from, margin_value);
end;
$$;
""";
            migrationBuilder.Sql(sql);
            
            migrationBuilder.RenameColumn(
                name: "margin",
                table: "margins",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "id_trim",
                table: "autos",
                newName: "id_car_model");

            migrationBuilder.RenameIndex(
                name: "IX_autos_id_trim",
                table: "autos",
                newName: "IX_autos_id_car_model");

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "id_client", "birthdate", "birthplace", "first_name", "last_name", "middle_name", "department_code", "passport_issuer", "passport_number", "passport_series" },
                values: new object[] { 1, new DateOnly(1999, 1, 1), "г. Москва, Московская обл., г.о. Подольск", "Olga", "Chernaya", null, "123-123", "МП МРО по России Московской области г.о. Подольск", "1321", "852852" });

            migrationBuilder.UpdateData(
                table: "detail_series",
                keyColumn: "id_detail_series",
                keyValue: 1,
                columns: new[] { "code", "description" },
                values: new object[] { "HAVAL Horizon 250", "Fully completed and assembled automobile" });

            migrationBuilder.InsertData(
                table: "detail_series",
                columns: new[] { "id_detail_series", "code", "description" },
                values: new object[] { 2, "CHR-81l", "Round car headlight (light)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id_client",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "detail_series",
                keyColumn: "id_detail_series",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "value",
                table: "margins",
                newName: "margin");

            migrationBuilder.RenameColumn(
                name: "id_car_model",
                table: "autos",
                newName: "id_trim");

            migrationBuilder.RenameIndex(
                name: "IX_autos_id_car_model",
                table: "autos",
                newName: "IX_autos_id_trim");

            migrationBuilder.UpdateData(
                table: "detail_series",
                keyColumn: "id_detail_series",
                keyValue: 1,
                columns: new[] { "code", "description" },
                values: new object[] { "SDH-242-790.1", "Full completed and assembled automobile" });
        }
    }
}
