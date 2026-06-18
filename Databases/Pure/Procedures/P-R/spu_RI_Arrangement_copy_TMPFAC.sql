SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_Arrangement_copy_TMPFAC'
GO


CREATE PROCEDURE spu_RI_Arrangement_copy_TMPFAC  
    @old_risk_cnt INT,  
    @new_ri_arrangement_id INT  
AS  
BEGIN  
DECLARE @ri_arrangement_line_id INT,  
@new_ri_arrangement_line_id INT,  
@grouping INT,  
@line_limit MONEY,  
@lower_limit MONEY,  
@ri_band_id INT  
  
SELECT @ri_band_id=ri_band_id FROM ri_arrangement WHERE ri_arrangement_id=@new_ri_arrangement_id  
  
	if not exists(select * From RI_Arrangement_line_Broker_Participants_Archive RIBrAr
					  Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id
					  Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI
																inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt
																inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt
																Where ri.risk_cnt = @old_risk_cnt and ifl.insurance_file_type_id in (2,5,8,9)))
		Begin
			Insert Into RI_Arrangement_line_Broker_Participants_Archive  
						(ri_arrangement_line_id,  
						ri_party_cnt,  
						participation_percent)  
			SELECT 	RIBrAr.ri_arrangement_line_id,  
					RIBrAr.ri_party_cnt,  
					RIBrAr.participation_percent FROM RI_Arrangement_line_Broker_Participants RIBrAr 
			Inner Join RI_Arrangement_Line  ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id
			WHERE RIBrAr.ri_arrangement_line_id IN
			(
			SELECT ri_arrangement_line_id FROM ri_arrangement_line WHERE type in('F','FX')  and ri_arrangement_id=@new_ri_arrangement_id
			)
		End
DELETE FROM Ri_Arrangement_line_Broker_Participants WHERE ri_arrangement_line_id IN  
(  
SELECT ri_arrangement_line_id FROM ri_arrangement_line WHERE type in('F','FX')  and ri_arrangement_id=@new_ri_arrangement_id  
)  
  
    if not exists(select * From ri_arrangement_Line_Archive RIALAr
                  Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id
                  Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI
														 inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt
														 inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt
														 Where ri.risk_cnt = @old_risk_cnt and ifl.insurance_file_type_id in (2,5,8,9)))
		Begin
			Insert Into  RI_Arrangement_Line_Archive  
			 (ri_arrangement_line_id,  
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
			 retained,  
			 lower_limit,  
			 participation_percent,  
			 grouping,  
			 ri_model_line_id,  
			 Is_Obligatory,
			 manually_added)  
			 SELECT ri_arrangement_line_id,  
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
			 retained,  
			 lower_limit,  
			 participation_percent,  
			 grouping,  
			 ri_model_line_id,  
			 Is_Obligatory  ,
			 manually_added
			 FROM RI_Arrangement_Line 
			 WHERE type in('F','FX') and ri_arrangement_id=@new_ri_arrangement_id
		End
		DELETE FROM ri_arrangement_line WHERE type in('F','FX') and ri_arrangement_id=@new_ri_arrangement_id
  IF OBJECT_ID(N'tempdb..#RITEMPTABLE', N'U') IS NOT NULL 
	DROP TABLE #RITEMPTABLE
  
	CREATE TABLE #RITEMPTABLE (
		ROW_ID INT IDENTITY Primary Key,
		ri_arrangement_line_id INT, 
		ri_grouping INT,
		line_limit MONEY,
		lower_limit MONEY,
		new_ri_arrangement_id INT,
		ral_type varchar(3), 
		ral_treaty_id INT, 
		ral_party_cnt INT,
		ral_default_share_percent FLOAT,
		ral_this_share_percent FLOAT,
		ral_premium_percent FLOAT,
		ral_commission_percent FLOAT,
		ral_agreement_code varchar(255),
		ral_priority INT,
		ral_number_of_lines smallint,
		ral_line_limit money,
        ral_lower_limit MONEY, 
		ral_sum_insured MONEY,
		ral_premium_value MONEY, 
		ral_commission_value MONEY,
		ral_premium_tax MONEY,
		ral_commission_tax MONEY,
		ral_is_commission_modified TINYINT,
		Retained FLOAT,  
		ral_is_obligatory TINYINT,
		ral_participation_percent FLOAT,
		ral_grouping INT,
		ral_ri_model_line_id INT,
		ral_manually_added bit,
		ral_FACPropPremiumPerc FLOAT
	)

	INSERT INTO #RITEMPTABLE (	
		ri_arrangement_line_id,
		ri_grouping,
		line_limit,
		lower_limit,
		new_ri_arrangement_id,
		ral_type,
		ral_treaty_id, ral_party_cnt, ral_default_share_percent,
		ral_this_share_percent, ral_premium_percent, ral_commission_percent, ral_agreement_code, ral_priority, ral_number_of_lines,  ral_line_limit,  
        ral_lower_limit, ral_sum_insured, ral_premium_value, ral_commission_value, ral_premium_tax, ral_commission_tax, ral_is_commission_modified,Retained,  
		ral_is_obligatory, ral_participation_percent, ral_grouping, ral_ri_model_line_id, ral_manually_added, ral_FACPropPremiumPerc
	)
	
	SELECT  ral.ri_arrangement_line_id, [grouping], line_limit, lower_limit, @new_ri_arrangement_id, ral.type, ral.treaty_id, ral.party_cnt, ral.default_share_percent,
			ral.this_share_percent, ral.premium_percent, ral.commission_percent,  ral.agreement_code,  ral.priority,  ral.number_of_lines,  ral.line_limit,  
            ral.lower_limit, ral.sum_insured, ral.premium_value, ral.commission_value, ral.premium_tax, ral.commission_tax, ral.is_commission_modified,Retained,  
			ral.is_obligatory, ral.participation_percent, ral.grouping, ral.ri_model_line_id, ral.manually_added, ral.FACPropPremiumPerc			
       FROM    ri_arrangement_line ral  
       JOIN    ri_Arrangement ra On ra.ri_arrangement_id = ral.ri_arrangement_id  
       WHERE   ra.risk_cnt = @old_risk_cnt AND ra.ri_band_id = @ri_band_id  
       AND version_id = (SELECT MAX(version_id) FROM ri_Arrangement WHERE risk_cnt=@old_risk_cnt AND ri_band_id=@ri_band_id AND ri_arrangement_id<>@new_ri_arrangement_id)
       AND ral.type in ('F','FX')
       AND original_flag = 0       
  
	
   
	DECLARE @NumberRecords INT
		DECLARE @RowCount INT
	   
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1

		WHILE @RowCount <= @NumberRecords
		BEGIN
  
    Insert Into ri_arrangement_line (  
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
      lower_limit,  
      sum_insured,  
      premium_value,  
      commission_value,  
      premium_tax,  
      commission_tax,  
      is_commission_modified,  
      Retained,  
      is_obligatory,  
      participation_percent,  
      grouping,  
      ri_model_line_id, manually_added, FACPropPremiumPerc)  
  
       SELECT 
		@new_ri_arrangement_id,
		ral_type,
		ral_treaty_id, 
		ral_party_cnt,
		ral_default_share_percent,
		ral_this_share_percent, 
		ral_premium_percent,
		ral_commission_percent,
		ral_agreement_code,
		ral_priority, 
		ral_number_of_lines, 
		ral_line_limit, 
		ral_lower_limit, 
		ral_sum_insured,
	    ral_premium_value, 
		ral_commission_value, 
		ral_premium_tax, 
		ral_commission_tax, 
		ral_is_commission_modified,
		Retained,  
		ral_is_obligatory, 
		ral_participation_percent, 
		ral_grouping, 
		ral_ri_model_line_id, ISNULL(ral_manually_added,0), ral_FACPropPremiumPerc
	   FROM #RITEMPTABLE 
	   WHERE ROW_ID = @RowCount
  
       SET @new_ri_arrangement_line_id=@@IDENTITY  
		
	SELECT @ri_arrangement_line_id = ri_arrangement_line_id,
		   @line_limit = ral_line_limit  ,
		   @lower_limit = ral_lower_limit,
		   @grouping = ral_grouping
		FROM #RITEMPTABLE
		WHERE ROW_ID = @RowCount
	  
  --Ri_Arrangement_line_Broker_Participants
  Insert Into Ri_Arrangement_line_Broker_Participants(ri_arrangement_line_id,  
               ri_party_cnt,  
               participation_percent)  
           SELECT  @new_ri_arrangement_line_id,ri_party_cnt,participation_percent From    Ri_Arrangement_line_Broker_Participants  
   Where     ri_arrangement_line_id=@ri_arrangement_line_id  
  
    
  Update RI_Arrangement_Line set grouping = CASE WHEN  ISNull(@grouping,0) = @ri_arrangement_line_id THEN @new_ri_arrangement_line_id  
           ELSE  
            CASE WHEN ISNull(@grouping,0) = 0  THEN @new_ri_arrangement_line_id  
            ELSE  
             Case When ISNULL(@grouping,0) <> @ri_arrangement_line_id  AND ISNULL(@grouping,0) <> 0  
               Then (select Top 1 ri_arrangement_line_id from RI_Arrangement_Line WHERE RI_Arrangement_id = @new_ri_arrangement_id AND line_limit = @line_limit AND lower_limit = @lower_limit )  
             END  
            END  
           END  
  WHERE ri_arrangement_line_id = @new_ri_arrangement_line_id  
  
  SET @RowCount = @RowCount + 1

	END -- End While
  
 
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
