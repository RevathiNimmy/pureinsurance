SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDROPPROCEDURE 'spu_get_latest_policy_version_for_Renewal'
GO

CREATE PROCEDURE spu_get_latest_policy_version_for_Renewal
@nInsurance_folder_cnt INT
AS
SELECT TOP 1 insurance_file_cnt FROM insurance_file
WHERE insurance_folder_cnt = @nInsurance_folder_cnt
AND insurance_file_status_id IS NULL 
AND insurance_file_type_id IN (2,5,9)
AND ISNULL(out_of_sequence_replaced,0)<>1
ORDER by cover_start_date DESC
GO

