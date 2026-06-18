SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Agent_Group_From_Agent_Key
GO

 --Start (Prakash C Varghese) - (Agent Group Association) 
CREATE  PROCEDURE spu_SAM_Get_Agent_Group_From_Agent_Key   
    @agent_cnt INT ,
    @agent_group_cnt INT OUTPUT   
AS    
BEGIN    
    SET @agent_group_cnt=0

    SELECT   
        @agent_group_cnt=pag.party_cnt
    FROM   
        party_agent_group pag  
        INNER JOIN party_agent pa  
            ON pa.linked_account_group=pag.party_cnt  
    WHERE  
        pa.party_cnt=@agent_cnt  
        AND pag.active=1  
END 
 --End (Prakash C Varghese) - (Agent Group Association) 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO