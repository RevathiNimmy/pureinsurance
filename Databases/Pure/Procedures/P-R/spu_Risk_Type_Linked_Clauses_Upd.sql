
--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.3)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Risk_Type_Linked_Clauses_Upd'
GO 
CREATE PROCEDURE spu_Risk_Type_Linked_Clauses_Upd  
    @risk_type_id int,  
    @code varchar(10)=NULL ,  
    @gis_property_id int = NULL,  
    @gis_object_id int = NULL,  
    @effective_date datetime = NULL,
    @document_template_id int = NULL
  
AS  
  
DECLARE @document_filter varchar(50)  
  
IF @effective_date IS NULL OR @effective_date ='1899-12-30'  
    SET @effective_date = GETDATE()  
  
--Underwriting  
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')  
BEGIN  
IF ISNULL(@code, ' ') = ' '  
    IF ISNULL(@gis_property_id,' ')=' ' OR ISNULL(@gis_object_id,' ')=' '  
        SELECT  dt.document_template_id,  
                dt.code,  
                dt.description,  
                wrtl.[default]  ,  
                wrtl.branch_id,  
                ty.document_type_id,  
                ty.code,  
                dt.is_deleted,  
                ty.description,  
                dt.effective_date  
        FROM    document_template dt  
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id  
        JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id  
        WHERE   wrtl.risk_type_id = @risk_type_id and dt.is_deleted=0 and dt.effective_date <getdate() 
          AND   (@document_template_id is null or dt.document_template_id = @document_template_id)  
        ORDER BY dt.code  
    ELSE  
    BEGIN  
        SELECT @document_filter=(SELECT gisp.specials_type_reference FROM GIS_Property gisp  
            WHERE gisp.gis_property_id=@gis_property_id AND gisp.gis_object_id=@gis_object_id)  
  
        SELECT  dt.document_template_id,  
                dt.code,  
                dt.description,  
                wrtl.[default]  ,  
                wrtl.branch_id,  
                ty.document_type_id,  
                ty.code,  
                dt.is_deleted,  
                ty.description,  
                dt.effective_date  
        FROM    document_template dt  
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id  
        JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id  
        WHERE   wrtl.risk_type_id = @risk_type_id  and dt.is_deleted=0 and dt.effective_date <getdate()  
        AND ISNULL(dt.document_filter,'') = ISNULL(@document_filter,'')  
        AND (dt.copy_of_original<>1 OR dt.copy_of_original IS NULL)  
        AND (@document_template_id is null or dt.document_template_id = @document_template_id)
        ORDER BY dt.code  
    END  
ELSE  
    IF ISNULL(@gis_property_id,' ')=' ' OR ISNULL(@gis_object_id,' ')=' '  
        SELECT  dt.document_template_id,  
                dt.code,  
                dt.description,  
                wrtl.[default]  ,  
                wrtl.branch_id,  
                ty.document_type_id,  
                ty.code,  
                dt.is_deleted,  
                ty.description,  
                dt.effective_date  
        FROM    document_template dt  
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id  
        JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id  
        WHERE   wrtl.risk_type_id = @risk_type_id  and dt.is_deleted=0 and dt.effective_date <getdate()  
        AND     dt.effective_date = ( SELECT MAX(effective_date )  
                                      FROM document_template  
                                      WHERE code = @code  
                                        AND is_deleted = 0  
                                        AND effective_date < @effective_date
                                        AND (@document_template_id is null or document_template_id = @document_template_id)  
                                     )  
        AND dt.code = @code
        AND (@document_template_id is null or dt.document_template_id = @document_template_id)
        ORDER BY dt.code  
    ELSE  
    BEGIN  
        SELECT @document_filter=(SELECT gisp.specials_type_reference FROM GIS_Property gisp  
            WHERE gisp.gis_property_id=@gis_property_id AND gisp.gis_object_id=@gis_object_id)  
  
        SELECT  dt.document_template_id,  
                dt.code,  
                dt.description,  
                wrtl.[default]  ,  
                wrtl.branch_id,  
                ty.document_type_id,  
                ty.code,  
                dt.is_deleted,  
                ty.description,  
                dt.effective_date  
        FROM    document_template dt  
        JOIN    document_type ty ON ty.document_type_id = dt.document_type_id  
        JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id  
        WHERE   wrtl.risk_type_id = @risk_type_id  and dt.is_deleted=0 and dt.effective_date <getdate()  
        AND     dt.effective_date = ( SELECT MAX(effective_date )  
                                      FROM document_template  
                                      WHERE code = @code  
                                        AND is_deleted = 0  
                                        AND effective_date < @effective_date
                                        AND (@document_template_id is null or document_template_id = @document_template_id)
                                        )  
                                        
        AND dt.code = @code  
        AND ISNULL(dt.document_filter,'') = ISNULL(@document_filter,'')  
        AND (dt.copy_of_original<>1 OR dt.copy_of_original IS NULL)  
        AND (@document_template_id is null or dt.document_template_id = @document_template_id)
        ORDER BY dt.code  
    END  
END  
  
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
          AND  (dt.copy_of_original<>1 OR dt.copy_of_original IS NULL)  
          AND  (@document_template_id is null or dt.document_template_id = @document_template_id)
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
          AND  (@document_template_id is null or dt.document_template_id = @document_template_id)
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
        AND (@document_template_id is null or dt.document_template_id = @document_template_id)
        ORDER BY dt.code  
    END  
END   

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.3)     
