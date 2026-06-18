

EXECUTE DDLDropProcedure 'spu_Check_Alternate_Logon'
GO

CREATE PROCEDURE spu_Check_Alternate_Logon
    @username varchar(100),
    @effective_date VARCHAR(100)
AS
SELECT 
user_id, 
language_id, 
username, 
secure_password, 
password_change_date, 
date_created, 
lastlogin, 
logged_on_at_client, 
party_cnt, 
is_pmb_link_required, 
server_printer, 
is_printer_changeable, 
is_deleted, 
effective_date, 
email_address,
full_name, 
signature_file, 
title, 
initials, 
telephone_number, 
extension_number, 
mobile_number, 
fax_number, 
job_title_id, 
claim_handler_id, 
party_handler_id, 
other_party_id, 
alternative_identifier, 
job_basis, 
percent_hours_worked, 
sirius_user, 
date_deleted, 
user_config_xml_dataset,
is_locked,
incorrect_attempt_count,
is_temp_password  , 
password,
sso_preferred_username
FROM pmuser 
WHERE alternative_identifier = @username
AND (is_deleted = 0 AND @effective_date >= effective_date)
GO


