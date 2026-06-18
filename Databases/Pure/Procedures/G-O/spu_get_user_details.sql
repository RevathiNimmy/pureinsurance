/*
	Created By		: Vidya Rangdale
	Creation Date		: 18 Jun 2014
	Description		: This procedure will select all items from pmuser table on the basis of username
	Test Code		: EXEC 'spu_get_user_details 'xyx','12/10/2013'

*/

EXEC DDLDROPPROCEDURE 'spu_get_user_details'
GO

CREATE PROCEDURE spu_get_user_details
	@username varchar(255) = NULL,
	@effective_date datetime = NULL

AS

BEGIN

	SELECT	user_id, 
			language_id, 
			username, 
			password, 
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
            is_temp_password,
			secure_password
			   
FROM pmuser WHERE (username = @username and isnull(@username,'')<>'' )or (isnull(@username,'')='')
AND (is_deleted = 0 AND (@effective_date  >= effective_date and isnull(@effective_date,'')<>'')or (isnull(@effective_date,'')=''))

END
 
GO
