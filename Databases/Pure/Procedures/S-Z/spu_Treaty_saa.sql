SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Treaty_saa'
GO

CREATE PROCEDURE spu_Treaty_saa
@IgnoreDelete INT=0,
@RIArrangementKey bigint = 0,
@Type varchar(2) = ''

AS
	 DECLARE @ri_band INT ,
	 @Date_for_Treaty_XOL_Calculation INT ,
	 @Date_for_Prop_Calculation INT,
	 @cover_start_date_ForRi DATETIME,
	 @rk_inception_Date DATETIME,
	 @risk_inception_Date DATETIME,
	 @insurance_cover_start_date DATETIME,
	 @insurance_file_cnt BIGINT,
	 @risk_cnt BIGINT,
	 @ri_band_id BIGINT

	 -- INITIALISE REQUIRED VALUES
	 SELECT @risk_cnt = risk_cnt , @ri_band_id= ri_band_id FROM RI_Arrangement 
	 WHERE ri_arrangement_id = @RIArrangementKey

	 SELECT  TOP (1) @insurance_file_cnt= insurance_file_cnt FROM insurance_file_risk_link where  risk_cnt = @risk_cnt 
	 
	 select @cover_start_date_ForRi = iff.inception_date_tpi ,
	 @risk_inception_Date= rk.inception_date,
	 @insurance_cover_start_date = iff.cover_start_date
	 from RI_Arrangement_Line ral
	 left join RI_Arrangement ra on ra.ri_arrangement_id = ral.ri_arrangement_id 
	 left join risk rk on rk.risk_cnt = ra.risk_cnt
	 left join insurance_file_risk_link ifrl on ifrl.risk_cnt = ra.risk_cnt
	 left join Insurance_File iff on iff.insurance_file_cnt = ifrl.insurance_file_cnt
	 left join Product p on p.product_id = iff.product_id
	 WHERE iff.insurance_file_cnt = @insurance_file_cnt

	   -- SELECT THE XOL CALCULATION AND PROP VALUES 
		SELECT TOP 1 @Date_for_Treaty_XOL_Calculation = ISNULL(@Date_for_Treaty_XOL_Calculation,date_for_treaty_xol_calculation_id),                
		             @Date_for_Prop_Calculation= ISNULL(@Date_for_Prop_Calculation,Proportional_RI_Cal_Method)
		FROM   RI_Band_Version                
		WHERE  ri_band_id = @ri_band_id              
		AND CONVERT(DATE, effective_date, 23)  <= CONVERT(DATE, @cover_start_date_ForRi, 23)          
		ORDER BY effective_date DESC
	-- END OF INITIALIZATION

		SELECT  DISTINCt t.treaty_id,
                t.code,  
                t.description,  
                t.is_deleted,  
                t.effective_date,  
                t.expiry_date,  
                t.agreement_code,  
                t.reinsurance_type_id,  
                rt.description,  
                t.replaces_treaty_id,  
                r.description,  
    	    	t.replaced_by_effective_date,  
                t.replaced_by_treaty_id  ,
			t.treaty_limit,
			t.currency_id,
			t.reinstatements	,
			Ltrim(rtrim(rt.code)) reinsurance_code
        From    Treaty t  
        Left Join  Reinsurance_Type rt  On rt.reinsurance_type_id = t.reinsurance_type_id  
        left Join  Treaty r  On r.treaty_id = t.replaces_treaty_id  
		WHERE (   ( (rt.code IN ('QUO', '001', '002', '003') AND @type = 'T'
						ANd ( ( @Date_for_Prop_Calculation = 1 AND CONVERT(DATE,@risk_inception_Date,23) >= CONVERT(DATE,t.effective_date,23))
							OR 
							(@Date_for_Prop_Calculation = 2 
								AND CASE WHEN GETDATE() > CONVERT(DATE,@insurance_cover_start_date,23) THEN GETDATE()
								    ELSE CONVERT(DATE,@insurance_cover_start_date,23) END >= CONVERT(DATE,t.effective_date,23)))
					) 
					 
				OR (rt.code IN ('XOL')  AND @type = 'TX'
					ANd (  (@Date_for_Treaty_XOL_Calculation = 1 AND CONVERT(DATE,@risk_inception_Date,23) >= CONVERT(DATE,t.effective_date,23))
							OR 
							(@Date_for_Treaty_XOL_Calculation = 2 and CONVERT(DATE,GETDATE(),23) >= CONVERT(DATE,t.effective_date,23))
							OR
							(@Date_for_Treaty_XOL_Calculation = 3 and CONVERT(DATE,@insurance_cover_start_date,23) >=CONVERT(DATE,t.effective_date,23))
							)
					)
				)
					AND ISNULL(T.is_deleted, 0) = 0
					AND @Type <> ''
			)
		OR @IgnoreDelete=0 
		OR @Type = ''
	    
        Order By  
            t.code  

GO