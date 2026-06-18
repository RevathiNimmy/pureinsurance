SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_Party_ShortName'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc)
CREATE PROCEDURE spu_SAM_Get_Party_ShortName
@Party_Cnt int
AS

BEGIN

    SELECT Party.ShortName
    FROM    Party
    WHERE   Party.Party_Cnt=@Party_Cnt

END
--End (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
