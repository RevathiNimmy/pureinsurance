
SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Policy_Numbers_Add_Upd'
GO

CREATE PROCEDURE spu_GIS_Policy_Numbers_Add_Upd
    @NumberRangeID INTEGER,
    @LastUsed VARCHAR(20),
    @RangeStart VARCHAR(20),
    @RangeEnd VARCHAR(20),
    @NewRangeStart VARCHAR(20),
    @NewRangeEnd VARCHAR(20),
    @DeleteCurrent tinyint,
    @Prefix VARCHAR(255),
    @Suffix VARCHAR(255),
    @BusinessClass char(2),
    @CoverNote tinyint,
    @ValidationMask VARCHAR(20),
    @CheckDigit VARCHAR(255),
    @MinAvailableNos INTEGER,
    @NumberFormatId INTEGER 
AS

IF EXISTS(SELECT policy_numbers_id FROM policy_numbers WHERE policy_numbers_id = @NumberRangeID) 
BEGIN
        -- Record found, so update it
	UPDATE Policy_Numbers
	   SET last_used = ISNULL(@LastUsed, last_used),
	       range_start = ISNULL(@RangeStart, range_start),
	       range_end = ISNULL(@RangeEnd, range_end),
	       new_range_start = ISNULL(@NewRangeStart, new_range_start),
	       new_range_end = ISNULL(@NewRangeEnd, new_range_end),
	       delete_current = @DeleteCurrent,
	       min_available_numbers = @MinAvailableNos
	 WHERE policy_numbers_id = @NumberRangeID	    
END 
ELSE 
BEGIN    
        -- No match found, insert a new record
        INSERT INTO Policy_Numbers (
                    policy_numbers_id,
                    last_used,
                    range_start,
                    range_end,
                    new_range_start,
                    new_range_end,
                    delete_current,                     
                    is_covernote_no,                    
                    min_available_numbers)
            VALUES (
                    @NumberRangeID,
                    @LastUsed,
                    @RangeStart,
                    @RangeEnd,
                    @NewRangeStart,
                    @NewRangeEnd,
                    @DeleteCurrent,                    
                    @CoverNote,                    
                    @MinAvailableNos)
       
END

IF EXISTS(SELECT policy_numbers_format_id FROM policy_numbers_format WHERE policy_numbers_format_id = @NumberFormatId) 
BEGIN
    UPDATE Policy_Numbers_Format
    SET prefix = @Prefix,
        suffix = @Suffix,
        validation_mask = @ValidationMask,
        check_digit = @CheckDigit
    WHERE policy_numbers_format_id = @NumberFormatId     
END
ELSE
BEGIN
    INSERT INTO Policy_Numbers_Format (
		policy_numbers_format_id,		
		prefix,
		suffix,			
		validation_mask,
		check_digit)
	VALUES (
		@NumberFormatID,		
		@Prefix,
		@Suffix,		
		@ValidationMask,
		@CheckDigit)
END
 
GO
