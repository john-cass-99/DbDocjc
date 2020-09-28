select event_object_table as table_name,
       trigger_name,
       action_order,
       action_timing,
       event_manipulation as trigger_event,
       action_statement as 'definition'
from information_schema.TRIGGERS 
where event_object_schema='test' and event_object_table='people';