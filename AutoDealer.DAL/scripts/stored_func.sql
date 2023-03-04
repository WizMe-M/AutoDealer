-- log functions

create or replace function log(type LogType, value text) returns void
    language plpgsql
as
$$
begin
    insert into logs (log_type, log_text)
    values (type, value);
end;
$$;

create or replace function log(value text) returns void
    language plpgsql
as
$$
begin
    insert into logs (log_text)
    values (value);
end;
$$;

-- log functions


-- normal functions

create or replace function sell_auto(
    auto int,
    client int,
    employee int
) returns void
    language plpgsql
as
$$
declare
    auto_cost     decimal;
    real_status   AutoStatus;
    trim_id       int;
    active_margin record;
begin

    select cost, status, id_trim
    into auto_cost, real_status, trim_id
    from autos
    where id_auto = auto;

    if real_status != 'ReadyToSale' then
        raise exception 'It is available to sale only "ready to sale" autos! Current status is: %', real_status;
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
    set status = 'Sold'
    where id_auto = auto;

    insert into sales(id_auto, id_client, id_employee, total_sum)
    values (auto, client, employee, auto_cost);
end;
$$;

create or replace function return_auto(
    auto int,
    sale_time timestamp
) returns void
    language plpgsql
as
$$
declare
    returning_sale record;
begin
    select *
    from sales
    where id_auto = auto
      and execution_date = sale_time
    limit 1
    into returning_sale;

    update autos
    set status = 'ReadyToSale'
    where id_auto = auto;

    delete
    from sales
    where id_auto = auto
      and execution_date = sale_time;
end;
$$;

create or replace function set_margin(
    trim_id int,
    begins_act_from date,
    margin_value decimal
)
    returns void
    language plpgsql
as
$$
declare
    margin_row record;
begin
    for margin_row in
        select *
        from margins
        loop
            if margin_row.id_trim = trim_id and margin_row.start_date >= begins_act_from then
                raise exception 'There is already margin for such trim with earlier date (%)!', margin_row.start_date;
            end if;
        end loop;

    if margin_row is null then
        raise notice 'no rows';
    end if;

    insert into margins (id_trim, start_date, margin)
    values (trim_id, begins_act_from, margin_value);
end;
$$;

create or replace function assembly_auto(
    auto int
) returns void
    language plpgsql
as
$$
declare
    auto_cost   decimal := 0;
    trim_id     int;
    real_status AutoStatus;
    detail_row  record;
begin

    select status, id_trim
    into real_status, trim_id
    from autos
    where id_auto = auto;

    if real_status != 'InAssembly' then
        raise exception 'It is available to finish assembly only "in_assembly" autos! Current status is: %', real_status;
    end if;

    for detail_row in
        select d.id_detail, ds.code, ds.description, d.cost
        from details as d
                 join detail_series as ds on ds.id_detail_series = d.id_detail_series
        where d.id_auto = auto
        loop
            raise notice 'detail (id `%`) code=`%`, description=`%`, cost=`%`',
                detail_row.id_detail, detail_row.code, detail_row.description, detail_row.cost;
            auto_cost = auto_cost + detail_row.cost;
        end loop;

    update autos
    set cost          = auto_cost,
        assembly_date = current_date,
        status        = 'ReadyToTest'
    where id_auto = auto;
end;
$$;

create or replace function process_lading_bill(
    contract int
) returns void
    language plpgsql
as
$$
declare
    detail_in_contract record;
begin

    for detail_in_contract in
        select cd.id_detail_series as detail_series,
               cd.cost_per_one     as cost_per_one,
               cd.count            as count
        from contract_details as cd
        where id_contract = contract
        loop
            for i in 1..detail_in_contract.count
                loop
                    insert into details (id_detail_series, cost, id_contract)
                    values (detail_in_contract.detail_series, detail_in_contract.cost_per_one, contract);
                end loop;
        end loop;

    update contracts
    set lading_bill_issue_date = current_date
    where id_contract = contract;

end;
$$;

-- normal functions