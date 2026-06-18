SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_partyBG_Branches_Sel'
GO

CREATE PROCEDURE spu_partyBG_Branches_Sel  
@BG_Number INT  
  
As  
BEGIN  
 IF ISNULL(@BG_Number,0) <> 0  
  SELECT  
       	BGBL.Source_Id,
		-- Start - Sankar - Bank Guarantee Bug Fixing
		S.description
		-- End - Sankar - Bank Guarantee Bug Fixing
  FROM BG_branch_link BGBL
	INNER JOIN Source S
		ON S.Source_Id = BGBL.Source_Id
  WHERE BG_id = @BG_Number  
  
END  


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

