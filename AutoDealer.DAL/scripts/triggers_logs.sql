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