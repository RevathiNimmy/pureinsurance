-- AK22072004: Create spu_GIS_Branch_Scheme_Upd.sql
-- Updates branch usage data for GII schemes. If scheme doesn't exist, inserts
-- a new record

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Branch_Scheme_Upd'
GO

CREATE PROCEDURE spu_GIS_Branch_Scheme_Upd
    @SourceID integer,
    @SchemeID integer,
    @PMCoNum integer,
    @AgencyCode varchar(30),
    @RelationshipStatus char(1),
    @EDIMailBox varchar(13)
AS

-- Check for an existing record
IF EXISTS (SELECT source_id 
             FROM gis_branch_scheme 
            WHERE source_id = @SourceID
              AND gis_scheme_id = @SchemeID) BEGIN

    -- Record found, so update it
    UPDATE GIS_Branch_Scheme
       SET pm_company_number = @PMCoNum, 
           agency_code = @AgencyCode,
           relationship_status = @RelationshipStatus,
	   EDI_Mail_Box = @EDIMailBox
     WHERE source_id = @SourceID
       AND gis_scheme_id = @SchemeID

END ELSE BEGIN

    -- No match found, insert a new record
    INSERT INTO GIS_Branch_Scheme (source_id,
                gis_scheme_id,
                pm_company_number,
                agency_code,
                relationship_status,
		EDI_Mail_Box)
        VALUES (@SourceID, 
                @SchemeID,
                @PMCoNum, 
                @AgencyCode,
                @RelationshipStatus,
		@EDIMailBox)

END

GO
