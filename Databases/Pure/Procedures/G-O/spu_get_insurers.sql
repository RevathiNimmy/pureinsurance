SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_insurers'
GO


CREATE PROCEDURE spu_get_insurers
    @insurance_file_cnt INT,
    @from_event BIT,
    @lead_insurer_cnt INT
AS

IF @from_event = 1
BEGIN
    /*Get accounts from event tables*/
    IF EXISTS
        (
            SELECT NULL
            FROM party p
            WHERE p.party_cnt = @lead_insurer_cnt
            AND UPPER(SUBSTRING(p.shortname,1,5)) = 'MULTI'
            AND ISNUMERIC('0' + SUBSTRING(p.shortname,6,5)) = 1
        )
    BEGIN
        /*Get each co-insurer*/
        SELECT p.party_cnt, p.shortname, p.name 
        FROM party p
        JOIN event_policy_coinsurers pc
            ON pc.party_cnt = p.party_cnt
        WHERE p.is_deleted <> 1  
        AND insurance_file_cnt = @insurance_file_cnt
        ORDER BY shortname
    END
    ELSE
    BEGIN
        /*Get just the main insurer*/
        SELECT p.party_cnt, p.shortname, p.name 
        FROM party p
        WHERE p.is_deleted <> 1  
        AND p.party_cnt = @lead_insurer_cnt
        ORDER BY shortname
    END
END
ELSE
BEGIN
    /*Get accounts from normal tables*/
    IF EXISTS
        (
            SELECT NULL
            FROM party p
            WHERE p.party_cnt = @lead_insurer_cnt
            AND UPPER(SUBSTRING(p.shortname,1,5)) = 'MULTI'
            AND ISNUMERIC('0' + SUBSTRING(p.shortname,6,5)) = 1
        )
    BEGIN
        /*Get each co-insurer*/
        SELECT p.party_cnt, p.shortname, p.name 
        FROM party p
        JOIN policy_coinsurers pc
            ON pc.party_cnt = p.party_cnt
        WHERE p.is_deleted <> 1  
        AND insurance_file_cnt = @insurance_file_cnt
        ORDER BY shortname
    END
    ELSE
    BEGIN
        /*Get just the main insurer*/
        SELECT p.party_cnt, p.shortname, p.name 
        FROM party p
        WHERE p.is_deleted <> 1  
        AND p.party_cnt = @lead_insurer_cnt
        ORDER BY shortname
    END
END

GO