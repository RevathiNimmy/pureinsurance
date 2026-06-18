SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_RI_Band_Version_saa'
GO
CREATE PROCEDURE spu_RI_Band_Version_saa    
     @RI_Band_id int    
AS    
BEGIN  
    SELECT RBV.ri_band_id RIBandId, RBV.code, ISNULL(RBV.caption_id,0) CaptionID ,
	RBV.description, rbv.effective_date, 
	ISNULL(DTC.Description,'') DateforTreaty,   
    ISNULL(XTR.Description,'') XOLTreatyToRecover,
	ISNULL(PTC.description,'') ProportionalRICalculation,   
    ISNULL(rbv.use_anniversary_date_for_TMP,'') UseAnniversaryDate,  
    ISNULL(RBV.Date_for_Treaty_XOL_Calculation_id,0) DTI_Id,  
    ISNULL(RBV.XOL_Treaty_To_Recover_From_id,0) XOL_Id,  
    ISNULL(RBV.Proportional_RI_Cal_Method,0) PropRICal_Id
FROM    RI_Band_Version RBV    
    LEFT JOIN Date_for_Treaty_XOL_Calculation DTC ON DTC.Date_for_Treaty_XOL_Calculation_id = RBV.Date_for_Treaty_XOL_Calculation_id  
    LEFT JOIN XOL_Treaty_To_Recover_From XTR ON XTR.XOL_Treaty_To_Recover_From_id = RBV.XOL_Treaty_To_Recover_From_id  
 LEFT JOIN Proportional_RI_Calculation_Method PTC ON PTC.Proportional_RI_Calculation_Method_id = RBV.Proportional_RI_Cal_Method  
    WHERE   RBV.ri_band_id = @RI_Band_id    
    ORDER BY RBV.effective_date DESC    
  
END  
  

