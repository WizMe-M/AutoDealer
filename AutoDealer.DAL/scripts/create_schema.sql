-- CREATE DATABASE AutoDealer;

drop schema if exists public cascade;
create schema public;

create type post as enum
    ('database_admin', 'assembly_chief', 'purchase_specialist', 'storekeeper', 'seller', 'tester');

create type request_status as enum
    ('sent', 'im_handling', 'closed');

create type auto_status as enum
    ('in_assembly', 'ready_to_test', 'in_test', 'ready_to_sale', 'sold');

create type test_status as enum
    ('not_checked', 'certified', 'defective');

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
    id_user              int            null,
    sent_date            timestamp      not null,
    expected_supply_date date           not null,
    status               request_status not null default ('sent'::request_status),

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
    id_trim       int         not null,
    assembly_date date        null,
    cost          decimal     null,
    status        auto_status not null default ('in_assembly'::auto_status),

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
    certification_date date        not null,
    status             test_status not null default ('not_checked'::test_status),

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