SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Apply'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Apply              
    @insurance_file_cnt int,              
    @discount_percentage numeric(11,8)              
AS              
              
-- ******************************************************************************************************              
-- Stored Procedure spu_SIR_Policy_Discount_Apply              
-- ******************************************************************************************************              
-- Revision             Description of Modification                                     Date        Who              
-- --------             ---------------------------                                     ----        ---              
-- 1.0                  Created (Save Original Ratings Premium and Update Premium with     
--                      required discount percentage                                    02-12-2005  MEvans        
-- 2.0                  Changed to calculate discount on net premium (original + this)
--                      per rating section so MTA discount is correct                   2026        AI-Assisted
-- ******************************************************************************************************              
              
Declare @Rating_section_type_id INT
Declare @Risk_cnt INT 
Declare @this_premium MONEY
Declare @RoundingAmount MONEY
Declare @net_premium MONEY
Declare @discount_amount MONEY
              
BEGIN              
    
    -- Save original this_premium before discount and calculate discount on NET premium
    -- Net premium = sum of all lines (original_flag 0 + original_flag 1) per rating section
    -- For NB: no original lines exist, so net = this_premium (same result as before)
    -- For MTA: net = original(-1500) + this(1600) = 100, discount applied to 100
    UPDATE rsec              
     SET rsec.discount_original_this_premium = rsec.this_premium,      
         rsec.this_premium = ISNULL(rsec.this_premium,0) + 
            ((ISNULL(net.net_premium,0) * @discount_percentage)/100)
    FROM Risk rsk              
        JOIN insurance_file_risk_link ifrl              
            ON rsk.risk_cnt=ifrl.risk_cnt              
        JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES rs              
            ON rs.risk_status_id=rsk.risk_status_id              
        JOIN rating_section rsec              
            ON rsk.risk_cnt=rsec.risk_cnt              
        JOIN (
            -- Calculate net premium per risk + rating_section_type (sum of all lines)
            SELECT rs2.risk_cnt, rs2.rating_section_type_id, 
                   SUM(ISNULL(rs2.this_premium,0)) AS net_premium
            FROM rating_section rs2
			JOIN insurance_file_risk_link ifrl2
            ON ifrl2.risk_cnt = rs2.risk_cnt
            WHERE ifrl2.insurance_file_cnt = @insurance_file_cnt
            GROUP BY rs2.risk_cnt, rs2.rating_section_type_id
        ) net ON rsec.risk_cnt = net.risk_cnt 
             AND rsec.rating_section_type_id = net.rating_section_type_id
    WHERE ifrl.insurance_file_cnt= @insurance_file_cnt              
    AND rsk.is_risk_selected = 1   
    AND rsec.original_flag = 0   
    AND ISNULL(ifrl.is_risk_edited,0)=1                      
    

	Declare cur_Risk Cursor FOR 
		SELECT  R.Risk_cnt 
		FROM Risk R
		JOIN insurance_file_risk_link ifrl  
		ON R.risk_cnt=ifrl.risk_cnt  
		WHERE ifrl.insurance_file_cnt= @insurance_file_cnt  
		AND R.is_risk_selected = 1  
	    AND ISNULL(ifrl.is_risk_edited,0)=1

	OPEN cur_Risk 	
	FETCH NEXT FROM cur_Risk INTO @Risk_cnt 

	WHILE @@FETCH_STATUS = 0 
	BEGIN
			Select @Rating_section_type_id = rs.rating_section_type_id
			From 
				Insurance_file F
			JOIN insurance_file_risk_link ifrl 
				ON F.insurance_file_cnt = ifrl.insurance_file_cnt
			JOIN RISK R
				ON R.risk_cnt=ifrl.risk_cnt  
			JOIN Rating_Section RS  
				ON R.risk_cnt=RS.risk_cnt 
			JOIN rating_section_type rst 
				ON RS.rating_section_type_id = RST.rating_section_type_id
			JOIN Product P 
				ON F.Product_id= P.Product_id
			    AND p.rounding_section_id= Rs.rating_section_type_id
			WHERE R.Risk_Cnt= @Risk_cnt
			AND P.Rounding_section_id > 0
			AND R.is_risk_selected = 1  
			AND RS.original_flag = 0
	
		IF @Rating_section_type_id > 0 
		BEGIN
			Select @this_premium =Sum(This_Premium) FROM rating_section 
			WHERE Risk_cnt = @risk_cnt AND rating_section_type_id <> @Rating_section_type_id
			AND original_flag = 0

			IF ISNULL(@this_premium,0)=0
				Set @RoundingAmount=0
			ELSE
				BEGIN  
					Set @RoundingAmount = -(@this_premium - Round(@this_premium + (@this_premium / Abs(@this_premium) * 0.0001), 0))  
				END

			Update Rating_section SET this_premium = @RoundingAmount, Annual_premium = @RoundingAmount
			WHERE risk_cnt= @Risk_cnt AND rating_section_type_id = @Rating_section_type_id AND original_flag = 0
				
		END
	 		FETCH NEXT FROM cur_Risk INTO @Risk_cnt 
	END

END              

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
