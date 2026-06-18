
--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.2)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'Spu_Product_Linked_Clauses_Sel'
GO
CREATE PROCEDURE Spu_Product_Linked_Clauses_Sel     
    @product_ID int,
    @Branch_ID int,
	@CoverToDate varchar(25) = '',
    @Default tinyint=NULL,
    @is_visible_from_client_manager tinyint = NULL,
    --Arul PN 63627
	@SearchCode varchar(10)=NULL

AS

  IF(ISNULL(@Default,' ')=' ')
    BEGIN
	   IF(@CoverToDate <> '')
	   BEGIN
		SELECT   dt.document_template_id,
                dt.code,
                dt.description,
                dt.is_deleted,
                ty.document_type_id,
                ty.code,
                ty.description,
                dt.effective_date
         FROM    document_template dt
         JOIN    document_type ty ON ty.document_type_id = dt.document_type_id
         JOIN    wording_product_link wpl ON wpl.document_template_id = dt.document_template_id
         WHERE   wpl.product_ID = @product_ID  and wpl.branch_id=@branch_id
         AND     dt.effective_date IN ( SELECT MAX(effective_date)
		FROM document_template
		WHERE document_template.code = dt.code
        AND (CONVERT(DATETIME,(CONVERT(VARCHAR,document_template.effective_date,106))) < = @CoverToDate) -- Cover End Date
        AND is_deleted=0)
        AND dt.is_deleted=0
	     AND (dt.is_visible_from_client_manager=@is_visible_from_client_manager or @is_visible_from_client_manager is null)
		 --Arul PN 63627
		 and ( @SearchCode IS NULL or dt.code like  RTRIM(LTRIM(@SearchCode)))
         ORDER BY dt.code
	   END
	   ELSE
	   BEGIN
		SELECT   dt.document_template_id,
                dt.code,
                dt.description,
                dt.is_deleted,
                ty.document_type_id,
                ty.code,
                ty.description,
                dt.effective_date
         FROM    document_template dt
         JOIN    document_type ty ON ty.document_type_id = dt.document_type_id
         JOIN    wording_product_link wpl ON wpl.document_template_id = dt.document_template_id
         WHERE   wpl.product_ID = @product_ID  and wpl.branch_id=@branch_id
         AND     dt.effective_date < getdate() and dt.is_deleted=0
	     AND (dt.is_visible_from_client_manager=@is_visible_from_client_manager or @is_visible_from_client_manager is null)
		 --Arul PN 63627
		 and ( @SearchCode IS NULL or dt.code like  RTRIM(LTRIM(@SearchCode)))
         ORDER BY dt.code
		END
    END
  ELSE
    BEGIN
	  IF(@CoverToDate <> '')
	  BEGIN
			SELECT    dt.document_template_id,
					dt.code,
					dt.description,
					dt.is_deleted,
					ty.document_type_id,
					ty.code,
					ty.description,
					dt.effective_date
			 FROM    document_template dt
			 JOIN   document_type ty ON ty.document_type_id = dt.document_type_id
			 JOIN   wording_product_link wpl ON wpl.document_template_id = dt.document_template_id
			 WHERE   wpl.product_ID = @product_ID  and wpl.branch_id=@branch_id
			 AND wpl.[default]=@Default
			 AND     dt.effective_date IN ( SELECT MAX(effective_date)
			 FROM document_template
			 WHERE document_template.code = dt.code
			 AND (CONVERT(DATETIME,(CONVERT(VARCHAR,document_template.effective_date,106))) < = @CoverToDate) -- Cover End Date
			 AND is_deleted=0)
			 AND dt.is_deleted=0
	 		 AND (dt.is_visible_from_client_manager=@is_visible_from_client_manager or @is_visible_from_client_manager is null)
			 --Arul PN 63627
			 and ( @SearchCode IS NULL or dt.code like  RTRIM(LTRIM(@SearchCode)))
			 ORDER BY dt.code
	  END
	  ELSE
	  BEGIN
			SELECT    dt.document_template_id,
					dt.code,
					dt.description,
					dt.is_deleted,
					ty.document_type_id,
					ty.code,
					ty.description,
					dt.effective_date
			 FROM    document_template dt
			 JOIN   document_type ty ON ty.document_type_id = dt.document_type_id
			 JOIN   wording_product_link wpl ON wpl.document_template_id = dt.document_template_id
			 WHERE   wpl.product_ID = @product_ID  and wpl.branch_id=@branch_id
			 AND wpl.[default]=@Default
			 AND     dt.effective_date < getdate() and dt.is_deleted=0
	 		 AND (dt.is_visible_from_client_manager=@is_visible_from_client_manager or @is_visible_from_client_manager is null)
			 --Arul PN 63627
			 and ( @SearchCode IS NULL or dt.code like  RTRIM(LTRIM(@SearchCode)))
			 ORDER BY dt.code
      END
    END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.2)   
    
  

    
  
