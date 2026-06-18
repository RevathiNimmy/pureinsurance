SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_CheckForRenewedPolicy'
GO

CREATE PROCEDURE spu_SIR_CheckForRenewedPolicy   
@insurance_file_cnt int,
@IsRenewedPolicy tinyint  OUTPUT
  
AS  
BEGIN
Declare @Insurance_folder_cnt INT  
Select @Insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WHERE Insurance_file_cnt = @insurance_file_cnt

IF EXISTS(SELECT null FROM insurance_file WHERE insurance_folder_cnt = @Insurance_folder_cnt AND Insurance_file_cnt < =@insurance_file_cnt 
					AND insurance_file_type_id = 2 AND policy_version >1 AND base_insurance_file_cnt IS NULL)
SELECT @IsRenewedPolicy = 1

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO





