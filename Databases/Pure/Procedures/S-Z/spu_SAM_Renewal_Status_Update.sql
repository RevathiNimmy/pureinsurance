SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Renewal_Status_Update'
GO

CREATE PROCEDURE spu_SAM_Renewal_Status_Update

@insurance_file_cnt int, 
@renewal_status_type_id int, 
@is_invite_printed int = NULL

AS

IF @is_invite_printed = NULL
	UPDATE renewal_status
	SET renewal_status_type_id = @renewal_status_type_id
	WHERE renewal_insurance_file_cnt = @insurance_file_cnt
ElSE
	UPDATE renewal_status
	SET renewal_status_type_id = @renewal_status_type_id,
	is_invite_printed = @is_invite_printed
	WHERE renewal_insurance_file_cnt = @insurance_file_cnt


GO
