drop schema if exists public cascade;
create schema if not exists public;


-- schema

create type Post as enum
    ('DatabaseAdmin', 'AssemblyChief', 'PurchaseSpecialist', 'Storekeeper', 'Seller', 'Tester');

create type RequestStatus as enum
    ('Sent', 'InHandling', 'Closed');

create type AutoStatus as enum
    ('InAssembly', 'ReadyToTest', 'InTest', 'ReadyToSale', 'Sold');

create type TestStatus as enum
    ('NotChecked', 'Certified', 'Defective');

create type LogType as enum ('Error', 'Normal');

create table logs
(
    id       serial primary key,
    log_time timestamp not null default (now()),
    log_type LogType   not null default ('Normal'::LogType),
    log_text text      not null
);

create table lines
(
    id_line serial,
    name    text not null,

    constraint pk_lines primary key (id_line),
    constraint uq_lines_name unique (name)
);

create table models
(
    id_model serial,
    id_line  int  not null,
    name     text not null,

    constraint pk_models primary key (id_model),
    constraint uq_models_name unique (name),
    constraint fk_models_lines foreign key (id_line) references lines (id_line)
);

create table trims
(
    id_trim  serial,
    id_model int  not null,
    code     text not null,

    constraint pk_trims primary key (id_trim),
    constraint uq_trims_code unique (code),
    constraint fk_trims_models foreign key (id_model) references models (id_model)
);

create table margins
(
    id_trim    int,
    start_date date,
    margin     decimal not null default (10),

    constraint pk_margins primary key (id_trim, start_date),
    constraint fk_margins_trims foreign key (id_trim) references trims (id_trim),
    constraint ch_margins_margin check ( margin > 0 )
);

create table detail_series
(
    id_detail_series serial,
    code             text not null,
    description      text,

    constraint pk_detail_series primary key (id_detail_series),
    constraint uq_detail_series_code unique (code)
);

create table trim_details
(
    id_trim          int,
    id_detail_series int,
    count            int not null,

    constraint pk_trim_details primary key (id_trim, id_detail_series),
    constraint fk_trim_details_trims foreign key (id_trim) references trims (id_trim),
    constraint fk_trim_details_detail_series foreign key (id_detail_series) references detail_series (id_detail_series),
    constraint ch_trim_details_count check ( count > 0 )
);

create table employees
(
    id_employee     serial,
    first_name      text not null,
    last_name       text not null,
    middle_name     text null,
    passport_series text not null,
    passport_number text not null,
    post            post not null,

    constraint pk_employee primary key (id_employee)
);

create table users
(
    id_employee int,
    login       text not null,
    password    text not null,
    deleted     bool not null default (false),

    constraint pk_users_employees primary key (id_employee),
    constraint fk_users_employees foreign key (id_employee) references employees (id_employee),
    constraint uq_users_login unique (login)
);

create table purchase_requests
(
    id_purchase_requests serial,
    id_user              int           null,
    sent_date            timestamp     not null,
    expected_supply_date date          not null,
    status               RequestStatus not null default ('Sent'::RequestStatus),

    constraint pk_purchase_requests primary key (id_purchase_requests),
    constraint fk_purchase_requests_users foreign key (id_user) references users (id_employee) on delete set null
);

create table purchase_request_details
(
    id_purchase_request int,
    id_detail_series    int,
    count               int not null default (1),

    constraint pk_purchase_request_details primary key (id_purchase_request, id_detail_series),
    constraint fk_purchase_request_details_purchase_requests foreign key (id_purchase_request) references purchase_requests (id_purchase_requests),
    constraint fk_purchase_request_details_detail_series foreign key (id_detail_series) references detail_series (id_detail_series),
    constraint ch_purchase_request_details_count check ( count > 0 )
);

create table suppliers
(
    id_supplier           serial,
    legal_address         text not null,
    postal_address        text not null,
    correspondent_account text not null,
    settlement_account    text not null,
    TIN                   text not null,

    constraint pk_suppliers primary key (id_supplier)
);

create table contracts
(
    id_contract            serial,
    id_employee            int,
    id_supplier            int,
    id_purchase_request    int     null,
    conclusion_date        date    not null default (now()),
    supply_date            date    not null,
    total_sum              decimal not null,
    lading_bill_issue_date date    null,

    constraint pk_contracts primary key (id_contract),
    constraint fk_contracts_suppliers foreign key (id_supplier) references suppliers (id_supplier),
    constraint fk_contracts_employees foreign key (id_employee) references employees (id_employee),
    constraint fk_contracts_purchase_requests foreign key (id_purchase_request) references purchase_requests (id_purchase_requests),
    constraint ch_contracts_total_sum check ( total_sum > 0 )
);

create table contract_details
(
    id_contract      int,
    id_detail_series int,
    count            int     not null default (1),
    cost_per_one     decimal not null,

    constraint pk_contract_details primary key (id_contract, id_detail_series),
    constraint fk_contract_details_contracts foreign key (id_contract) references contracts (id_contract),
    constraint fk_contract_details_detail_series foreign key (id_detail_series) references detail_series (id_detail_series),
    constraint ch_contract_details_count check ( count > 0 ),
    constraint ch_contract_details_cost_per_one check ( cost_per_one > 0 )
);

create table autos
(
    id_auto       serial,
    id_trim       int        not null,
    assembly_date date       null,
    cost          decimal    null,
    status        AutoStatus not null default ('InAssembly'::AutoStatus),

    constraint pk_autos primary key (id_auto),
    constraint fk_autos_trims foreign key (id_trim) references trims (id_trim),
    constraint ch_autos_cost check ( cost > 0 )
);

create table details
(
    id_detail_series int,
    id_detail        serial,
    id_contract      int     not null,
    id_auto          int     null,
    cost             decimal not null,

    constraint pk_details primary key (id_detail_series, id_detail),
    constraint fk_details_contracts foreign key (id_contract) references contracts (id_contract) on update restrict on delete restrict,
    constraint fk_details_detail_series foreign key (id_detail_series) references detail_series (id_detail_series) on update restrict on delete restrict,
    constraint fk_details_autos foreign key (id_auto) references autos (id_auto) on delete set null,
    constraint ch_details_cost check ( cost > 0 )
);

create table work_plans
(
    id_work_plan    serial,
    conclusion_date date not null default (now()),
    work_start_date date not null,
    work_end_date   date not null,

    constraint pk_work_plans primary key (id_work_plan)
);

create table works
(
    id_work_plan int,
    id_auto      int,
    name         text not null,
    description  text null,


    constraint pk_works primary key (id_work_plan, id_auto),
    constraint fk_works_work_plans foreign key (id_work_plan) references work_plans (id_work_plan),
    constraint fk_works_autos foreign key (id_auto) references autos (id_auto) on delete set null
);

create table tests
(
    id_test     serial,
    id_employee int  null,
    start_date  date not null,
    end_date    date not null,

    constraint pk_tests primary key (id_test),
    constraint fk_tests_employees foreign key (id_employee) references employees (id_employee) on delete set null
);

create table test_autos
(
    id_test            int,
    id_auto            int,
    certification_date date       not null,
    status             TestStatus not null default ('NotChecked'::TestStatus),

    constraint pk_test_autos primary key (id_test, id_auto),
    constraint fk_test_autos_tests foreign key (id_test) references tests (id_test),
    constraint fk_test_autos_autos foreign key (id_auto) references autos (id_auto)
);

create table clients
(
    id_client       serial,
    first_name      text not null,
    last_name       text not null,
    middle_name     text null,
    birthdate       date not null,
    birthplace      text not null,
    passport_series text not null,
    passport_number text not null,
    passport_issuer text not null,
    department_code text not null,

    constraint pk_clients primary key (id_client),
    constraint ch_clients_birthdate check ( birthdate < (current_date - interval '18 years') )
);

create table sales
(
    id_auto        int,
    execution_date timestamp default (now()),
    id_client      int     not null,
    id_employee    int     not null,
    total_sum      decimal not null,

    constraint pk_sales primary key (id_auto, execution_date),
    constraint fk_sales_autos foreign key (id_auto) references autos (id_auto) on update restrict on delete restrict,
    constraint fk_sales_clients foreign key (id_client) references clients (id_client) on update restrict on delete restrict,
    constraint fk_sales_employees foreign key (id_employee) references employees (id_employee) on delete set null
);

-- schema

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

-- triggers

create or replace function tr_log_contracts_ins() returns trigger
    language plpgsql
as
$$
declare
    value text;
begin
    value = 'INSERT IN contracts (id = ' || new.id_contract || '): ' ||
            'supplier = ' || new.id_supplier || '; total sum of details = ' || new.total_sum ||
            '; supply date = ' || new.supply_date || '; employee = ' || new.id_employee;
    perform log(value);
    return new;
end;
$$;

create or replace function tr_log_contracts_upd() returns trigger
    language plpgsql
as
$$
declare
    value text;
begin
    value = 'UPDATE IN contracts (id = ' || new.id_contract
                || '):  lading bill issued on = ' || new.lading_bill_issue_date;
    perform log(value);
    return new;
end;
$$;

create or replace function tr_log_sales_ins() returns trigger
    language plpgsql
as
$$
declare
    value text;
begin
    value = 'INSERT IN sales: ' ||
            'auto = ' || new.id_auto || '; doc execution time = ' || new.execution_date || ': ' ||
            'total sum = ' || new.total_sum || '; client = ' || new.id_client;
    perform log(value);
    return new;
end;
$$;

create or replace function tr_log_sales_del() returns trigger
    language plpgsql
as
$$
declare
    value text;
begin
    value = 'DELETE IN sales: ' ||
            'auto = ' || old.id_auto || '; doc execution time = ' || old.execution_date || ': ' ||
            'total sum = ' || old.total_sum || '; client = ' || old.id_client;
    perform log(value);
    return old;
end;
$$;

create or replace trigger log_insert_contract
    before insert
    on contracts
    for each row
execute function tr_log_contracts_ins();

create or replace trigger log_update_contract
    before update of lading_bill_issue_date
    on contracts
    for each row
    when ( old.lading_bill_issue_date is null
        and old.lading_bill_issue_date is distinct from new.lading_bill_issue_date )
execute function tr_log_contracts_upd();

create or replace trigger log_insert_sale
    before insert
    on sales
    for each row
execute function tr_log_sales_ins();

create or replace trigger log_delete_sale
    after delete
    on sales
    for each row
execute function tr_log_sales_del();

-- triggers