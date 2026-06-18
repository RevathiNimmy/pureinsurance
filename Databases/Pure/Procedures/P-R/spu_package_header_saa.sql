SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Package_Header_saa'
GO

CREATE PROCEDURE spu_Package_Header_saa
    @Package_Header_Id int,
    @Party_cnt int,
    @is_deleted tinyint
AS

    IF @Package_Header_Id <> 0
        SELECT  
            Package_Header_Id,
            Package_Code	 ,	
            Package_Description,
            Party_cnt	,
            Policy_Start_Date,
            Policy_End_Date	,	
            Policy_Effective_Date,
            Policy_Holder_Name,
            Policy_Handler_cnt,	
            Policy_Agent_cnt,	
            Policy_Branch	,	
            Policy_Payment_Method,
            Policy_Renewal_Frequency,
            Package_Notes	,
            Modified_Date	,
            Effective_Date	,
            Is_deleted	
        FROM
            Package_Header
        WHERE
            Package_Header_Id = @Package_Header_Id AND (is_deleted = 0 or is_deleted = @is_deleted)
        ORDER BY 
            Package_Code ASC
    ELSE
        SELECT  
            Package_Header_Id,
            Package_Code	 ,	
            Package_Description,
            Party_cnt	,
            Policy_Start_Date,
            Policy_End_Date	,	
            Policy_Effective_Date,
            Policy_Holder_Name,
            Policy_Handler_cnt,	
            Policy_Agent_cnt,	
            Policy_Branch	,	
            Policy_Payment_Method,
            Policy_Renewal_Frequency,
            Package_Notes	,
            Modified_Date	,
            Effective_Date	,
            Is_deleted	
        FROM
            Package_Header
        WHERE
            Party_cnt = @Party_cnt AND (is_deleted = 0 or is_deleted = @is_deleted)
        ORDER BY 
            Package_Code ASC

GO


