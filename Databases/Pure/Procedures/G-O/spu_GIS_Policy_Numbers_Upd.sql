--  spu_GIS_Policy_Numbers_Upd.sql
-- Updates a number range record to the Policy_Numbers table

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Policy_Numbers_Upd'
GO

CREATE PROCEDURE spu_GIS_Policy_Numbers_Upd
    @NumberRangeID integer OUTPUT,
    @QMInsRef varchar(3),
    @InsurerID integer,
    @SchemeID integer,
    @LastUsed varchar(20),
    @RangeStart varchar(20),
    @RangeEnd varchar(20),
    @NewRangeStart varchar(20),
    @NewRangeEnd varchar(20),
    @DeleteCurrent tinyint,
    @Prefix varchar(255),
    @Suffix varchar(255),
    @BusinessClass char(2),
    @CoverNote tinyint,
    @ValidationMask varchar(20),
    @CheckDigit varchar(255),
    @MinAvailableNos integer
AS

DECLARE @lID integer

-- Check if a valid scheme ID was supplied
IF @SchemeID <> 0 BEGIN

    --Check for an existing scheme-specific record
    SELECT @lID = pn.policy_numbers_id 
        FROM Policy_Numbers pn,
	       gis_scheme_policy_numbers gspn
        WHERE gspn.gis_scheme_id = @SchemeID 
	AND gspn.policy_numbers_id = pn.policy_numbers_id
	AND pn.is_covernote_no = @CoverNote
    
    IF @lID IS NOT NULL BEGIN
    
        -- Record found, so update it
        UPDATE Policy_Numbers
           SET last_used = @LastUsed,
               range_start = @RangeStart,
               range_end = @RangeEnd,
               new_range_start = @NewRangeStart,
               new_range_end = @NewRangeEnd,
               delete_current = @DeleteCurrent,               
               min_available_numbers = @MinAvailableNos
         WHERE policy_numbers_id = @lID
        
        -- Return the record ID
        SET @NumberRangeID = @lID
    
    END ELSE BEGIN   
        RETURN(1)    
    END

END ELSE BEGIN
    RETURN(1)
END

GO
