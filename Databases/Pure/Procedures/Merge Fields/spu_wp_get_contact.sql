SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_get_contact'
GO

CREATE PROCEDURE spu_wp_get_contact
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @contact_code VARCHAR(10),
    @area_code VARCHAR(10) OUTPUT,
    @number VARCHAR(255) OUTPUT,
    @extension VARCHAR(6) OUTPUT
AS

SELECT @area_code = NULL,
    @number = NULL,
    @extension = NULL

SELECT @area_code = c.area_code,
    @number = c.number,
    @extension = c.extension
    FROM party_contact_usage pcu,
    contact c,
    contact_type ct
    WHERE pcu.party_cnt = @PartyCnt
    AND pcu.contact_cnt = c.contact_cnt
    AND c.contact_type_id = ct.contact_type_id
    AND ct.code = @contact_code

GO

