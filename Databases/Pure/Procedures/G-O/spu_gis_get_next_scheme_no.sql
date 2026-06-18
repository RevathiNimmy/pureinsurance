SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_gis_get_next_scheme_no'
GO

CREATE PROCEDURE spu_gis_get_next_scheme_no
    @gis_insurer_id   integer,
    @scheme_no        integer    OUTPUT
    			
AS


IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') BEGIN

	-- Underwriting upgrade code goes here
    SELECT  @scheme_no = ISNULL(MAX(scheme_no), 0)
    FROM    Gis_Scheme 
    WHERE   gis_insurer_id = @gis_insurer_id

END ELSE BEGIN

    -- TR 17/03/04 PN11040 - Gemini need exclusive access to Scheme_nos
    -- 1 to 9 apparently. We can do this whether Gemini is installed or not.
    -- So if this is the first Scheme to be setup for a 
    -- particular Insurer in Broking Schemes, then default max to 
    -- 9 (vb code then increments by 1)
    -- Of course we are assuming here that Gemini does not use this script to determine their next Scheme no
    SELECT  @scheme_no = ISNULL(MAX(scheme_no), 9)
    FROM    Gis_Scheme 
    WHERE   gis_insurer_id = @gis_insurer_id
END
