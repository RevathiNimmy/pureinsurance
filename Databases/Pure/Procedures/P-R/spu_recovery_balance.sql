SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_recovery_balance'
GO

CREATE PROCEDURE spu_recovery_balance    
    @claim_id int,    
    @is_salvage tinyint    
AS    
    
    -- Balance all reserves on reserve tables for given claim    
    UPDATE  wr    
    SET     revision_count = ISNULL(revision_count, 0) + 1,    
            revised_reserve = received_to_date - initial_reserve    
    FROM    recovery wr    
    JOIN    recovery_type rt    
            ON rt.recovery_type_id = wr.recovery_type_id    
    JOIN    claim_peril wcp    
            ON wcp.claim_peril_id = wr.claim_peril_id    
    WHERE   wcp.claim_id = @claim_id    
    AND     rt.is_salvage = @is_salvage    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
