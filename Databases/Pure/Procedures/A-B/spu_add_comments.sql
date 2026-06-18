SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_comments'
GO

CREATE PROCEDURE spu_add_comments  
    @perilid int,  
    @comments varchar(255)  
AS  
  
BEGIN
	UPDATE Claim_Peril  
	SET comments=@comments  
	WHERE claim_peril_id=@perilid  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
