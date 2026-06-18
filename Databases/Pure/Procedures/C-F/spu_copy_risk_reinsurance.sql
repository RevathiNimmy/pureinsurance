SET QUOTED_IDENTIfIER OFF SET ANSI_NULLS On
GO


Execute DDLDropProcedure 'spu_copy_risk_reinsurance'
GO


CREATE PROCEDURE spu_copy_risk_reinsurance  
    @old_risk_cnt int,  
    @new_risk_cnt int  
AS  

    Return

    -- This procedure needs to be either rewritten appropriately or dropped completely.
    -- It's previous version copied and pro-rated (according to currency changes) the old 
    -- reinsurance and stored it, temporarily, as the new reinsurance.
    --
    -- Even if this is intended it is incorrect. The values are likely to be wrong as
    -- those on the risk change, the RI model are not necessarily correct and thus
    -- the treaties are likely to be wrong. 


GO


