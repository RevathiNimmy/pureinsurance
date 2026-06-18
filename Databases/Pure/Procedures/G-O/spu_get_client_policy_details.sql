SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_client_policy_details'
GO


CREATE PROCEDURE spu_get_client_policy_details
    @insurance_file_cnt INT
AS

SELECT p.party_cnt,
	   p.shortname,
       i.insurance_folder_cnt, 
       i.insurance_ref,
       i.renewal_date,
       i.policy_type_id
FROM party p, insurance_file i
WHERE insurance_file_cnt = @insurance_file_cnt
AND i.insured_cnt = p.party_cnt

GO


