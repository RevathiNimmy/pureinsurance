SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Account_Details_For_InsuranceFile'
GO

CREATE PROCEDURE spu_Get_Account_Details_For_InsuranceFile
@insurance_file_cnt INT
AS
    SELECT intermediary_agent_account_id FROM Insurance_File WHERE insurance_file_cnt=@insurance_file_cnt 
GO
