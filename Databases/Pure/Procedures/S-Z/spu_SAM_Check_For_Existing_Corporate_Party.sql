SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Check_For_Existing_Corporate_Party'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (6.1.2)
CREATE PROCEDURE spu_SAM_Check_For_Existing_Corporate_Party
@Trading_Name varchar(255),
@Address_Line_1 varchar(60),
@PostCode varchar(20)
AS

BEGIN

    SELECT p.Party_Cnt
    FROM    Party p
        INNER JOIN Party_Corporate_Client pcc ON
            pcc.Party_Cnt=p.Party_Cnt
        INNER JOIN Party_Address_Usage au ON
            au.Party_Cnt=p.Party_Cnt
        INNER JOIN Address a ON
            a.Address_Cnt=au.Address_Cnt
    WHERE   p.Name=@Trading_Name
        AND a.Address1=@Address_Line_1
END
--End (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (6.1.2)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
