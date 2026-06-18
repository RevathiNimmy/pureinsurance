SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Claim_Tab_Add'
GO


CREATE PROCEDURE [spu_Claim_Tab_Add] 

@Caption VARCHAR(20),
@Display_Order  TINYINT,
@Risk_Or_Peril TINYINT

AS

INSERT INTO Claim_Tab(Caption, Display_Order, Risk_Or_Peril)
VALUES (@Caption, @Display_Order, @Risk_Or_Peril)

RETURN @@IDENTITY






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

