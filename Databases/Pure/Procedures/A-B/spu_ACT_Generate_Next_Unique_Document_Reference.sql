SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO
EXEC DDLDropProcedure 'spu_ACT_Generate_Next_Unique_Document_Reference'
GO

CREATE  PROCEDURE spu_ACT_Generate_Next_Unique_Document_Reference  
    @number_range_id INT,  
    @user_id SMALLINT,  
    @company_id SMALLINT,  
    @document_reference VARCHAR(25) OUTPUT  
AS  
  
/*
    Modified By	Date			 Description
  
    GHarris		30 Sep 2014	 Reformatted and added error handling to roll back the transaction
    RTaylor		14 Oct 2014	 Removed all code to generate ref using locking and the ACTUnique_Document_Number table
    GHarris		15 Oct 2014	 Changed @@Identity to Scope_Identity for safety purposes
    Deepak      11 Feb 2016  Changed the Number Length if the Series length is increased to @DefaultNumberLength + 1
*/
  BEGIN  
  
    DECLARE @NextACTNumber  AS INT
    DECLARE @NumberLength   AS TINYINT
    DECLARE @DefaultNumberLength AS TINYINT 
    DECLARE @CurrentNumberLength AS TINYINT
  
    SET @DefaultNumberLength=8  
    SET @NumberLength = @DefaultNumberLength
    INSERT INTO ACTNumber
    (
          actnumber_id ,  
          actnumber_range_id ,  
	   [user_id] ,
	   company_id
	   )
    VALUES 
    (
	   0,
       @number_range_id,  
       @user_id,  
	   @company_id
    )
  
    SELECT @NextACTNumber = SCOPE_IDENTITY()
  
    SET @CurrentNumberLength=LEN(CONVERT(VARCHAR,@NextACTNumber))  
    IF @CurrentNumberLength > @DefaultNumberLength  
        SET @NumberLength = @CurrentNumberLength
  
    IF @CurrentNumberLength=@NumberLength AND @NextACTNumber =CONVERT(INT,REPLICATE('9',@NumberLength))  
    BEGIN   
        SET @NumberLength=@NumberLength+1  
        SET @NextACTNumber = 0  
END  

    SET @document_reference=REPLICATE('0',@NumberLength-LEN(CONVERT(VARCHAR,@NextACTNumber))) + CONVERT(VARCHAR,@NextACTNumber)


END
SET QUOTED_IDENTIFIER OFF

