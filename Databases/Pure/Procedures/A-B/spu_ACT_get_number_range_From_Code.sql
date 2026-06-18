
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO
EXEC DDLDropProcedure 'spu_ACT_get_number_range_From_Code'
GO
--Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
CREATE   PROCEDURE spu_ACT_get_number_range_From_Code  
    @RangeCode char(10),  
    @NumberRangeID int OUTPUT  
AS  
  
BEGIN  
  
    SELECT  
  @NumberRangeID = ACTnumber_range_id  
 FROM  
  ACTNumber_Range  
    WHERE  
     code = @RangeCode  
  
END  
--End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
