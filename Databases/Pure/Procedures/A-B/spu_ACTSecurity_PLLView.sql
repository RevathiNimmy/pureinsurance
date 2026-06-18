SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_ACTSecurity_PLLView
GO

CREATE PROCEDURE spu_ACTSecurity_PLLView
    @node_id INT  
AS  
BEGIN
    SELECT
        P.pmuser_group_id,
        P.code,  
        CASE  
            WHEN (
                  PP.pmuser_group_id IS NULL
                  OR PP.Has_unrestricted_enquiry = 0
                  OR PP.Has_unrestricted_update = 1
                 ) 
                 THEN 0
            ELSE 
                 1
        END Chosen  
    FROM  
        PMUser_Group P
        LEFT OUTER JOIN PMUser_Group_Authorities PP  
            ON P.pmuser_group_id = PP.pmuser_group_id 
            AND PP.node_id = @node_id
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
