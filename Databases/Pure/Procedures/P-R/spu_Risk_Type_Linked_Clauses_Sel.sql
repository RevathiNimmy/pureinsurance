SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Risk_Type_Linked_Clauses_Sel'
GO
CREATE PROCEDURE spu_Risk_Type_Linked_Clauses_Sel   
    @risk_type_id int,    
    @code varchar(10)=NULL,    
    @gis_property_id int = NULL,    
    @gis_object_id int = NULL,    
    @effective_date datetime = NULL,    
    @Branch_ID int=NULL,    
--Start Arul -Bug Fixing PN 55217  
    @Default tinyint=NULL,  
    @PropertyName Varchar(70)=NULL,  
    @ColumnName Varchar(70)=NULL,
    @is_visible_from_client_manager varchar(10)=NULL   
   
AS  
  
--CJB 02/08/2005 PN22739 Cater for broking which does not use wording_risk_type_link table  
--RKS 29/04/2005 Added gis_property_id  
--RKS 30/05/2005 Added gis_object_id, ignored record filtering on document_filter column if the  
--               special_type_reference is blank  
  
-- Peter Finney 09/04/2003  
-- Check for blank code  
  
DECLARE @document_filter varchar(50)  
  
--Start Arul -Bug Fixing PN 55217  
DECLARE @Specials_Type_Reference  Varchar(50)  
   
IF  NOT ISNULL(@PropertyName,'')=''  and NOT ISNULL(@ColumnName,'')=''  
Begin  
 SELECT @Specials_Type_Reference=Specials_Type_Reference FROM gis_property   
 WHERE property_name=@PropertyName and column_name=@ColumnName  AND Isnull(Specials_Type_Reference,'')<>'' AND  gis_object_id = @gis_object_id
End  
  
--End Arul -Bug Fixing PN 55217  
  
IF @effective_date IS NULL OR @effective_date IN (CONVERT(DATETIME,'1899-12-30',120), CONVERT(DATETIME,'1899-01-01',120))    
    SET @effective_date = GETDATE()    
    
--Underwriting    
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')    
  BEGIN    
 IF(ISNULL(@Default,' ')=' ')    
          BEGIN    
          SELECT  distinct  dt.document_template_id,    
                 dt.code,    
                 dt.description,    
                 dt.is_deleted,    
                 ty.document_type_id,    
                 ty.code,    
                 ty.description,    
                 dt.effective_date    
          FROM    document_template dt    
          JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
          JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id    
          WHERE   wrtl.risk_type_id = @risk_type_id  and wrtl.branch_id=@branch_id    
          AND     dt.effective_date 
				  IN 
					( SELECT MAX(effective_date) 
					  FROM document_template WHERE document_template.code = dt.code AND 
					                        (CONVERT(DATETIME,(CONVERT(VARCHAR,document_template.effective_date,106))) < = @effective_date)	AND is_deleted=0
					)
		  AND dt.is_deleted=0
   
    	  AND (ISNULL(@Specials_Type_Reference,'')='' OR dt.document_filter = @Specials_Type_Reference)    
	      AND (ISNULL(@code, '') = '' OR dt.code like @code )    
          ORDER BY dt.code    
          END    
        ELSE    
            BEGIN    
        SELECT   distinct  dt.document_template_id,    
                 dt.code,    
                  dt.description,    
                 dt.is_deleted,    
                 ty.document_type_id,    
                 ty.code,    
                 ty.description,    
                 dt.effective_date    
          FROM    document_template dt    
          JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
          JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id    
          WHERE   wrtl.risk_type_id = @risk_type_id  and wrtl.branch_id=@branch_id    
          AND wrtl.[default]=@Default    
          AND     dt.effective_date 
				  IN 
					( SELECT MAX(effective_date) 
					  FROM document_template WHERE document_template.code = dt.code AND 
					                        (CONVERT(DATETIME,(CONVERT(VARCHAR,document_template.effective_date,106))) < = @effective_date)	AND is_deleted=0
					)
		  AND dt.is_deleted=0    
 		AND (ISNULL(@Specials_Type_Reference,'')='' OR dt.document_filter = @Specials_Type_Reference)  
          ORDER BY dt.code    
    
     END    
    END    
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.1)    
--Broking     
ELSE    
BEGIN    
IF ISNULL(@code, ' ') = ' '    
    IF ISNULL(@gis_property_id,' ')=' ' OR ISNULL(@gis_object_id,' ')=' '    
        SELECT  dt.document_template_id,    
                dt.code,    
                dt.description,    
                ty.document_type_id,    
                ty.code,    
                dt.is_deleted,    
                ty.description    
        FROM    document_template dt    
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
        ORDER BY dt.code    
    ELSE    
    BEGIN    
        SELECT @document_filter=(SELECT gisp.specials_type_reference FROM GIS_Property gisp    
            WHERE gisp.gis_property_id=@gis_property_id AND gisp.gis_object_id=@gis_object_id)    
    
        SELECT  dt.document_template_id,    
                dt.code,    
                dt.description,    
                ty.document_type_id,    
                ty.code,    
                dt.is_deleted,    
                ty.description    
        FROM    document_template dt    
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
        WHERE   ISNULL(dt.document_filter,'')= ISNULL(@document_filter,'')    
        AND (dt.copy_of_original<>1 OR dt.copy_of_original IS NULL)    
        ORDER BY dt.code    
    END    
ELSE    
    IF ISNULL(@gis_property_id,' ')=' ' OR ISNULL(@gis_object_id,' ')=' '    
        SELECT  dt.document_template_id,    
                dt.code,    
                dt.description,    
                ty.document_type_id,    
                ty.code,    
                dt.is_deleted,    
                ty.description    
     FROM    document_template dt    
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
        WHERE   dt.code LIKE @code    
        ORDER BY dt.code    
    ELSE    
    BEGIN    
        SELECT @document_filter=(SELECT gisp.specials_type_reference FROM GIS_Property gisp    
            WHERE gisp.gis_property_id=@gis_property_id AND gisp.gis_object_id=@gis_object_id)    
    
        SELECT  dt.document_template_id,    
                dt.code,    
                dt.description,    
                ty.document_type_id,    
                ty.code,    
                dt.is_deleted,    
                ty.description    
        FROM    document_template dt    
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
        WHERE   dt.code LIKE @code    
        AND ISNULL(dt.document_filter,'')= ISNULL(@document_filter,'')    
        AND (dt.copy_of_original<>1 OR dt.copy_of_original IS NULL)    
        ORDER BY dt.code    
    END    




END    
  
 GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 