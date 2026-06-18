SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_shortname'
GO

CREATE PROCEDURE spu_CLM_Get_shortname  
  
@party_cnt int  
  
AS  
  
  
Select shortname from party where party_cnt = @party_cnt  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
