SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Check_For_Existing_Personal_Party'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (6.1.1)
CREATE PROCEDURE spu_SAM_Check_For_Existing_Personal_Party
@Last_Name varchar(255),
@First_Name varchar(60),
@Address_Line_1 varchar(60),
@PostCode varchar(20),
@Date_of_Birth datetime
AS

BEGIN

    SELECT p.Party_Cnt
    FROM    Party p
        INNER JOIN Party_Personal_Client ppc ON
            ppc.Party_Cnt=p.Party_Cnt
        INNER JOIN Party_Address_Usage au ON
            au.Party_Cnt=p.Party_Cnt
        INNER JOIN Address a ON
            a.Address_Cnt=au.Address_Cnt
        INNER JOIN Party_LifeStyle pl ON
            pl.Party_Cnt=p.Party_Cnt
    WHERE   p.Name=@Last_Name
        AND ppc.ForeName=@First_Name
        AND a.Address1=@Address_Line_1
        AND a.Postal_Code=@PostCode
        AND DateDiff(d,pl.Date_of_Birth,@Date_of_Birth)=0
END
--End (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (6.1.1)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
