SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_previous_accidents_del'
GO

CREATE PROCEDURE spe_previous_accidents_del


@party_cnt int,
@previous_accidents_id int
    
AS    
    
    
DELETE FROM previous_accidents    
WHERE previous_accidents_id = @previous_accidents_id    
AND party_cnt =@party_cnt    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
