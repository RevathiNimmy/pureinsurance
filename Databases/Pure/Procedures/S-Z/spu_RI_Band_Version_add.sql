
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_RI_Band_Version_add'
GO
CREATE PROCEDURE spu_RI_Band_Version_add  
        @RI_Band_id int,
	@code char(10),  
      --@caption_id int,  
        @description varchar(255),  
        @effective_date datetime,   
	@Date_for_Treaty_XOL_Calculation_id int,
	@XOL_Treaty_To_Recover_From_id int,
	@Proportional_RI_Cal_Method int,
	@use_anniversary_date_for_TMP tinyint,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL
AS  
BEGIN
   

   BEGIN
       DECLARE  @caption_id int
       -- Ensure correct caption id  
       EXEC spu_pm_caption_id_return 1, @description, @caption_id Output 

      INSERT INTO RI_Band_Version 
	  (
	    code
		,caption_id
		,description
		,effective_date
		,ri_band_id
		,Date_for_Treaty_XOL_Calculation_id
		,XOL_Treaty_To_Recover_From_id
		,Proportional_RI_Cal_Method
		,use_anniversary_date_for_TMP
		,userid
		,uniqueid
		,screenhierarchy
	  )
	  VALUES
	  (
	         @code
		,@caption_id
		,@description		
		,@effective_date
		,@ri_band_id
		,@Date_for_Treaty_XOL_Calculation_id
		,@XOL_Treaty_To_Recover_From_id
		,@Proportional_RI_Cal_Method
		,@use_anniversary_date_for_TMP
	        ,@userid
		,@uniqueid
		,@screenhierarchy + '/' + @description + '/' + CAST(convert(date,@effective_date,103) AS VARCHAR)
	  )
   END

END

GO