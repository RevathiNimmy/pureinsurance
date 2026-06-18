SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_Arrangement_Line_Del_RI2007'
GO


CREATE PROCEDURE spu_RI_Arrangement_Line_Del_RI2007  
@ri_arrangement_line_id INT  
      
AS    
    
 DECLARE @Type VARCHAR(2) 
--Insert deleted RI lines in archive tables during clone amendment
IF Exists(Select null from RI_Arrangement_Line_Archive where ri_arrangement_id =
	(select ri_arrangement_id from RI_Arrangement_Line where ri_arrangement_line_id =@ri_arrangement_line_id))
BEGIN
Insert Into RI_Arrangement_line_Broker_Participants_Archive  
 (ri_arrangement_line_id,  
 ri_party_cnt,  
 participation_percent)  
   SELECT ri_arrangement_line_id,  
 ri_party_cnt,  
 participation_percent FROM RI_Arrangement_line_Broker_Participants  
 WHERE ri_arrangement_line_id =@ri_arrangement_line_id
  
       
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
 Is_Obligatory)  
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
 Is_Obligatory  
 FROM RI_Arrangement_Line  
     WHERE ri_arrangement_line_id =@ri_arrangement_line_id

END

  
SELECT @Type = [type]  
FROM ri_arrangement_line  
WHERE ri_arrangement_line_id = @ri_arrangement_line_id  
  
IF @Type <>'FX'  
BEGIN  
 DELETE FROM RI_Arrangement_line_Broker_Participants  
 WHERE ri_arrangement_line_id = @ri_arrangement_line_id  
  
 DELETE FROM ri_arrangement_line  
 WHERE ri_arrangement_line_id = @ri_arrangement_line_id  
END  
ELSE IF @Type ='FX'  
 BEGIN  
  DELETE FROM RI_Arrangement_line_Broker_Participants WHERE Ri_arrangement_line_id In  
  ( SELECT Ri_arrangement_line_id FROM ri_arrangement_line WHERE Grouping=@ri_arrangement_line_id)  
  
  DELETE FROM ri_arrangement_line  
  WHERE grouping = @ri_arrangement_line_id  
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
