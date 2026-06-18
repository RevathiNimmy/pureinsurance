-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    12 Oct 2003
--  Desc:    SFB 1.8.6 Accident Management development
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Party_Extra_ups'
GO

CREATE PROCEDURE spu_Party_Extra_ups
(
	@party_cnt      int,
    @agency_number  varchar(255)
)
AS 

-- perform an upsert ie. if the record doesn't exist, insert it - if it does exist, update it
IF EXISTS (SELECT 
               party_cnt,
               agency_number
           FROM 
               Party_Extra
           WHERE
               party_cnt = @party_cnt)
BEGIN

    UPDATE
        Party_Extra
    SET
        agency_number = @agency_number
    WHERE
        party_cnt     = @party_cnt

END ELSE BEGIN

    INSERT INTO
        Party_Extra
        (
        party_cnt,
        agency_number
        )
    VALUES
        (
        @party_cnt,
        @agency_number        
        )

END




