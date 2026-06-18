SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Claim_Tab_Update'
GO

CREATE PROCEDURE [spu_Claim_Tab_Update] 

@Claim_Tab_ID INT,
@Caption VARCHAR(20),
@Display_Order  TINYINT,
@Risk_Or_Peril Tinyint

AS

UPDATE Claim_Tab
SET Caption = @Caption, Display_Order = @Display_Order,Risk_Or_Peril = @Risk_Or_Peril
WHERE Claim_Tab_ID = @Claim_Tab_ID

RETURN @@ROWCOUNT






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

