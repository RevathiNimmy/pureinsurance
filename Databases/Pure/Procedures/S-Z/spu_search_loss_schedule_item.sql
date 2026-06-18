SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_search_loss_schedule_item'
GO


CREATE PROCEDURE spu_search_loss_schedule_item
    @category varchar(20),
    @searchstring varchar(255),
    @majoritemcategoryid int,
    @minoritemcategoryid int,
    @manuname varchar(255),
    @Effective_Date datetime,
    @ReturnAll bit = 0

AS

IF @category = 'major'
    BEGIN  
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            IC.item_category_id,
            IC.category_description
         FROM Item_Categories IC
         WHERE IC.Effective_Date = (SELECT Max(IC2.Effective_Date) as Effective_Date
                                      FROM Item_Categories IC2
                                     WHERE IC.category_description = IC2.category_description)
           AND IC.category_description LIKE @searchstring
           AND IC.parent_category_id IS NULL
           AND IC.is_deleted = 0
           AND IC.effective_date <= @Effective_Date
        ORDER BY IC.category_description
        END
    Else
        BEGIN
        SELECT DISTINCT
            item_category_id,
            category_description
          FROM Item_Categories IC
         WHERE IC.category_description LIKE @searchstring
           AND IC.parent_category_id IS NULL
         ORDER BY IC.category_description
        END
    END

IF @category = 'minor'
    BEGIN
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            IC.item_category_id,
            IC.category_description,
            'dummy',
            IC.parent_category_id
          FROM Item_Categories IC
         WHERE IC.Effective_Date = (SELECT Max(IC2.Effective_Date) As Effective_Date
                                      FROM Item_Categories IC2
                                     WHERE IC2.category_description = IC.category_description)
           AND IC.category_description LIKE @searchstring
           AND IC.parent_category_id = @majoritemcategoryid
           AND IC.is_deleted = 0
           AND IC.effective_date <= @Effective_Date
        ORDER BY IC.category_description
        END
    Else
        BEGIN
        SELECT DISTINCT
            item_category_id,
            category_description,
            'dummy',
            parent_category_id
          FROM Item_Categories IC
         WHERE IC.category_description LIKE @searchstring
           AND IC.parent_category_id = @majoritemcategoryid
         ORDER BY IC.category_description
        END
    END

IF @category = 'manu'
    BEGIN    
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            'Dummy',
            ID.manufacturer_name
          FROM Item_Details ID
         WHERE ID.Effective_Date = (SELECT Max(ID2.Effective_Date) As Effective_Date
                                      FROM Item_Details ID2
                                     WHERE ID2.manufacturer_name = ID.manufacturer_name)
           AND ID.manufacturer_name LIKE @searchstring
           AND ID.major_category_id = @majoritemcategoryid
           AND ID.minor_category_id = @minoritemcategoryid
           AND ID.is_deleted = 0
           AND ID.effective_date <= @Effective_Date
         ORDER BY ID.manufacturer_name
         END
    Else
        BEGIN
        SELECT DISTINCT
            'Dummy',
            manufacturer_name
          FROM Item_Details ID
         WHERE ID.manufacturer_name LIKE @searchstring
           AND ID.major_category_id = @majoritemcategoryid
           AND ID.minor_category_id = @minoritemcategoryid
        END
    END


IF @category = 'model'
    BEGIN
    If @ReturnAll = 0
        BEGIN    
        SELECT DISTINCT
            ID.item_detail_id,
            ID.description,
            ID.model
          FROM Item_Details ID
         WHERE ID.Effective_Date = (SELECT Max(ID2.Effective_Date) As Effective_Date
                                      FROM Item_Details ID2
                                     WHERE ID2.manufacturer_name = ID.manufacturer_name)
           AND ID.manufacturer_name LIKE @manuname
           AND ID.major_category_id = @majoritemcategoryid
           AND ID.minor_category_id = @minoritemcategoryid
           AND ID.is_deleted = 0
           AND ID.effective_date <= @Effective_Date
        ORDER BY ID.model
        END
    Else
        BEGIN
        SELECT DISTINCT
            item_detail_id,
            description,
            model
          FROM Item_Details ID
         WHERE ID.manufacturer_name LIKE @manuname
           AND ID.major_category_id = @majoritemcategoryid
           AND ID.minor_category_id = @minoritemcategoryid
         ORDER BY ID.model
         END
    END

IF @category = 'minorsearch'
    BEGIN    
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            IC.item_category_id,
            IC.category_description,
            'dummy',
            IC.parent_category_id
          FROM Item_Categories IC
         WHERE IC.Effective_Date = (SELECT Max(IC2.Effective_Date) As Effective_Date
                                      FROM Item_Categories IC2
                                     WHERE IC2.category_description = IC.category_description)
           AND IC.category_description LIKE @searchstring
           AND IC.parent_category_id IS NOT NULL
           AND IC.is_deleted = 0
           AND IC.effective_date <= @Effective_Date
         ORDER BY IC.category_description
         END
    Else
        BEGIN
        SELECT DISTINCT
            item_category_id,
            category_description,
            'dummy',
            parent_category_id
          FROM Item_Categories IC
         WHERE IC.category_description LIKE @searchstring
           AND IC.parent_category_id IS NOT NULL
         ORDER BY IC.category_description
         END
    END

IF @category = 'manusearch'
    BEGIN    
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            'dummy',
            ID.manufacturer_name
          FROM Item_Details ID
         WHERE ID.Effective_Date = (SELECT Max(ID2.Effective_Date) As Effective_Date
                                      FROM Item_Details ID2
                                     WHERE ID2.manufacturer_name = ID.manufacturer_name )
           AND ID.manufacturer_name LIKE @searchstring
           AND ID.is_deleted = 0
           AND ID.effective_date <= @Effective_Date
         ORDER BY ID.manufacturer_name
        END
    Else
        BEGIN
        SELECT DISTINCT
            'dummy',
            manufacturer_name
          FROM Item_Details ID
         WHERE ID.manufacturer_name LIKE @searchstring
         ORDER BY ID.manufacturer_name
        END
    END

IF @category = 'modelsearch'
    BEGIN    
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            ID.item_detail_id,
            ID.description,
            ID.model
         FROM Item_Details ID
         WHERE ID.Effective_Date = (SELECT Max(ID2.Effective_Date) As Effective_Date
                                      FROM Item_Details ID2
                                     WHERE (ID2.model = ID.model) OR (ID2.description = ID.description))
           AND(ID.model LIKE @searchstring
               OR ID.description LIKE @searchstring)
           AND ID.is_deleted = 0
           AND ID.effective_date <= @Effective_Date
        ORDER BY ID.model
        END
    Else
        BEGIN
        SELECT DISTINCT
            item_detail_id,
            description,
            model
         FROM Item_Details ID
        WHERE (ID.model LIKE @searchstring
           OR ID.description LIKE @searchstring)
        ORDER BY ID.model
        END
    END

IF @category = 'manumodelsearch'
    BEGIN    
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            ID.item_detail_id,
            ID.description,
            ID.model
          FROM Item_Details ID
         WHERE ID.Effective_Date = (SELECT Max(ID2.Effective_Date) As Effective_Date
                                      FROM Item_Details ID2
                                     WHERE ID2.manufacturer_name = ID.manufacturer_name)
           AND ID.manufacturer_name LIKE @manuname
           AND ID.is_deleted = 0
           AND ID.effective_date <= @Effective_Date
         ORDER BY ID.model
        END
    Else
        BEGIN
        SELECT DISTINCT
            item_detail_id,
            description,
            model
          FROM Item_Details ID
         WHERE ID.manufacturer_name LIKE @manuname
         ORDER BY ID.model
        END
    END

IF @category = 'QuickSearch'
    BEGIN
    If @ReturnAll = 0
        BEGIN
        SELECT DISTINCT
            ID.item_detail_id,
            ID.model,
            ID.description,
            'dummy',
            ID.manufacturer_name,
            ID.price
          FROM Item_Details ID
         WHERE ID.Effective_Date = (SELECT Max(ID2.Effective_Date) As Effective_Date
                                      FROM Item_Details ID2
                                     WHERE (ID2.model = ID.model) OR (ID2.description = ID.description) OR (ID2.manufacturer_name = ID.manufacturer_name))
           AND (ID.model LIKE @searchstring
              OR ID.description LIKE @searchstring
              OR ID.manufacturer_name LIKE @searchstring)
           AND ID.is_deleted = 0
           AND ID.effective_date <= @Effective_Date
         ORDER BY ID.model
        END
    Else
        BEGIN
        SELECT DISTINCT
            item_detail_id,
            model,
            description,
            'dummy',
            manufacturer_name,
            price,
            supplier_id,
            effective_date,
            is_deleted
          FROM Item_Details ID
         WHERE (ID.model LIKE @searchstring
               OR ID.description LIKE @searchstring
               OR ID.manufacturer_name LIKE @searchstring)
         ORDER BY ID.model
        END
    END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

