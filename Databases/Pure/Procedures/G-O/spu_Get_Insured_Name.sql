SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_Insured_Name'
GO


CREATE PROCEDURE spu_Get_Insured_Name
    @Party_Cnt int
AS


DECLARE @party_type AS VARCHAR(10)

DECLARE @ICCS VARCHAR(4)

SELECT  @party_type = pt.code
FROM    party p,
    party_type pt
WHERE   p.party_cnt = @party_cnt
AND p.party_type_id = pt.party_type_id

EXEC    spu_pm_iccs @ICCS OUTPUT

IF @party_type = 'PC' AND @ICCS = '5604'
    BEGIN
        SELECT  case isnull(pc.party_title_code, '')
            when '' then ''
            else pc.party_title_code + ' '
            end
            + case isnull(pc.forename, '')
            when '' then ''
            else pc.forename + ' '
            end
            + isnull(p.name, '')
        FROM    party p,
            party_personal_client pc
        WHERE   p.party_cnt = @Party_Cnt
        AND p.party_cnt = pc.party_cnt
    END
ELSE
    BEGIN
        SELECT  CASE ISNULL(p.resolved_name, '')
            WHEN '' THEN name
            ELSE    p.resolved_name
            END
        FROM    party p
        WHERE   p.party_cnt = @party_cnt
    END
GO


