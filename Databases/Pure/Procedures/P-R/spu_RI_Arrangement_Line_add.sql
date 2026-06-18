SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Arrangement_Line_add'
GO


CREATE PROCEDURE spu_RI_Arrangement_Line_add
    @ri_arrangement_line_id int output,
    @ri_arrangement_id int,
    @type varchar(2),
    @treaty_id int,
    @party_cnt int,
    @default_share_percent float,
    @this_share_percent float,
    @premium_percent float,
    @commission_percent float,
    @agreement_code varchar(255),
    @priority int,
    @number_of_lines DECIMAL(10,2),
    @line_limit money,
    @sum_insured money,
    @premium_value money,
    @commission_value money,
    @premium_tax money,
    @commission_tax money,
    @is_commission_modified tinyint,
    @lower_limit money = 0,    
    @retained_percentage float = 0,
    --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
    @participation_percent float = 0,  
    @is_obligatory TINYINT = 0 , 
    --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
	@fac_prop_premium_percent  float = 0  ,
	@manually_added bit = 0,
	@treaty_code varchar(100) = '',
	@is_edited bit = 0,
	@is_premium_edited bit = 0
AS

		if @treaty_code <>'' and @treaty_code <>'0' AND @type <> 'F' AND @type  <> 'FX'
		begin
			set @treaty_id = ISNULL((select TOP (1) treaty_id from treaty where code = @treaty_code ORDER BY treaty_id), 0)
		end
		else if  @type = 'F' or @type  = 'FX'
		begin
		set @treaty_id = null
		end
		
    -- Insert record
    Insert  ri_arrangement_line (
            ri_arrangement_id,
            type,
            treaty_id,
            party_cnt,
            default_share_percent,
            this_share_percent,
            premium_percent,
            commission_percent,
            agreement_code,
            priority,
            number_of_lines,
            line_limit,
            sum_insured,
            premium_value,
            commission_value,
            premium_tax,
            commission_tax,
            is_commission_modified,  
            lower_limit,  
            retained,
	    --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
            participation_percent,  
	    is_obligatory,
		FACPropPremiumPerc,
		manually_added,
		is_edited,
		is_premium_edited
		)  
	    --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
    Values (@ri_arrangement_id,
            @type,
            @treaty_id,
            @party_cnt,
            @default_share_percent * 100,
            @this_share_percent * 100,
            @premium_percent * 100,
            @commission_percent * 100,
            @agreement_code,
            @priority,
            @number_of_lines,
            @line_limit,
            @sum_insured,
            @premium_value,
            @commission_value,
            @premium_tax,
            @commission_tax,
            @is_commission_modified,  
	    @lower_limit,  
            @retained_percentage,
	    --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
            @participation_percent,  
	    @is_obligatory,
		@fac_prop_premium_percent * 100,
		@manually_added,
		@is_edited,
		@is_premium_edited
		)  
	    --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  

    -- Get new ID
    Select @ri_arrangement_line_id = @@Identity
   

Go


