DDLDropProcedure 'spu_GetPolicyRenewalStatus'
Go

CREATE PROCEDURE spu_GetPolicyRenewalStatus
						@RenewalStatusCnt int
AS

BEGIN
	SELECT 	rst.renewal_status_type_id, 
			rst.code, 
			rst.description
	FROM 	Renewal_Status rs JOIN Renewal_Status_Type rst ON rs.renewal_status_type_id = rst.renewal_status_type_id
	WHERE	rs.renewal_status_cnt = @RenewalStatusCnt
	
END
