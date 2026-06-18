SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_previous_accidents_upd'
GO

CREATE PROCEDURE spu_SAM_previous_accidents_upd  
    @party_cnt int,    
    @previous_accidents_id int,    
    @Date datetime,    
    @Description varchar(70),    
    @is_at_fault tinyint    
AS    
    
BEGIN    
UPDATE previous_accidents   
SET party_cnt = @Party_cnt,    
    [Date] = @Date,    
    [Description] = @Description,    
    is_at_fault = @is_at_fault  
WHERE previous_accidents_id = previous_accidents_id
AND party_cnt = @party_cnt
END    

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
