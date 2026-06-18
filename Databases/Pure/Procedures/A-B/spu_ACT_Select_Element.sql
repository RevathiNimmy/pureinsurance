SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Element'
GO


CREATE PROCEDURE spu_ACT_Select_Element
    @element_id int = NULL,
    @element_name varchar(30) = NULL,
    @company_id int = NULL
AS

DECLARE @itl_company_id int
SET @itl_company_id = @company_id


/* SP180100 - tune this SQL */
IF ISNULL(@itl_company_id, 0) = 0
BEGIN
    IF (@element_id IS NULL)
    BEGIN
        SELECT e.element_id,
               e.element_name,
               e.parent_id,
               st.company_id
        FROM   Element e
        JOIN   StructureTree st On st.element_id = e.element_id
        WHERE  e.element_name = @element_name
    END
    
    IF (@element_name IS NULL)
    BEGIN
        SELECT e.element_id,
               e.element_name,
               e.parent_id,
               st.company_id
        FROM   Element e
        JOIN   StructureTree st On st.element_id = e.element_id
        WHERE  e.element_id = @element_id
    END
END
ELSE
BEGIN
    IF (@element_id IS NULL)
    BEGIN
        SELECT distinct(e.element_id),
               e.element_name,
               e.parent_id,
               st.company_id
        FROM   Element e
        JOIN   StructureTree st On st.element_id = e.element_id
        WHERE  e.element_name = @element_name
        AND    st.company_id = @itl_company_id
    END
    
    IF (@element_name IS NULL)
    BEGIN
        SELECT e.element_id,
               e.element_name,
               e.parent_id,
               st.company_id
        FROM   Element e
        JOIN   StructureTree st On st.element_id = e.element_id
        WHERE  e.element_id = @element_id
        AND    st.company_id = @itl_company_id
    END
END

GO


