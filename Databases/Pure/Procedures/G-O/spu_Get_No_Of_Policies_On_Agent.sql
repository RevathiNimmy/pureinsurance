SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_No_Of_Policies_On_Agent'
GO
CREATE PROCEDURE spu_Get_No_Of_Policies_On_Agent  
    @lead_agent_cnt INT  
AS 
SELECT COUNT(*) FROM insurance_file WHERE insurance_file_cnt IN
(SELECT MAX(insurance_file_cnt) FROM insurance_file WHERE lead_agent_cnt=@lead_agent_cnt
	AND insurance_file_type_id in (2,5,8,9) GROUP BY insurance_ref) 
		AND insurance_file_type_id in (2,5,9) 

Go
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO    