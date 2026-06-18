SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Insurance_File_is_coinsured'
GO


CREATE PROCEDURE spu_Insurance_File_is_coinsured
    @insurance_file_cnt integer,
    @is_coinsured tinyint output,
    @retained_percent float output
AS

    -- Check to see if this policy employs coinsurance.
    Select  @is_coinsured = Count(*)
    From    insurance_file ifi
    Where   ifi.insurance_file_cnt = @insurance_file_cnt
    And     ifi.business_type_id In (Select business_type_id From business_type Where code in ('COIN LEAD', 'COIN FOLL'))

    
    -- Is it coinsured?
    If @is_coinsured > 0
        --If it is then retrieve the retained share.
        Select  @retained_percent = IsNull(Sum(cv.share_percent), 0) / 100
        From    Coi_Value cv
        Join    Party_Insurer pin
                On  pin.party_cnt = cv.party_cnt
        Where   insurance_file_cnt = @insurance_file_cnt
        And     pin.is_retained = 1
    Else
        -- If not return retained amount as 1 (100%)
        Select  @retained_percent = 1


GO


