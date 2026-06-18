EXECUTE DDLDropProcedure 'spu_get_pmuser_source_for_report'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_get_pmuser_source_for_report	
    @nUser_id INTEGER,
	@bIsAllBranches BIT OUTPUT

    AS BEGIN

		DECLARE @count INT	

		SELECT @count = COUNT(pmuser_source.source_id) FROM pmuser_source  
		 INNER JOIN source ON source.source_id = pmuser_source.source_id
		 INNER JOIN pmuser ON pmuser.user_id = pmuser_source.user_id
		 WHERE source.is_deleted = 0 
		 AND pmuser.user_id = @nUser_id
	
		IF @count= 0 --All Branches
		BEGIN
			SET @bIsAllBranches = 1			
		END
			
		ELSE		--Restricted branches only
		BEGIN					
			SET @bIsAllBranches = 0
		END    


    END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
